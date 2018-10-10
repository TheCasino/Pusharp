using System;
using Model = Pusharp.Models.ChannelModel;

namespace Pusharp.Entities
{
    public class Channel
    {
        private readonly Model _model;

        public string Identifier => _model.Identifier;
        public string Tag => _model.Tag;
        public string Name => _model.Name;
        public string Description => _model.Description;

        private Uri _imageUrl;
        public Uri ImageUrl => _imageUrl ?? (_imageUrl = new Uri(_model.ImageUrl));

        private Uri _websiteUrl;
        public Uri WebsiteUrl => _websiteUrl ?? (_websiteUrl = new Uri(_model.WebsiteUrl));

        internal Channel(Model model)
        {
            _model = model;
        }
    }
}
