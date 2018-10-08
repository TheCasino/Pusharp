using System.Net.Http;
using System.Threading.Tasks;
using Pusharp.Models;
using Pusharp.RequestParameters;

namespace Pusharp
{
    public partial class PushBulletClient
    {
        /// <summary>
        ///     Sends an SMS message to a device.
        /// </summary>
        /// <param name="parameters">The parameters to use when sending the message.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous send operation.</returns>
        public async Task SendSmsAsync(SmsParameters parameters)
            => await _requests.SendAsync<EmptyModel>("/ephemerals", HttpMethod.Post, true, 1, parameters)
                .ConfigureAwait(false);
    }
}
