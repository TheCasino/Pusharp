using Pusharp.RequestParameters;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pusharp
{
    public partial class PushBulletClient
    {
        /// <summary>
        ///     Sends an SMS message to a device.
        /// </summary>
        /// <param name="smsParameters">The parameters to use when sending the message.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous send operation.</returns>
        public async Task SendSmsAsync(SmsParameters smsParameters)
        {
            await RequestClient.SendAsync("/v2/ephemerals", HttpMethod.Post, smsParameters)
                .ConfigureAwait(false);
        }
    }
}