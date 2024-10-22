using KSozluk.Application.Services.Repositories;
using KSozluk.Domain;
using KSozluk.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.Persistence.Repositories
{
    public class FavouriteWordRepository : IFavouriteWordRepository
    {
        private readonly SozlukContext _context;
        public FavouriteWordRepository(SozlukContext context) { 
            _context = context;
        }
        public async Task CreateAsync(FavouriteWord entity)
        {
            await _context.AddAsync(entity);
        }

        public void Delete(FavouriteWord entity)
        {
            _context.Remove(entity);
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<FavouriteWord> FindAsync(Guid? id)
        {
            return await _context.FavouriteWords.SingleOrDefaultAsync(d => d.Id == id);
        }

        public async Task<FavouriteWord> GetByFavouriteWordAndUserAsync(Guid _wordId, Guid _userId)
        {
            var favouriteWord = await _context.FavouriteWords.FirstOrDefaultAsync(x => x.WordId == _wordId && x.UserId == _userId);
            if (favouriteWord == null)
            {
                return null;
            }
            return favouriteWord;
        }
    }
}
