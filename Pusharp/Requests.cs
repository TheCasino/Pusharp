using Pusharp.RequestParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Voltaic.Serialization.Json;

namespace Pusharp
{
    internal class Requests
    {
        public const int FreeAccountPushLimit = 500; // Pushbullet Free accounts are limited to 500 pushes per month.

        private readonly HttpClient _client;
        private readonly JsonSerializer _serializer;
        private readonly SemaphoreSlim _semaphore;
        private readonly PushBulletClientConfig _config;

        public const string RatelimitLimitHeaderName = "X-Ratelimit-Limit";
        public const string RatelimitRemainingHeaderName = "X-Ratelimit-Remaining";
        public const string RatelimitResetHeaderName = "X-Ratelimit-Reset";

        private int _ratelimitLimit;
        private int _ratelimitRemaining;
        private DateTimeOffset _ratelimitReset;

        public Requests(string accessToken, PushBulletClientConfig config)
        {
            _config = config;
            _client = new HttpClient
            {
                BaseAddress = new Uri(_config.ApiBaseUrl)
            };

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("Access-Token", accessToken);

            _serializer = new JsonSerializer();
            _semaphore = new SemaphoreSlim(1, 1);
        }

        public async Task<T> SendAsync<T>(string endpoint, HttpMethod method, BaseRequest parameters = null)
        {
            await _semaphore.WaitAsync().ConfigureAwait(false);

            var request = new HttpRequestMessage(method, endpoint);
            parameters = parameters ?? EmptyParameters.Create();
            request.Content = new StringContent(parameters.BuildContent(_serializer), Encoding.UTF8, "application/json");

            using (var response = await _client.SendAsync(request).ConfigureAwait(false))
            {
                ParseResponseHeaders(response);

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                return await HandleResponseAsync<T>(response).ConfigureAwait(false);
            }
        }

        private void ParseResponseHeaders(HttpResponseMessage message)
        {
            if (message.Headers.Contains(RatelimitLimitHeaderName))
            {
                _ratelimitLimit = int.Parse(message.Headers.GetValues(RatelimitLimitHeaderName).FirstOrDefault() ?? throw new ArgumentNullException(RatelimitLimitHeaderName));
            }

            if (message.Headers.Contains(RatelimitRemainingHeaderName))
            {
                _ratelimitRemaining = int.Parse(message.Headers.GetValues(RatelimitRemainingHeaderName).FirstOrDefault() ?? throw new ArgumentNullException(RatelimitRemainingHeaderName));
            }

            if (message.Headers.Contains(RatelimitResetHeaderName))
            {
                _ratelimitReset = DateTimeOffset.FromUnixTimeSeconds(long.Parse(message.Headers.GetValues(RatelimitResetHeaderName).FirstOrDefault() ?? throw new ArgumentNullException(RatelimitResetHeaderName)));
            }
        }

        private async Task<T> HandleResponseAsync<T>(HttpResponseMessage message)
        {
            var result = await message.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

            _semaphore.Release();
            return _serializer.ReadUtf8<T>(new ReadOnlySpan<byte>(result));
        }
    }
}
