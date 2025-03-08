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
        Task<ServiceResponse<List<DescriptionWithIsLikeDto>>> GetDescriptionsAsync(Guid WordId, long? UserId, List<string> Roles); // Açıklamaları al
        Task<ServiceResponse<Description>> DeleteDescriptionAsync(Guid DescriptionId, long? UserId, List<string> Roles); // Açıklamayı sil
        Task<ServiceResponse<List<DescriptionTimelineDto>>> DescriptionTimelineAsync(long? UserId); // Zaman çizelgesini al
        Task<ServiceResponse<List<DescriptionHeaderNameDto>>> HeaderDescriptionAsync(string WordContent, long? UserId, List<string> Roles); // Başlık açıklamalarını al
        Task<ServiceResponse<Guid>> LikeDescriptionAsync(Guid DescriptionId, long? UserId, List<string> Roles); // Açıklamayı beğen
        Task<ServiceResponse<Description>> RecommendNewDescriptionAsync(Guid WordId, Guid? PreviousDescriptionId, string Content, long? UserId, List<string> roles); // Yeni açıklama öner
        Task<ServiceResponse<Description>> UpdateOrderAsync(Guid DescriptionId, int Order, long? UserId, List<string> Roles); // Sıralamayı güncelle
        Task<ServiceResponse<Guid>> FavouriteWordAsync(Guid WordId, long? UserId, List<string> Roles); // Favori kelime ekle
        Task<ServiceResponse<List<ResponseFavouriteWordContentDto>>> FavouriteWordsOnScreenAsync(long? UserId, List<string> Roles); // Favori kelime sil
        Task<ServiceResponse<Description>> UpdateStatusAsync(Guid DescriptionId, ContentStatus Status, int RejectionReasons, string CustomRejectionReason, long? UserId, string Email, List<string> Roles); // Durumu güncelle    
        Task<ServiceResponse<List<ResponseTopWordListDto>>> WeeklyLikedAsync(long? UserId, List<string> Roles); // Haftalık beğenilenleri al
    }
}