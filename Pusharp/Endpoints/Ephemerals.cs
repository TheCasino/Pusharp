using System;
using System.Net.Http;
using System.Threading.Tasks;
using Pusharp.RequestParameters;

namespace Pusharp
{
    public partial class PushBulletClient
    {
        /// <summary>
        ///     Sends an SMS message to a device.
        /// </summary>
        /// <param name="parameters">The parameters to use when sending the message.</param>
        /// <returns>A <see cref="Task" /> representing the asynchronous send operation.</returns>
        public async Task SendSmsAsync(SmsParameters parameters)
        {
            await RequestClient.SendAsync("/v2/ephemerals", HttpMethod.Post, parameters)
                .ConfigureAwait(false);
        }

        /// <summary>
        ///     Sends an SMS message to a device.
        /// </summary>
        /// <param name="parameterOperator">
        ///     The <see cref="Action{SmsParameters}" /> that configures the parameters to use when
        ///     sending.
        /// </param>
        /// <returns>A <see cref="Task" /> representing the asynchronous send operation.</returns>
        public Task SendSmsAsync(Action<SmsParameters> parameterOperator)
        {
            var parameters = new SmsParameters();
            parameterOperator(parameters);
            return SendSmsAsync(parameters);
        }
    }
}