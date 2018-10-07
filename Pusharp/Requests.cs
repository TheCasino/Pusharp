using Pusharp.RequestParameters;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Voltaic.Serialization.Json;

namespace Pusharp
{
    internal class Requests
    {
        private readonly HttpClient _client;
        private readonly JsonSerializer _serializer;

        public Requests(string accessToken)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://api.pushbullet.com")
            };

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("Access-Token", accessToken);

            _serializer = new JsonSerializer();
        }

        public async Task<T> GetRequestAsync<T>(string endpoint)
        {
            using (var get = await _client.GetAsync(endpoint).ConfigureAwait(false))
            {
                //for testing
                try
                {
                    if (!get.IsSuccessStatusCode)
                        throw new Exception("gud exception");

                    return await HandleResponseAsync<T>(get).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return default;
                }
            }
        }

        public async Task<TModel> PostRequestAsync<TModel, TParam>(string endpoint, BaseRequest parameters) where TParam : BaseRequest
        {
            //TODO verify parameters
            var requestContent = new StringContent(_serializer.WriteUtf16String(parameters as TParam), Encoding.UTF8, "application/json");

            using (var post = await _client.PostAsync(endpoint, requestContent).ConfigureAwait(false))
            {
                //for testing
                try
                {
                    if (!post.IsSuccessStatusCode)
                        throw new Exception(await post.Content.ReadAsStringAsync());

                    return await HandleResponseAsync<TModel>(post).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return default;
                }
            }
        }

        private async Task<T> HandleResponseAsync<T>(HttpResponseMessage message)
        {
            var result = await message.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            Console.WriteLine(result);
            return _serializer.ReadUtf8<T>(new ReadOnlySpan<byte>(result));
        }
    }
}
