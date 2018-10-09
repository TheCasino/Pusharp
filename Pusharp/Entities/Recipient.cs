using System;
using Model = Pusharp.Models.With;

namespace Pusharp.Entities
{
    public class Recipient
    {
        private readonly Model _model;

        public string Identifier => _model.Identifier;
        public string Email => _model.Email;
        public string NormalizedEmail => _model.NormalizedEmail;
        public string Type => _model.Type; //TODO enum
        public string Name => _model.Name;

        private Uri _imageUrl;
        //stops a new uri being made each call
        public Uri ImageUrl => _imageUrl ?? (_imageUrl = new Uri(_model.ImageUrl));

        internal Recipient(Model model)
        {
            _model = model;
        }
    }
}
