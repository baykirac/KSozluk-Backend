using KSozluk.Application.Services.Repositories;
using KSozluk.Domain;
using KSozluk.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

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
            return _context.Words.Include(w => w.Descriptions).SingleOrDefaultAsync(w => w.Id == id);
        }

        public Task<Word> FindByContentAsync(string content)
        {
            return _context.Words.Include(d => d.Descriptions).SingleOrDefaultAsync(w => w.WordContent == content);
        }

        public async Task<List<Word>> GetAllWordsAsync()
        {
            return await _context.Words.Include(w => w.Descriptions.OrderBy(d => d.Order)).Include(w => w.Acceptor).ToListAsync();
        }

        public async Task<List<Word>> GetWordsByLetterAsync(char letter, int pageNumber, int pageSize)
        {
            return await _context.Words
                .Where(w => w.Status == ContentStatus.Onaylı && w.WordContent.ToLower().StartsWith(letter.ToString().ToLower()))
                .OrderBy(w => w.WordContent)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Word>> GetWordsByContainsAsync(string content)
        {
            return await _context.Words.Where(w => w.WordContent.ToLower().Contains(content.ToLower()) && w.Status == ContentStatus.Onaylı).ToListAsync();
        }

        public async Task<List<Word>> GetAllWordsByPaginate(int pageNumber, int pageSize)
        {
            return await _context.Words
                .Include(w => w.Descriptions)
                .OrderBy(w => w.WordContent)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var word = await _context.Words.FirstOrDefaultAsync(w => w.Id == id);

            _context.Words.Remove(word);
        }
    }
}
