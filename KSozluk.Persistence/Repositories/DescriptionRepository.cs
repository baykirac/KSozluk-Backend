using KSozluk.Application.Services.Repositories;
using KSozluk.Domain;
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

        public Task<Description> FindAsync(Guid id)
        {
            return _context.Descriptions.SingleOrDefaultAsync(d => d.Id == id);
        }

        public async Task<List<Description>> FindByWordAsync(Guid id)
        {
            return await _context.Descriptions.Where(d => d.Word.Id == id)
                .OrderBy(d => d.Order)
                .ToListAsync();
        }
    }
}
