using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain.SharedKernel;
using KSozluk.Application.Common;
using KSozluk.Domain.Resources;
using KSozluk.Domain;
using System.Security.Claims;
using IdentityModel;

namespace KSozluk.Application.Features.Users.Commands.SignIn
{
    public class SignInCommandHandler : RequestHandlerBase<SignInCommand, SignInResponse>
    {
        private readonly IAuthenticationTokenService _authenticationTokenService;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SignInCommandHandler(IAuthenticationTokenService authenticationTokenService, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _authenticationTokenService = authenticationTokenService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public override async Task<SignInResponse> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindByEmailAsync(request.Email);
            
            if (user is null)
            {
                return Response.Failure<SignInResponse>(OperationMessages.EmailNotFound);
            }

            if (!request.Password.Equals(user.Password))
            {
                return Response.Failure<SignInResponse>(OperationMessages.WrongPassword);
            }

            var tokenModel = _authenticationTokenService.GenerateToken(GetClaims(user));

            user.SignIn(tokenModel.RefreshToken, tokenModel.RefreshTokenExpireDate);

            await _unitOfWork.SaveChangesAsync();

            return Response.SuccessWithBody<SignInResponse>(tokenModel, OperationMessages.LoginSuccessfull);
        }

        private List<Claim> GetClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, user.Id.ToString()),
                new Claim(JwtClaimTypes.Name, user.FullName),
                new Claim(JwtClaimTypes.Email, user.Email),
                new Claim(JwtClaimTypes.Role, ((int)user.Permissions).ToString())
            };

            return claims;
        }
    }
}
