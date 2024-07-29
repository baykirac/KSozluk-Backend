using KSozluk.Domain;
using KSozluk.Domain.SharedKernel;

namespace KSozluk.Application.Services.Repositories
{
    public interface IUserRepository : IDomainRepository<User>
    {
        Task<User> FindByEmailAsync(string email);
        public Task<bool> HasPermissionView(Guid id);
        public Task<bool> HasPermissionForAdmin(Guid id);
    }
}
