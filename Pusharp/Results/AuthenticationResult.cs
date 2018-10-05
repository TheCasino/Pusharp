using System;
using Model = PBSharp.Models.AuthenticationModel;

namespace PBSharp.Results
{
    public class AuthenticationResult
    {
        private readonly Model _model;

        public bool IsActive => _model.Active;

        public string Name => _model.Name;
        public string Identifier => _model.Iden;
        public string Email => _model.Email;
        public string NormalizedEmail => _model.EmailNormalized;

        //TODO parse this 'better'
        public long MaxUploadSize => _model.MaxUploadSize;

        public Uri ImageUrl => new Uri(_model.ImageUrl);

        public DateTimeOffset Created => DateTimeOffset.FromUnixTimeMilliseconds((long)_model.Created);
        public DateTimeOffset Modified => DateTimeOffset.FromUnixTimeMilliseconds((long)_model.Modified);

        internal AuthenticationResult(Model model)
        {
            _model = model;
        }
    }
}
