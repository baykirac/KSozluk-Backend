using KSozluk.Domain;
using KSozluk.Domain.SharedKernel;

namespace KSozluk.Application.Services.Repositories
{
    public interface IWordRepository : IDomainRepository<Word>
    {
        public Task<List<Word>> GetWordsByLetterAsync(char letter);
    }
}
