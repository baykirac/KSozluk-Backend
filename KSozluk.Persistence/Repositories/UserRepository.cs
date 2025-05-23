﻿using KSozluk.Application.Services.Repositories;
using KSozluk.Domain;
using KSozluk.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        public Task<User> FindAsync(Guid? id)
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

        public async Task<bool> HasPermissionForAdmin(Guid id)
        {
            var user = await FindAsync(id);

            return user.Permissions == Permission.Admin;
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAll(Expression<Func<User, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetById(Expression<Func<User, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
