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

        public async Task CreateAsync(Word word)
        {
            await _context.Words.AddAsync(word);
        }

        public Task<Word> FindAsync(Guid id)
        {
            return _context.Words.SingleOrDefaultAsync(w => w.Id == id);
        }

        public async Task<List<Word>> GetWordsByLetterAsync (char letter, int pageNumber, int pageSize)
        {
            return await _context.Words
                .Where(w => w.Status == ContentStatus.Onaylı && w.WordContent.ToLower().StartsWith(letter.ToString().ToLower()))
                .OrderBy(w => w.WordContent)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        }
    }
}
