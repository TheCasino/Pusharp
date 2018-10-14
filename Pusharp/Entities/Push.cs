using System;
using System.Net.Http;
using System.Threading.Tasks;
using Pusharp.RequestParameters;
using Pusharp.Utilities;
using Model = Pusharp.Models.PushModel;

namespace Pusharp.Entities
{
    public class Push
    {
        private Model _model;
        private readonly RequestClient _client;

        public string Identifier => _model.Identifier;
        public string Guid => _model.Guid;
        public string Type => _model.Type;
        public string TargetDeviceIdentifier => _model.TargetDeviceIdentifier;
        public string SourceDeviceIdentifier => _model.SourceDeviceIdentifier;
        public string ClientIdentifier => _model.ClientIdentifier;
        public string Title => _model.Title;
        public string Body => _model.Body;
        public string Url => _model.Url;

        public string[] AwakeAppGuids => _model.AwakeAppGuids;

        private Sender _sender;
        public Sender Sender => _sender ?? (_sender = new Sender(_model));

        private Receiver _receiver;
        public Receiver Receiver => _receiver ?? (_receiver = new Receiver(_model));

        public PushDirection Direction
        {
            get
            {
                switch (_model.Direction)
                {
                    case "self":
                        return PushDirection.Self;

                    case "outgoing":
                        return PushDirection.Outgoing;

                    case "incoming":
                        return PushDirection.Incoming;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(_model.Direction));
                }
            }
        }

        public bool IsActive => _model.IsActive;
        public bool IsDismissed => _model.IsDismissed;

        public DateTimeOffset Created => DateTimeHelpers.ToDateTime(_model.Created);
        public DateTimeOffset Modified => DateTimeHelpers.ToDateTime(_model.Modified);
        internal double InternalModified => _model.Modified;

        private File _file;
        public File File => _file ?? (_file = new File(_model.FileName, _model.FileType, _model.FileUrl));

        private Image _image;
        public Image Image => _image ?? (_image = new Image(_model.ImageUrl, _model.ImageWidth, _model.ImageHeight));

        internal Push(Model model, RequestClient client)
        {
            _model = model;
            _client = client;
        }

        public async Task DismissAsync()
        {
            var pushModel = await _client.SendAsync<Model>($"/v2/pushes/{Identifier}", HttpMethod.Post, new DismissPush
            {
                Dismissed = true
            }).ConfigureAwait(false);

            _model = pushModel;
        }

        public async Task DeleteAsync()
        {
            await _client.SendAsync($"/v2/pushes/{Identifier}", HttpMethod.Delete, null).ConfigureAwait(false);
        }
    }
}
