using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Model = Pusharp.Models.ChannelInfoModel;

namespace Pusharp.Entities
{
    public sealed class ChannelInfo
    {
        private readonly Model _model;
        private readonly RequestClient _client;

        public string Identifier => _model.Identifier;
        public string Name => _model.Name;
        public string Description => _model.Description;
        public string Tag => _model.Tag;

        private Uri _imageUrl;
        public Uri ImageUrl => _imageUrl ?? (_imageUrl = new Uri(_model.ImageUrl));

        public int SubscribersCount => _model.Subscribers;

        private IReadOnlyCollection<Push> _pushes;

        public IReadOnlyCollection<Push> Pushes =>
            _pushes.ToImmutableList() ?? (_pushes = _model.Pushes.Select(x => new Push(x, _client)).ToImmutableList());

        internal ChannelInfo(Model model, RequestClient client)
        {
            _model = model;
            _client = client;
        }
    }
}
