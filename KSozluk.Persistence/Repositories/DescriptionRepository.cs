using KSozluk.Domain;
using KSozluk.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace KSozluk.Persistence.Repositories
{
    public sealed class DescriptionRepository
    {
        private readonly SozlukContext _context;

        public DescriptionRepository(SozlukContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Descriptions descriptions)
        {
            await _context.Descriptions.AddAsync(descriptions);
        }

        public Task<Descriptions> FindAsync(Guid id)
        {
            return _context.Descriptions.SingleOrDefaultAsync(d => d.Id == id);
        }
    }
}
