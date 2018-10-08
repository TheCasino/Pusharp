using System;
using System.Net.Http;

namespace Pusharp
{
    public class RatelimitedException: Exception
    {
        public RatelimitedException(HttpMethod method, string endpoint, int remaining) : base($"Hit ratelimit on {method.Method} {endpoint} (remaining operations: {remaining})")
        {

        }
    }
}