using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Voltaic.Serialization.Json;

namespace PBSharp
{
    internal class Requests
    {
        private readonly HttpClient _client;
        private readonly JsonSerializer _serializer;

        public Requests(string accessToken)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(@"https://api.pushbullet.com")
            };

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("Access-Token", accessToken);

            _serializer = new JsonSerializer();
        }

        public async Task<T> GetRequestAsync<T>(string endpoint)
        {
            using (var post = await _client.GetAsync(endpoint).ConfigureAwait(false))
            {
                //for testing
                try
                {
                    if (!post.IsSuccessStatusCode)
                        throw new Exception("gud exception");

                    var result = await post.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

                    //for testing
                    Console.WriteLine(await post.Content.ReadAsStringAsync());

                    return _serializer.ReadUtf8<T>(new ReadOnlySpan<byte>(result));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return default;
                }
            }
        }
    }
}
