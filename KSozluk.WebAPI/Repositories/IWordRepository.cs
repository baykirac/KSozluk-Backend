using KSozluk.WebAPI;
using KSozluk.WebAPI.DTOs;
using KSozluk.WebAPI.Entities;
using KSozluk.WebAPI.SharedKernel;
using System.Transactions;

namespace KSozluk.WebAPI.Repositories
{
    public interface IWordRepository : IDomainRepository<Word>
    {
        public Task<List<Word>> GetWordsByLetterAsync(char letter, int pageNumber, int pageSize);
        public Task<List<Word>> GetAllWordsAsync();
        public Task<List<Word>> GetWordsByContainsAsync(string Content);
        public Task<Word> FindByContentAsync(string Content);
        public Task<List<Word>> GetAllWordsByPaginate (int pageNumber, int pageSize);
        public Task<List<ResponseTopWordListDto>> GetMostLikedWeekly();
        public Task<Word> FindByIdAsync(Guid? wordId);

    }
}
