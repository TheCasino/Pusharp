using Pusharp.RequestParameters;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Pusharp.Utilities;
using Voltaic.Serialization.Json;

namespace Pusharp
{
    internal class Requests
    {
        public const int FreeAccountPushLimit = 500; // Pushbullet Free accounts are limited to 500 pushes per month.

        private readonly HttpClient _client;
        private readonly JsonSerializer _serializer;
        private readonly SemaphoreSlim _semaphore;

        private const string RatelimitLimitHeaderName = "X-Ratelimit-Limit";
        private const string RatelimitRemainingHeaderName = "X-Ratelimit-Remaining";
        private const string RatelimitResetHeaderName = "X-Ratelimit-Reset";

        private string _rateLimit;
        private string _rateLimitReset;

        //default to 1 because the initial ping doesn't use a request
        private string _remaining = "1";

        private int RateLimit => int.TryParse(_rateLimit, out var value) ? value : 0;
        private int Remaining => int.TryParse(_remaining, out var amount) ? amount : 0;

        public PushBulletClient PushBulletClient { get; set; } // TODO: Something nicer

        private DateTimeOffset RateLimitReset => DateTimeOffset.FromUnixTimeSeconds(long.TryParse(_rateLimitReset, out var seconds) ? seconds : 0);

        public Requests(string accessToken, PushBulletClientConfig config)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(config.ApiBaseUrl)
            };

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("Access-Token", accessToken);

            _serializer = new JsonSerializer();
            _semaphore = new SemaphoreSlim(1, 1);
        }

        public Task<T> SendAsync<T>(string endpoint)
            => SendAsync<T>(endpoint, HttpMethod.Get, false, 0, null);

        public async Task<T> SendAsync<T>(string endpoint, HttpMethod method, bool isDatabaseRequest, int hits, BaseRequest parameters)
        {
            await _semaphore.WaitAsync().ConfigureAwait(false);

            if(CalculateCost(isDatabaseRequest, hits) > Remaining)
            {
                _semaphore.Release();
                await PushBulletClient.InternalLogAsync(new LogMessage(LogLevel.Warning, $"Hit pre-emptive ratelimit on {endpoint}!"));
            }

            var request = new HttpRequestMessage(method, endpoint);
            parameters = parameters ?? EmptyParameters.Create();
            parameters.VerifyParameters();
            request.Content = new StringContent(parameters.BuildContent(_serializer), Encoding.UTF8, "application/json");
            var requestTime = Stopwatch.StartNew();
            using (var response = await _client.SendAsync(request).ConfigureAwait(false))
            {
                requestTime.Stop();
                if (PushBulletClient != null) await PushBulletClient.InternalLogAsync(new LogMessage(LogLevel.Verbose, $"{method} {endpoint}: {requestTime.ElapsedMilliseconds}ms"));
                ParseResponseHeaders(response);

                //TODO response.StatusCode parsing

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                return await HandleResponseAsync<T>(response).ConfigureAwait(false);
            }
        }

        private static int CalculateCost(bool isDatabaseRequest, int hits = 0)
            => 1 + (isDatabaseRequest ? 4 : 0) * hits;

        private void ParseResponseHeaders(HttpResponseMessage message)
        {
            var headers = message.Headers.ToDictionary(x => x.Key, x => x.Value.FirstOrDefault(),
                StringComparer.OrdinalIgnoreCase);

            headers.TryGetValue(RatelimitLimitHeaderName, out _rateLimit);
            headers.TryGetValue(RatelimitResetHeaderName, out _rateLimitReset);
            headers.TryGetValue(RatelimitRemainingHeaderName, out _remaining);

            Console.WriteLine($"Ratelimit: {RateLimit}\n" +
                              $"Remaining: {Remaining}\n" +
                              $"Reset: {RateLimitReset}");
        }

        private async Task<T> HandleResponseAsync<T>(HttpResponseMessage message)
        {
            var result = await message.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

            _semaphore.Release();
            return _serializer.ReadUtf8<T>(new ReadOnlySpan<byte>(result));
        }
    }
}
