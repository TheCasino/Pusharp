using System;
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
        public async Task<IReadOnlyCollection<Push>> GetPushesAsync(Action<PushFilterParameters> pushFilterParameters)
        {
            var parameters = new PushFilterParameters();
            pushFilterParameters(parameters);

            var pushesModel = await RequestClient.SendAsync<PushesModel>("/v2/pushes", HttpMethod.Get, parameters)
                .ConfigureAwait(false);
            
            var pushes = pushesModel.Pushes.Select(x => new Push(x, RequestClient));

            return pushes.ToImmutableList();
        }

        public Task<Push> EmailNoteAsync(Action<NotePushParameters> notePushParameters, string email)
        {
            var parameters = new NotePushParameters();
            notePushParameters(parameters);

            return PushNoteAsync(parameters, PushTarget.Email, email);
        }

        public Task<Push> EmailLinkAsync(Action<LinkPushParameters> linkPushParameters, string email)
        {
            var parameters = new LinkPushParameters();
            linkPushParameters(parameters);

            return PushLinkAsync(parameters, PushTarget.Email, email);
        }

        public Task<Push> EmailFileAsync(Action<FilePushParameters> filePushParameters, string email)
        {
            var parameters = new FilePushParameters();
            filePushParameters(parameters);

            return PushFileAsync(parameters, PushTarget.Email, email);
        }

        internal Task<Push> PushNoteAsync(NotePushParameters parameters, PushTarget target, string identifier)
        {
            parameters.Type = "note";
            return InternalSendPushAsync(parameters, target, identifier);
        }

        internal Task<Push> PushLinkAsync(LinkPushParameters parameters, PushTarget target, string identifier)
        {
            parameters.Type = "link";
            return InternalSendPushAsync(parameters, target, identifier);
        }

        internal Task<Push> PushFileAsync(FilePushParameters parameters, PushTarget target, string identifier)
        {
            parameters.Type = "file";
            return InternalSendPushAsync(parameters, target, identifier);
        }

        private async Task<Push> InternalSendPushAsync(BasePush parameters, PushTarget target, string identifier)
        {
            switch (target)
            {
                case PushTarget.Device:
                    parameters.DeviceIndentifier = identifier;
                    break;

                case PushTarget.Email:
                    parameters.Email = identifier;
                    break;

                case PushTarget.Channel:
                    parameters.ChannelTag = identifier;
                    break;

                case PushTarget.Client:
                    parameters.ClientIdentifier = identifier;
                    break;
            }

            var pushModel = await RequestClient.SendAsync<PushModel>("/v2/pushes", HttpMethod.Post, parameters)
                .ConfigureAwait(false);

            return new Push(pushModel, RequestClient);
        }
    }
}
