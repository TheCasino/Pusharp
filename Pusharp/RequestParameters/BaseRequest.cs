namespace Pusharp.RequestParameters
{
    public abstract class BaseRequest : IRequestParameters
    {
        public abstract bool VerifyParameters();
    }
}
