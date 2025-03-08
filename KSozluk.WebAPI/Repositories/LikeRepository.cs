using KSozluk.WebAPI.Repositories;
using KSozluk.WebAPI;
using KSozluk.WebAPI.SharedKernel;
using KSozluk.WebAPI.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using KSozluk.WebAPI.Entities;

namespace KSozluk.WebAPI.Repositories
{
    public class LikeRepository : ILikeRepository
    {

        private readonly SozlukContext _context;

        public LikeRepository(SozlukContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(DescriptionLike entity)
        {
            await _context.DescriptionLikes.AddAsync(entity);
        }

        public void DeleteDescriptionLike(DescriptionLike entity)
        {
            _context.DescriptionLikes.Remove(entity);
        }

        public void DeleteWordLike(WordLike entity)
        {
            _context.WordLikes.Remove(entity);
        }  

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<DescriptionLike> GetByDescriptionAndUserAsync(Guid _descriptionId, long? _userId)
        {
            var _descriptionLike = await _context.DescriptionLikes.FirstOrDefaultAsync(x => x.UserId == _userId && x.DescriptionId == _descriptionId);

            if (_descriptionLike == null) {
                return null;
            }

            return _descriptionLike;
        
        }

        public async Task DeleteWordLikesByWordIdAsync(Guid wordId)
        {
            var wordLikes = await _context.WordLikes
                .Where(x => x.WordId == wordId)
                .ToListAsync();

            if (wordLikes.Any())
            {
                _context.WordLikes.RemoveRange(wordLikes);
            }
        }

        public async Task<WordLike> GetByWordAndUserAsync(Guid wordId, long? userId)
        {
            return await _context.WordLikes.FirstOrDefaultAsync(w => w.UserId == userId && w.WordId == wordId);
        }

        public async Task CreateDescriptionLike(DescriptionLike descriptionLike)
        {
            await _context.DescriptionLikes.AddAsync(descriptionLike);
        }
        
        public async Task CreateWordLike(WordLike wordLike)
        {
            await _context.WordLikes.AddAsync(wordLike);
        }

        public async Task<WordLike> FindLikedWordAsync(Guid wordId, long? userId)
        {
            return await _context.WordLikes.FirstOrDefaultAsync(w => w.WordId == wordId && w.UserId == userId);
        }

        public async Task<DescriptionLike> FindLikedDescriptionAsync(Guid descriptionId)
        {
            return await _context.DescriptionLikes.SingleOrDefaultAsync(d => d.DescriptionId == descriptionId);
        }

        public Task<DescriptionLike> FindAsync(Guid? id)
        {
            throw new NotImplementedException();
        }

        public Task<List<DescriptionLike>> GetAll(Expression<Func<DescriptionLike, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<DescriptionLike> GetById(Expression<Func<DescriptionLike, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
