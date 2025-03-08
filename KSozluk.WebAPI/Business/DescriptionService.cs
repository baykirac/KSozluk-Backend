using KSozluk.WebAPI.DTOs;
using KSozluk.WebAPI.Entities;
using KSozluk.WebAPI.Repositories;
using KSozluk.WebAPI.SharedKernel;
using KSozluk.WebAPI.Enums;
using System.ComponentModel;
using System.Reflection;
using KSozluk.WebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Nest;

namespace KSozluk.WebAPI.Business
{
    public class DescriptionService : IDescriptionService
    {
        private readonly IDescriptionRepository _descriptionRepository;
        private readonly IWordRepository _wordRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly ILikeRepository _descriptionLikeRepository;
        private readonly IFavoriteWordRepository _favouriteLikeRepository;
        private readonly IFavoriteWordRepository _favoriteWordRepository;
        private readonly IEmailService _emailService;

        private readonly IUnit _unit;

        public DescriptionService(IDescriptionRepository descriptionRepository, ILikeRepository likeRepository, IUnit unit, IWordRepository wordRepository, IFavoriteWordRepository favouriteLikeRepository, IFavoriteWordRepository favoriteWordRepository, ILikeRepository descriptionLikeRepository, IEmailService emailService)
        {
            _descriptionRepository = descriptionRepository;
            _likeRepository = likeRepository;
            _unit = unit;
            _wordRepository = wordRepository;
            _favouriteLikeRepository = favouriteLikeRepository;
            _favoriteWordRepository = favoriteWordRepository;
            _descriptionLikeRepository = descriptionLikeRepository;
            _emailService = emailService;
        }
        public async Task<ServiceResponse<List<DescriptionWithIsLikeDto>>> GetDescriptionsAsync(Guid WordId, long? UserId, List<string> Roles)
        {

            var descriptions = await _descriptionRepository.FindByWordAsync(WordId, UserId);

            var isFavourited = false;
            var isLikedWord = false;

            var favoriteWord = await _favoriteWordRepository.GetByFavoriteWordAndUserAsync(WordId, UserId);
            var likedWord = await _likeRepository.FindLikedWordAsync(WordId, UserId);

            if (favoriteWord != null)
            {
                isFavourited = true;
            }

            if (likedWord != null)
            {
                isLikedWord = true;
            }

            //var favouriteWord = await _favouriteWordRepository.GetByFavouriteWordAndUserAsync(request.WordId, userId);

            //if (favouriteWord != null)
            //{
            //    isFavourited = true;
            //}

            var _response = new
            {
                Body = descriptions,
                IsFavourited = isFavourited,
                IsLikedWord = isLikedWord
            };

            return new ServiceResponse<List<DescriptionWithIsLikeDto>>(_response.Body);
        }

        public async Task<ServiceResponse<Description>> DeleteDescriptionAsync(Guid DescriptionId, long? userId, List<string> Roles)
        {

            var word = await _descriptionRepository.FindByDescription(DescriptionId);
            var id = word.Id;
            word = await _wordRepository.FindAsync(id);
            var description = await _descriptionRepository.FindAsync(DescriptionId);

            if (word.Descriptions.Count == 1)
            {
                await _wordRepository.DeleteAsync(word.Id);
                await _unit.SaveChangesAsync();
            }
            word.RemoveDescription(description);
            await _unit.SaveChangesAsync();
            return new ServiceResponse<Description>(description);
        }

        public async Task<ServiceResponse<List<DescriptionTimelineDto>>> DescriptionTimelineAsync(long? UserId)
        {
            var response = await _descriptionRepository.GetDescriptionForTimelineAsync(UserId);

            if (!response.Any())
            {
                return new ServiceResponse<List<DescriptionTimelineDto>>(null, false, "Henüz kelime eklenmemiş.");
            }

            return new ServiceResponse<List<DescriptionTimelineDto>>(response);
        }

        public async Task<ServiceResponse<List<DescriptionHeaderNameDto>>> HeaderDescriptionAsync(string WordContent, long? UserId, List<string> Roles)
        {
            if (!Roles.Contains("admin"))
            {
                return new ServiceResponse<List<DescriptionHeaderNameDto>>(null, false, "Yetkisiz işlem. Admin rolü gereklidir.");
            }

            var word = await _wordRepository.FindByContentAsync(WordContent);
            if (word == null)
            {
                return new ServiceResponse<List<DescriptionHeaderNameDto>>(null, false, "Kelime bulunamadı.");
            }

            var descriptions = await _descriptionRepository.FindHeaderByWordAsync(word.Id);

            var resultList = descriptions.Select(d => new DescriptionHeaderNameDto
            {
                Id = d.Id,
                DescriptionContent = d.DescriptionContent
            }).ToList();

            return new ServiceResponse<List<DescriptionHeaderNameDto>>(resultList, true, "Başarıyla getirildi.");
        }


        public async Task<ServiceResponse<Guid>> LikeDescriptionAsync(Guid DescriptionId, long? UserId, List<string> Roles)
        {

            var existingLike = await _descriptionLikeRepository.GetByDescriptionAndUserAsync(DescriptionId, UserId);

            if (existingLike != null)
            {
                _descriptionLikeRepository.DeleteDescriptionLike(existingLike);
                await _unit.SaveChangesAsync();

                return new ServiceResponse<Guid>(DescriptionId);
            }
            else
            {
                var newLike = new DescriptionLike
                {
                    Id = Guid.NewGuid(),
                    DescriptionId = DescriptionId,
                    UserId = UserId,
                    Timestamp = DateTime.UtcNow
                };

                await _descriptionLikeRepository.CreateDescriptionLike(newLike);
                await _unit.SaveChangesAsync();
                 return new ServiceResponse<Guid>(DescriptionId);
            }
        }

        public async Task<ServiceResponse<Description>> RecommendNewDescriptionAsync(Guid WordId, Guid? PreviousDescriptionId, string Content, long? UserId, List<string> Roles)
        {

            var word = await _wordRepository.FindAsync(WordId);

            if (word == null)
            {
                  return new ServiceResponse<Description>(null, false, "Word Yok");
            }

            var greatestOrder = await _descriptionRepository.FindGreatestOrder(word.Id);
            var newOrder = greatestOrder + 1;

            var description = Description.Create(Content, newOrder, UserId, PreviousDescriptionId);

            word.AddDescription(description);

            await _unit.SaveChangesAsync();

            return new ServiceResponse<Description>(description);
        }

        public async Task<ServiceResponse<Description>> UpdateOrderAsync(Guid DescriptionId, int Order, long? UserId, List<string> Roles)
        {

            var description = await _descriptionRepository.FindAsync(DescriptionId);

            if (description == null)
            {
                throw new InvalidOperationException("Description not fou");
            }

            description.UpdateOrder(Order);

            await _unit.SaveChangesAsync();
            return new ServiceResponse<Description>(description);
        }

        public async Task<ServiceResponse<Guid>> FavouriteWordAsync(Guid WordId, long? UserId, List<string> Roles)
        {

            var existingLike = await _favouriteLikeRepository.GetByFavoriteWordAndUserAsync(WordId, UserId);

            if (existingLike != null)
            {
                _favouriteLikeRepository.Delete(existingLike);
                await _unit.SaveChangesAsync();

                return new ServiceResponse<Guid>(WordId);
            }
            else
            {
                var newLike = new KSozluk.WebAPI.Entities.FavoriteWord
                {
                    Id = Guid.NewGuid(),
                    WordId = WordId,
                    UserId = UserId
                };

                await _favouriteLikeRepository.CreateAsync(newLike);
                await _unit.SaveChangesAsync();
                return new ServiceResponse<Guid>(WordId);
            }
        }

        public async Task<ServiceResponse<Description>> UpdateStatusAsync(Guid DescriptionId, ContentStatus Status, int RejectionReasons, string CustomRejectionReason, long? UserId, string Email, List<string> Roles)
        {

            var description = await _descriptionRepository.FindAsync(DescriptionId);
            var word = await _wordRepository.FindAsync(description.WordId);
            var previousDescriptionId = description.PreviousDescriptionId;

            if (previousDescriptionId.HasValue && Status == ContentStatus.Onaylı)
            {
                var previousDescription = await _descriptionRepository.FindAsync(previousDescriptionId);
                previousDescription.UpdateStatus(ContentStatus.Reddedildi);
            }

            var parentDescription = await _descriptionRepository.FindParentDescription(DescriptionId);
            if (parentDescription is not null && Status == ContentStatus.Onaylı)
            {
                parentDescription.UpdateStatus(ContentStatus.Reddedildi);
            }

            if (Status == ContentStatus.Onaylı)
            {
                word.UpdateStatus(ContentStatus.Onaylı);
            }

            else if (Status == ContentStatus.Reddedildi || Status == ContentStatus.Bekliyor)
            {
                var hasOtherApprovedDescriptions = word.Descriptions
                    .Any(d => d.Id != description.Id && d.Status == ContentStatus.Onaylı);

                if (!hasOtherApprovedDescriptions && word.Descriptions.All(d =>
                    d.Status == ContentStatus.Reddedildi || d.Status == ContentStatus.Bekliyor))
                {
                    word.UpdateStatus(ContentStatus.Reddedildi);
                }
            }

            description.UpdateStatus(Status);
            description.UpdateAcceptor(UserId);


            var descriptionDto = await _descriptionRepository.GetById(x => x.Id == description.Id && x.UserId == description.UserId);

            if (descriptionDto == null)
            {
               return new ServiceResponse<Description>(null, false, "descriptionDto not found");
            }

            var _responseDescriptionRecommendDto = new DescriptionReccomendDto()
            {
                DescriptionContent = descriptionDto.DescriptionContent,
                LastEditedDate = descriptionDto.LastEditedDate,
            };

            // Only handle rejection reasons when status is Reddedildi
            if (Status == ContentStatus.Reddedildi)
            {
                int? rejectionReasons;
                string customRejectionReason;

                //when RejectionReasons is 7
                if (RejectionReasons == 7 && !string.IsNullOrEmpty(CustomRejectionReason))
                {
                    rejectionReasons = RejectionReasons;
                    customRejectionReason = CustomRejectionReason;
                    description.UpdateRejectionReasons(rejectionReasons, customRejectionReason);
                }
                else
                {
                    rejectionReasons = RejectionReasons;
                    customRejectionReason = CustomRejectionReason;
                    description.UpdateRejectionReasons(rejectionReasons, customRejectionReason);
                }          
            }

            // Only handle rejection reasons when status is Onaylı
            if (Status == ContentStatus.Onaylı)
            {
                Status = ContentStatus.Onaylı;
            }


            await _unit.SaveChangesAsync();
            return new ServiceResponse<Description>(description);
        }

        public static int? GetEnumDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();
            if (attribute != null && int.TryParse(attribute.Description, out int result))
            {
              return result;
            }
    
           return null;
        }

        public async Task<ServiceResponse<List<ResponseFavouriteWordContentDto>>> FavouriteWordsOnScreenAsync(long? UserId, List<string> Roles)
        {
            // if (!Roles.Contains("admin"))
            // {
            //     return new ServiceResponse<List<ResponseFavouriteWordContentDto>>(null, false, "Yetkisiz işlem. Admin rolü gereklidir.");
            // }

            var favouriteWords = await _favoriteWordRepository.GetFavouriteWordsByUserIdAsync(UserId);

            if (!favouriteWords.Any())
            {
                throw new InvalidOperationException("Description not fou");
            }

            return new ServiceResponse<List<ResponseFavouriteWordContentDto>>(favouriteWords);
        }

        public async Task<ServiceResponse<List<ResponseTopWordListDto>>> WeeklyLikedAsync(long? UserId, List<string> Roles)
        {
            var mostLikedWords = await _wordRepository.GetMostLikedWeekly();
            return new ServiceResponse<List<ResponseTopWordListDto>>(mostLikedWords);
        }
    }
}