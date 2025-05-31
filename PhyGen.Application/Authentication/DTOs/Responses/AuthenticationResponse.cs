using PhyGen.Shared.Constants;

namespace PhyGen.Application.Authentication.Responses
{
    public class AuthenticationResponse
    {
        public string Email { get; set; }
        public StatusCode StatusCode { get; set; }
        public string Message => ResponseMessages.GetMessage(StatusCode);
    }
}
