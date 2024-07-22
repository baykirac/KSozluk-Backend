using KSozluk.Application.Common;

namespace KSozluk.Application.Features.Users.Commands.SignIn
{
    public class SignInCommand : CommandBase<SignInResponse>
    {
        public string Email { get; init; }
        public string Password { get; init; }
    }
}
