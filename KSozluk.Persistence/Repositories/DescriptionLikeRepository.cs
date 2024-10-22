using KSozluk.Application.Services.Repositories;
using KSozluk.Domain;
using KSozluk.Domain.SharedKernel;
using KSozluk.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.Persistence.Repositories
{
    public class DescriptionLikeRepository : IWordLikeRepository
    {

        private readonly SozlukContext _context;

        public DescriptionLikeRepository(SozlukContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(DescriptionLike entity)
        {
            await _context.DescriptionLikes.AddAsync(entity);
        }

        public void Delete(DescriptionLike entity)
        {
            _context.DescriptionLikes.Remove(entity);
        }

        public async Task<DescriptionLike> FindAsync(Guid? id)
        {
            return await _context.DescriptionLikes.SingleOrDefaultAsync(d => d.Id == id);
        }  

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<DescriptionLike> GetByDescriptionAndUserAsync(Guid _descriptionId, Guid _userId)
        {
            var _descriptionLike = await _context.DescriptionLikes.FirstOrDefaultAsync(x => x.UserId == _userId && x.DescriptionId == _descriptionId);

            if (_descriptionLike == null) {
                return null;
            }

            return _descriptionLike;
        
        }
    }
}
