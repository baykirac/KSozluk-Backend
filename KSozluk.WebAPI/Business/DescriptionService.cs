using KSozluk.WebAPI.DTOs;
using KSozluk.WebAPI.Entities;
using System.ComponentModel;
using System.Reflection;
using Ozcorps.Generic.Dal;
using Ozcorps.Generic.Bll;
using Microsoft.EntityFrameworkCore;



namespace KSozluk.WebAPI.Business
{
    public class DescriptionService : DbServiceBase, IDescriptionService
    {
        private readonly IRepository<Description> _DescriptionRepository;
        private readonly IRepository<Word> _WordRepository;
        private readonly IRepository<WordLike> _WordLikeRepository;
        private readonly IRepository<DescriptionLike> _DescriptionLikeRepository;
        private readonly IRepository<FavoriteWord> _FavoriteWordRepository;

        public DescriptionService(IUnitOfWork _unitOfWork) : base(_unitOfWork)
        {

            _DescriptionRepository = _unitOfWork.GetRepository<Description>();

            _WordRepository = _unitOfWork.GetRepository<Word>();

            _WordLikeRepository = _unitOfWork.GetRepository<WordLike>();

            _DescriptionLikeRepository = _unitOfWork.GetRepository<DescriptionLike>();

            _FavoriteWordRepository = _unitOfWork.GetRepository<FavoriteWord>();
        }

        public async Task<DescriptionWithDetailsDto> GetDescriptionsAsync(Guid wordId, long? userId, List<string> roles)
        {
            var descriptions = await _DescriptionRepository.GetQueryable()
                .Where(d => d.WordId == wordId && d.Status == ContentStatus.Onaylı)
                .OrderBy(d => d.Order)
                .Select(d => new DescriptionWithIsLikeDto
                {
                    Id = d.Id,
                    DescriptionContent = d.DescriptionContent
                })
                .ToListAsync();

            if (userId.HasValue)
            {
                var likedDescriptions = await _DescriptionLikeRepository
                    .GetQueryable()
                    .Where(x => x.UserId == userId.Value && descriptions.Select(d => d.Id).Contains(x.DescriptionId))
                    .Select(x => x.DescriptionId)
                    .ToListAsync();

                foreach (var desc in descriptions)
                {
                    desc.isLike = likedDescriptions.Contains(desc.Id);
                }
            }

            var isFavourited = userId.HasValue && await _FavoriteWordRepository
                   .GetQueryable()
                   .AnyAsync(f => f.WordId == wordId && f.UserId == userId.Value);

            var isLikedWord = userId.HasValue && await _WordLikeRepository
                .GetQueryable()
                .AnyAsync(w => w.WordId == wordId && w.UserId == userId.Value);

            return new DescriptionWithDetailsDto
            {
                Body = descriptions,
                IsFavourited = isFavourited,
                IsLikedWord = isLikedWord
            };
        }
        public async Task<Description> DeleteDescriptionAsync(Guid descriptionId, long? userId, List<string> roles)
        {
            var description = await _DescriptionRepository.GetQueryable()
                .Include(d => d.Word)
                    .ThenInclude(w => w.Descriptions)
                .FirstOrDefaultAsync(d => d.Id == descriptionId);

            if (description == null)
                return null;

            var word = description.Word;

            if (word.Descriptions.Count == 1)
            {
                _WordRepository.Remove(word);
            }
            else
            {
                _DescriptionRepository.Remove(description);
            }

            _UnitOfWork.Save();

            return description;
        }

        public async Task<List<DescriptionTimelineDto>> DescriptionTimelineAsync(long? userId)
        {
            var descriptions = await _DescriptionRepository.GetQueryable()
                .Where(d => d.UserId == userId)
                .OrderByDescending(d => d.LastEditedDate)
                .Select(d => new DescriptionTimelineDto
                {
                    Id = d.Id,
                    Status = d.Status,
                    WordId = d.WordId,
                    DescriptionContent = d.DescriptionContent,
                    RejectionReasons = d.RejectionReasons,
                    CustomRejectionReason = d.CustomRejectionReason,
                    IsActive = d.IsActive
                })
                .ToListAsync();

            if (!descriptions.Any())
            {
                return null;
            }

            foreach (var item in descriptions)
            {
                var word = _WordRepository.GetFirst(w => w.Id == item.WordId);
                item.WordContent = word?.WordContent;
            }

            return descriptions;
        }

        public async Task<List<DescriptionHeaderNameDto>> HeaderDescriptionAsync(string wordContent, long? userId, List<string> roles)
        {
            // Önce word'ü bul
            var word = _WordRepository.GetFirst(w => w.WordContent == wordContent);

            if (word == null)
            {
                return null;
            }

            // Description'ları getir
            var descriptions = await _DescriptionRepository.GetQueryable()
                .Where(d => d.WordId == word.Id && d.Status == ContentStatus.Onaylı)
                .OrderBy(d => d.Order)
                .Select(d => new DescriptionHeaderNameDto
                {
                    Id = d.Id,
                    DescriptionContent = d.DescriptionContent
                })
                .ToListAsync();

            return descriptions;
        }

        public async Task<LikeDescriptionDto> LikeDescriptionAsync(Guid descriptionId, long? userId, List<string> roles)
        {
            var existingLike = _DescriptionLikeRepository.GetFirst(x =>
                x.DescriptionId == descriptionId &&
                x.UserId == userId);

            if (existingLike != null)
            {
                _DescriptionLikeRepository.Remove(existingLike);
            }
            else
            {
                await _DescriptionLikeRepository.AddAsync(new DescriptionLike
                {
                    Id = Guid.NewGuid(),
                    DescriptionId = descriptionId,
                    UserId = userId,
                    Timestamp = DateTime.UtcNow
                });
            }

            _UnitOfWork.Save();

            return new LikeDescriptionDto
            {
                Id = descriptionId,
                UserId = userId
            };
        }

        public async Task<ReccomendDescriptionDto> RecommendNewDescriptionAsync(Guid WordId, Guid? PreviousDescriptionId, string Content, long? UserId, List<string> Roles)
        {
            var word = await _WordRepository.GetQueryable()
                .Include(w => w.Descriptions)
                .FirstOrDefaultAsync(w => w.Id == WordId);


            var greatestOrder = await _DescriptionRepository.GetQueryable()
                .Where(d => d.WordId == word.Id)
                .OrderByDescending(d => d.Order)
                .Select(d => d.Order)
                .FirstOrDefaultAsync();

            var newOrder = greatestOrder + 1;

            var description = Description.Create(Content, newOrder, UserId, PreviousDescriptionId);

            word.AddDescription(description);

            _UnitOfWork.Save();

            return new ReccomendDescriptionDto
            {
                Id = description.Id,
                DescriptionContent = description.DescriptionContent,
                LastEditedDate = description.LastEditedDate
            };
        }

        public async Task<UpdateOrderDto> UpdateOrderAsync(Guid descriptionId, int order, long? userId, List<string> roles)
        {
            var description = await _DescriptionRepository.GetQueryable()
                .FirstOrDefaultAsync(d => d.Id == descriptionId);

            if (description == null)
            {
                return null;
            }

            description.UpdateOrder(order);
            _UnitOfWork.Save();

            return new UpdateOrderDto
            {
                DescriptionId = description.Id,
                Order = description.Order
            };
        }

        public async Task<UpdateIsActiveDto> UpdateIsActiveAsync(Guid descriptionId, bool isActive, long? userId, List<string> roles)
        {
            var description = await _DescriptionRepository.GetQueryable()
                .FirstOrDefaultAsync(d => d.Id == descriptionId);

            if (description == null)
            {
                return null;
            }

            description.UpdateIsActive(isActive);
            _UnitOfWork.Save();

            return new UpdateIsActiveDto
            {
                DescriptionId = description.Id,
                IsActive = description.IsActive
            };
        }

        public async Task<FavouriteWordDto> FavouriteWordAsync(Guid wordId, long? userId, List<string> roles)
        {
            // Mevcut favori kontrolü
            var existingFavorite = _FavoriteWordRepository.GetFirst(x =>
                x.WordId == wordId && x.UserId == userId);

            if (existingFavorite != null)
            {
                _FavoriteWordRepository.Remove(existingFavorite);
                _UnitOfWork.Save();

                return new FavouriteWordDto
                {
                    WordId = Guid.Empty,
                    UserId = userId
                };
            }
            else
            {
                // Yeni favori ekle
                var newFavorite = new FavoriteWord
                {
                    Id = Guid.NewGuid(),
                    WordId = wordId,
                    UserId = userId
                };

                await _FavoriteWordRepository.AddAsync(newFavorite);
                _UnitOfWork.Save();

                return new FavouriteWordDto
                {
                    WordId = wordId,
                    UserId = userId
                };
            }
        }

        public async Task<UpdateStatusDto> UpdateStatusAsync(Guid DescriptionId, ContentStatus Status, int RejectionReasons, string CustomRejectionReason, bool IsActive, long? UserId, string Email, List<string> Roles)
        {

            var description = await _DescriptionRepository.GetQueryable()
            .FirstOrDefaultAsync(d => d.Id == DescriptionId);

            if (description == null) return null;


            var word = await _WordRepository.GetQueryable()
                .Include(w => w.Descriptions)
                .FirstOrDefaultAsync(w => w.Id == description.WordId);

            if (word == null) return null;

            if (description.PreviousDescriptionId.HasValue && Status == ContentStatus.Onaylı)
            {
                var previousDescription = _DescriptionRepository.GetFirst(
                    d => d.Id == description.PreviousDescriptionId);
                if (previousDescription != null)
                {
                    previousDescription.UpdateStatus(ContentStatus.Reddedildi);
                }
            }

            var parentDescription = _DescriptionRepository.GetFirst(
               d => d.PreviousDescriptionId == DescriptionId);

            if (parentDescription != null && Status == ContentStatus.Onaylı)
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

                if (!hasOtherApprovedDescriptions && word.Descriptions.All(d => d.Status == ContentStatus.Reddedildi || d.Status == ContentStatus.Bekliyor))
                {

                    word.UpdateStatus(ContentStatus.Reddedildi);

                }
            }

            description.UpdateStatus(Status);

            description.UpdateAcceptor(UserId);

            var acceptorIdResponse = new AcceptorIdDto
            {
                AcceptorId = description.AcceptorId
            };

            var updatedDescription = _DescriptionRepository.GetFirst(
                x => x.Id == description.Id && x.AcceptorId == description.AcceptorId);

            if (updatedDescription == null)
            {
                return null;
            }

            var descriptionRecommendResponse = new DescriptionReccomendDto
            {
                DescriptionContent = updatedDescription.DescriptionContent,
                LastEditedDate = updatedDescription.LastEditedDate
            };

            if (Status == ContentStatus.Reddedildi)
            {
                IsActive = true;

                // RejectionReasons 7 özel durumu
                if (RejectionReasons == 7 && !string.IsNullOrEmpty(CustomRejectionReason))
                {
                    description.UpdateRejectionReasons(RejectionReasons, CustomRejectionReason);
                }
                else
                {
                    description.UpdateRejectionReasons(RejectionReasons, CustomRejectionReason);
                }
            }

            if (Status == ContentStatus.Onaylı)
            {
                IsActive = false;
            }

            _UnitOfWork.Save();

            var responseDto = new UpdateStatusDto
            {
                DescriptionId = description.Id,
                Status = description.Status,
                RejectionReasons = description.RejectionReasons ?? 0,
                CustomRejectionReason = description.CustomRejectionReason
            };

            return responseDto;
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

        public async Task<List<FavouriteWordContentDto>> FavouriteWordsOnScreenAsync(long? userId, List<string> roles)
        {
            var favoriteWords = await _FavoriteWordRepository.GetQueryable()
                .Where(x => x.UserId == userId)
                .Select(y => y.WordId)
                .ToListAsync();

            if (!favoriteWords.Any())
            {
                return null;
            }

            var words = await _WordRepository.GetQueryable()
                .Where(x => favoriteWords.Contains(x.Id))
                .OrderBy(x => x.WordContent)
                .Select(x => new FavouriteWordContentDto
                {
                    Id = x.Id,
                    WordContent = x.WordContent
                })
                .ToListAsync();

            return words;
        }

        public async Task<List<TopWordListDto>> WeeklyLikedAsync(long? UserId, List<string> Roles)
        {
            var oneWeekAgo = DateTime.UtcNow.AddDays(-7);

            var words = _WordRepository.GetAll().ToList();

            var likedWords = await _WordLikeRepository.GetQueryable()
                .Where(wl => wl.Timestamp >= oneWeekAgo)
                .GroupBy(wl => wl.WordId)
                .Select(group => new
                {
                    WordId = group.Key,
                    Count = group.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToListAsync();

            return likedWords.Select(x => new TopWordListDto
            {
                WordId = x.WordId,
                Count = x.Count,
                Word = words.FirstOrDefault(y => y.Id == x.WordId)?.WordContent
            }).ToList();
        }

    }
}