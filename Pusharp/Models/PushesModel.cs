using Voltaic.Serialization;

namespace Pusharp.Models
{
    internal class PushesModel
    {
        [ModelProperty("pushes")]
        public PushModel[] Pushes { get; set; }
    }
}
