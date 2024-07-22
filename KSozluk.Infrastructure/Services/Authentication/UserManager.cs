using IdentityModel;
using KSozluk.Application.Services.Authentication;
using Microsoft.AspNetCore.Http;

namespace KSozluk.Infrastructure.Services.Authentication
{
    public class UserManager : IUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserManager(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public Guid GetUserId()
        {
            string userId = _contextAccessor.HttpContext.User.Claims.First(c => c.Type == JwtClaimTypes.Subject).Value;

            return Guid.Parse(userId);
        }
    }
}
