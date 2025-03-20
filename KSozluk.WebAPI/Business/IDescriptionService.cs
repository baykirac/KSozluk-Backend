using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KSozluk.WebAPI.DTOs;
using KSozluk.WebAPI.Entities;
using KSozluk.WebAPI.SharedKernel;

namespace KSozluk.WebAPI.Business
{
    public interface IDescriptionService
    {
        Task<DescriptionWithDetailsDto> GetDescriptionsAsync(Guid WordId, long? UserId, List<string> Roles);
        Task<Description> DeleteDescriptionAsync(Guid DescriptionId, long? UserId, List<string> Roles);
        Task<List<DescriptionTimelineDto>> DescriptionTimelineAsync(long? UserId);
        Task<List<DescriptionHeaderNameDto>> HeaderDescriptionAsync(string WordContent, long? UserId, List<string> Roles);
        Task<LikeDescriptionDto> LikeDescriptionAsync(Guid DescriptionId, long? UserId, List<string> Roles);
        Task<ReccomendDescriptionDto> RecommendNewDescriptionAsync(Guid WordId, Guid? PreviousDescriptionId, string Content, long? UserId, List<string> roles);
        Task<UpdateOrderDto> UpdateOrderAsync(Guid DescriptionId, int Order, long? UserId, List<string> Roles);
        Task<FavouriteWordDto> FavouriteWordAsync(Guid WordId, long? UserId, List<string> Roles);
        Task<List<FavouriteWordContentDto>> FavouriteWordsOnScreenAsync(long? UserId, List<string> Roles);
        Task<UpdateStatusDto> UpdateStatusAsync(Guid DescriptionId, ContentStatus Status, int RejectionReasons, string CustomRejectionReason, bool IsActive, long? UserId, string Email, List<string> Roles);
        Task<List<TopWordListDto>> WeeklyLikedAsync(long? UserId, List<string> Roles);
        Task<UpdateIsActiveDto> UpdateIsActiveAsync(Guid DescriptionId, bool IsActive, long? UserId, List<string> Roles);
    }
}