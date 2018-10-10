using Voltaic.Serialization;

namespace Pusharp.Models
{
    internal class SubscriptionsModel
    {
        [ModelProperty("subscriptions")]
        public SubscriptionModel[] Subscriptions { get; set; }
    }
}
