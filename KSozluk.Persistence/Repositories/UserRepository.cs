using KSozluk.Application.Services.Repositories;
using KSozluk.Domain;
using KSozluk.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace KSozluk.Persistence.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly SozlukContext _context;

        public UserRepository(SozlukContext context)
        {
            _context = context;
        }


        public async Task CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public Task<User> FindAsync(Guid id)
        {
            return _context.Users.SingleOrDefaultAsync(u => u.Id == id);    
        }

        public Task<User> FindByEmailAsync(string email)
        {
            return _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> HasPermissionView(Guid id)
        {
            var user = await FindAsync(id);            

            return user.Permissions == Permission.NormalUser || user.Permissions == Permission.Admin;
        }
    }
}
