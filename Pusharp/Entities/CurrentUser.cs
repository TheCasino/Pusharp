using System;
using System.Net.Http;
using System.Threading.Tasks;
using Pusharp.Utilities;
using Model = Pusharp.Models.CurrentUserModel;

namespace Pusharp.Entities
{
    public sealed class CurrentUser
    {
        private readonly Model _model;
        private readonly RequestClient _client;

        public bool IsActive => _model.Active;

        public string Name => _model.Name;
        public string Identifier => _model.Iden;
        public string Email => _model.Email;
        public string NormalizedEmail => _model.EmailNormalized;
        public string ReferrerIdentifier => _model.ReferrerIdentifier;

        public int ReferredCount => _model.ReferredCount;

        //TODO parse this 'better'
        public long MaxUploadSize => _model.MaxUploadSize;

        public Uri ImageUrl => new Uri(_model.ImageUrl);

        public DateTimeOffset Created => DateTimeHelpers.ToDateTime(_model.Created);
        public DateTimeOffset Modified => DateTimeHelpers.ToDateTime(_model.Created);

        internal CurrentUser(Model model, RequestClient client)
        {
            _model = model;
            _client = client;
        }

        public async Task DeleteAllPushesAsync()
        {
            await _client.SendAsync("/v2/pushes", HttpMethod.Delete, null).ConfigureAwait(false);
        }
    }
}