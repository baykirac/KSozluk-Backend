using KSozluk.WebAPI.DTOs;
using KSozluk.WebAPI.Entities;
using KSozluk.WebAPI.DataAccess.Contexts;
using KSozluk.WebAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Linq.Expressions;

namespace KSozluk.WebAPI.Repositories
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

        public Task<Word> FindAsync(Guid? id)
        {
            return _context.Words.Include(w => w.Descriptions).SingleOrDefaultAsync(w => w.Id == id);
        }

        public Task<Word> FindByContentAsync(string Content)
        {
            return _context.Words.Include(d => d.Descriptions).SingleOrDefaultAsync(w => w.WordContent == Content);
        }

        public async Task<Word> FindByIdAsync(Guid? wordId)
        {
            return await _context.Words.SingleOrDefaultAsync(w => w.Id == wordId);
        }

        public async Task<List<Word>> GetAllWordsAsync()
        {
            // return await _context.Words
            // .Include(w => w.Descriptions)
            // .OrderByDescending(w => w.OperationDate)         
            // .ToListAsync();
            return await _context.Words
            .Include(w => w.Descriptions
            .OrderByDescending(d => d.LastEditedDate))
            .ThenInclude(d => d.PreviousDescription)
            .Include(w => w.User)
            .OrderByDescending(x => x.OperationDate)
            .ToListAsync();
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

        public async Task<List<Word>> GetWordsByContainsAsync(string Content)
        {
            return await _context.Words.Where(w => w.WordContent.ToLower().Contains(Content.ToLower()) && w.Status == ContentStatus.Onaylı).ToListAsync();
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

        public async Task<List<ResponseTopWordListDto>> GetMostLikedWeekly()
        {
            var oneWeekAgo = DateTime.UtcNow.AddDays(-7);

            var words = _context.Words.ToList();

            var data = await _context.WordLikes
                .Where(wl => wl.Timestamp >= oneWeekAgo)
                .GroupBy(wl => new
                {
                    wl.WordId
                })
                .Select(group => new ResponseTopWordListDto
                {
                    WordId = group.Key.WordId,
                   
                    Count = group.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToListAsync();

            var response = data.Select(x => new ResponseTopWordListDto
            {
                Count = x.Count,
                WordId = x.WordId,
                Word = words.FirstOrDefault(y => y.Id == x.WordId).WordContent,
            }).ToList();

            return response;
        }

        public Task<List<Word>> GetAll(Expression<Func<Word, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Word> GetById(Expression<Func<Word, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
