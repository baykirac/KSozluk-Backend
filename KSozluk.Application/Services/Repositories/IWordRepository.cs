﻿using KSozluk.Domain;
using KSozluk.Domain.DTOs;
using KSozluk.Domain.SharedKernel;
using System.Transactions;

namespace KSozluk.Application.Services.Repositories
{
    public interface IWordRepository : IDomainRepository<Word>
    {
        public Task<List<Word>> GetWordsByLetterAsync(char letter, int pageNumber, int pageSize);
        public Task<List<Word>> GetAllWordsAsync();
        public Task<List<Word>> GetWordsByContainsAsync(string content);
        public Task<Word> FindByContentAsync(string content);
        public Task<List<Word>> GetAllWordsByPaginate (int pageNumber, int pageSize);
        public Task<List<ResponseTopWordListDto>> GetMostLikedWeekly();
        public Task<Word> FindByIdAsync(Guid? wordId);

    }
}
