using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KSozluk.WebAPI.Entities;
using KSozluk.WebAPI.SharedKernel;

namespace KSozluk.WebAPI.Business
{
    public interface IWordService
    {
        Task<ServiceResponse<Word>> AddWordAsync(string WordContent, List<string> DescriptionsContent, long? UserId, List<string> Roles);
        Task<ServiceResponse<bool>> DeleteWordAsync(Guid WordId, long? UserId, List<string> Roles);
        Task<ServiceResponse<List<Word>>> GetAllWordsAsync(long? UserId, List<string> Roles);
        Task<ServiceResponse<List<Word>>>GetApprovedWordsPaginatedAsync(int PageNumber, int PageSize, long? UserId, List<string> Roles);
        Task<ServiceResponse<List<Word>>> GetWordsByContainsAsync(string Content, long? UserId, List<string> Roles);
        Task<ServiceResponse<List<Word>>> GetWordsByLetterAsync(char Letter, int PageNumber, int PageSize, long? UserId, List<string> Roles);
        Task<ServiceResponse<Guid>> LikeWordAsync(Guid WordId, long? UserId, List<string> Roles);
        Task<ServiceResponse<Word>> RecommendNewWordAsync(string WordContent, List<string> DescriptionContent, long? UserId, List<string> Roles);
        Task<ServiceResponse<Word>> UpdateWordAsync(Guid WordId, Guid DescriptionId, string WordContent, string DescriptionContent, long? UserId, List<string> Roles);
        Task<ServiceResponse<Word>> UpdateWordByIdAsync(Guid WordId, string WordContent, long? UserId, List<string> Roles);
    }
}