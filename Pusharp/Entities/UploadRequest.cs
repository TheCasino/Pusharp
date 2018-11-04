using System;
using System.Net.Http;
using System.Threading.Tasks;
using Pusharp.RequestParameters;
using Model = Pusharp.Models.UploadRequestModel;

namespace Pusharp.Entities
{
    public sealed class UploadRequest
    {
        private readonly Model _model;
        private readonly RequestClient _client;

        public string FileName => _model.FileName;
        public string FileType => _model.FileType;

        private Uri _fileUrl;
        public Uri FileUrl => _fileUrl ?? (_fileUrl = new Uri(_model.FileUrl));

        private Uri _uploadUrl;
        public Uri UploadUrl => _uploadUrl ?? (_uploadUrl = new Uri(_model.UploadUrl));

        internal UploadRequest(Model model, RequestClient client)
        {
            _model = model;
            _client = client;
        }

        public async Task UploadAsync()
        {
            await _client.SendAsync(UploadUrl.AbsoluteUri, HttpMethod.Post, new FileUploadParameters())
                .ConfigureAwait(false);
        }
    }
}
