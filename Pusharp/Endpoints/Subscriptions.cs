using Pusharp.Entities;
using Pusharp.Models;
using Pusharp.RequestParameters;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pusharp
{
    public partial class PushBulletClient
    {
        public async Task<IReadOnlyCollection<Subscription>> GetSubscriptionsAsync()
        {
            var subscriptionsModel = await RequestClient
                .SendAsync<SubscriptionsModel>("/v2/subscriptions", HttpMethod.Get, null)
                .ConfigureAwait(false);

            var subscriptions = subscriptionsModel.Subscriptions.Select(x => new Subscription(x, this));

            return subscriptions.ToImmutableList();
        }

        public async Task<Subscription> CreateSubscriptionAsync(SubscriptionParameters subscriptionParameters)
        {
            var subscriptionModel = await RequestClient
                .SendAsync<SubscriptionModel>("/v2/subscriptions", HttpMethod.Post, subscriptionParameters)
                .ConfigureAwait(false);

            return new Subscription(subscriptionModel, this);
        }

        public async Task<ChannelInfo> GetChannelInfoAsync(ChannelInfoParameters channelInfoParameters)
        {
            var channelInfoModel = await RequestClient
                .SendAsync<ChannelInfoModel>("/v2/channel-info", HttpMethod.Get, channelInfoParameters)
                .ConfigureAwait(false);

            return new ChannelInfo(channelInfoModel, RequestClient);
        }
    }
}
