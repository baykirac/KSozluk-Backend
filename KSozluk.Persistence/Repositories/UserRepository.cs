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


        public async Task CreateAsync(Users user)
        {
            await _context.Users.AddAsync(user);
        }

        public Task<Users> FindAsync(Guid id)
        {
            return _context.Users.SingleOrDefaultAsync(u => u.Id == id);    
        }
    }
}
