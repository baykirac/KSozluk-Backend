using KSozluk.Application.Services.Repositories;
using KSozluk.Domain;
using KSozluk.Domain.DTOs;
using KSozluk.Domain.SharedKernel;
using KSozluk.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace KSozluk.Persistence.Repositories
{
    public sealed class DescriptionRepository : IDescriptionRepository
    {
        private readonly SozlukContext _context;

        public DescriptionRepository(SozlukContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Description descriptions)
        {
            await _context.Descriptions.AddAsync(descriptions);
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Description> FindAsync(Guid? id)
        {
            return _context.Descriptions.SingleOrDefaultAsync(d => d.Id == id);
        }

        

        public async Task<Word> FindByDescription(Guid id)
        {
            var description = await _context.Descriptions
             .Include(d => d.Word) 
             .SingleOrDefaultAsync(d => d.Id == id);

            return description?.Word;
        }

        public async Task<List<DescriptionWithIsLikeDto>> FindByWordAsync(Guid id, Guid userId)
        {
            var deneme = await _context.Descriptions.Where(d => d.WordId == id && d.Status == ContentStatus.Onaylı)
                .OrderBy(d => d.Order)
                .ToListAsync();

            var _responseList = new List<DescriptionWithIsLikeDto>();

            foreach (var d in deneme) {
             
              var _isLiked = await _context.DescriptionLikes.AnyAsync(x=>x.UserId == userId && x.DescriptionId == d.Id);

                var newObj = new DescriptionWithIsLikeDto
                {
                    Id = d.Id,
                    DescriptionContent = d.DescriptionContent,
                    isLike = _isLiked
                };

                _responseList.Add(newObj);
            }

            return _responseList;   
        }

        public async Task<int> FindGreatestOrder(Guid wordId)
        {
            return await _context.Descriptions.Where(d => d.WordId == wordId)
                .OrderByDescending(d => d.Order)
                .Select(d => d.Order)
                .FirstOrDefaultAsync();
        }

        public async Task<Description> FindParentDescription(Guid id)
        {
            return await _context.Descriptions.SingleOrDefaultAsync(d => d.PreviousDescriptionId == id);
        }
    }
}
