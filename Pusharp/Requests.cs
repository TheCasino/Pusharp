using Pusharp.RequestParameters;
using System;
using System.Collections.Generic;
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
        private readonly HttpClient _client;
        private readonly JsonSerializer _serializer;
        private readonly SemaphoreSlim _semaphore;

        public Requests(string accessToken)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://api.pushbullet.com")
            };

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("Access-Token", accessToken);

            _serializer = new JsonSerializer();
            _semaphore = new SemaphoreSlim(1, 1);
        }

        public async Task<T> SendAsync<T>(string endpoint, HttpMethod method, BaseRequest parameters = null)
        {
            await _semaphore.WaitAsync();

            var request = new HttpRequestMessage(method, endpoint);
            parameters = parameters ?? EmptyParameters.Create();
            request.Content = new StringContent(parameters.BuildContent(_serializer), Encoding.UTF8, "application/json");

            using (var response = await _client.SendAsync(request).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());

                return await HandleResponseAsync<T>(response).ConfigureAwait(false);
            }
        }

        private async Task<T> HandleResponseAsync<T>(HttpResponseMessage message)
        {
            var result = await message.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            Console.WriteLine(result);

            _semaphore.Release();
            return _serializer.ReadUtf8<T>(new ReadOnlySpan<byte>(result));
        }
    }
}
