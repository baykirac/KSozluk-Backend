using KSozluk.Application.Services.Repositories;
using KSozluk.Domain;
using KSozluk.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.Persistence.Repositories
{
    public class FavoriteWordRepository : IFavoriteWordRepository
    {
        private readonly SozlukContext _context;
        public FavoriteWordRepository(SozlukContext context) { 
            _context = context;
        }
        public async Task CreateAsync(FavoriteWord entity)
        {
            await _context.AddAsync(entity);
        }

        public void Delete(FavoriteWord entity)
        {
            _context.Remove(entity);
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<FavoriteWord> FindAsync(Guid? id)
        {
            return await _context.FavoriteWords.SingleOrDefaultAsync(d => d.Id == id);
        }

        public Task<List<FavoriteWord>> GetAll(Expression<Func<FavoriteWord, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<FavoriteWord> GetByFavoriteWordAndUserAsync(Guid _wordId, Guid _userId)
        {
            var favouriteWord = await _context.FavoriteWords.FirstOrDefaultAsync(x => x.WordId == _wordId && x.UserId == _userId);
            if (favouriteWord == null)
            {
                return null;
            }
            return favouriteWord;
        }

        public Task<FavoriteWord> GetById(Expression<Func<FavoriteWord, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
