using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KSozluk.WebAPI.Entities;
using KSozluk.WebAPI.DTOs;
using KSozluk.WebAPI.SharedKernel;

namespace KSozluk.WebAPI.Business
{
    public interface IWordService
    {
        Task<AddWordResultDto> AddWordAsync(string WordContent, List<string> DescriptionsContent, long? UserId, List<string> Roles);
        Task DeleteWordAsync(Guid wordId, long? userId, List<string> roles);
        Task<List<WordDto>> GetAllWordsAsync(long? UserId, List<string> Roles, int pageNumber, int pageSize);
        Task<WordPagedResultDto> GetApprovedWordsPaginatedAsync(int pageNumber, int pageSize, long? userId, List<string> roles);
        Task<List<WordSearchResultDto>> GetWordsByContainsAsync(string Content, long? UserId, List<string> Roles);
        Task<PagedWordDto> GetWordsByLetterAsync(char letter, int pageNumber, int pageSize, long? userId, List<string> roles);
        Task<LikeResultDto> LikeWordAsync(Guid wordId, long? userId, List<string> roles);
        Task<RecommendWordResultDto> RecommendNewWordAsync(string WordContent, List<string> DescriptionContent, long? UserId, List<string> Roles);
        Task<UpdateWordResultDto> UpdateWordAsync(Guid WordId, Guid DescriptionId, string WordContent, string DescriptionContent, long? UserId, List<string> Roles);
        Task<UpdateWordByIdResultDto> UpdateWordByIdAsync(Guid wordId, string wordContent, long? userId, List<string> roles);
        Task<List<GetLastEditDto>> GetLastEditDateAsync(long? UserId, List<string> Roles);
    }
}