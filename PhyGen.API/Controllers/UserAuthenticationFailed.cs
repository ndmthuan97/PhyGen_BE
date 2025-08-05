
namespace PhyGen.API.Controllers
{
    [Serializable]
    internal class UserAuthenticationFailed : Exception
    {
        public UserAuthenticationFailed()
        {
        }

        public UserAuthenticationFailed(string? message) : base(message)
        {
        }

        public UserAuthenticationFailed(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}