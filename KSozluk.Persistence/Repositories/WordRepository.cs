using KSozluk.Application.Services.Repositories;
using KSozluk.Domain;
using KSozluk.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace KSozluk.Persistence.Repositories
{
    public sealed class WordRepository : IWordRepository
    {
        private readonly SozlukContext _context;

        public WordRepository(SozlukContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Words word)
        {
            await _context.Words.AddAsync(word);
        }

        public Task<Words> FindAsync(Guid id)
        {
            return _context.Words.SingleOrDefaultAsync(w => w.Id == id);
        }
    }
}
