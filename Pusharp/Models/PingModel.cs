using Voltaic.Serialization;

namespace Pusharp.Models
{
    internal class PingModel
    {
        [ModelProperty("cat")] public string Cat { get; set; }

        [ModelProperty("happy_to_see_you")] public bool IsHappy { get; set; }

        [ModelProperty("message")] public string Message { get; set; }
    }
}