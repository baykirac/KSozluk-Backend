using System;
using KSozluk.WebAPI.DTOs;
using KSozluk.WebAPI.Entities;
using Ozcorps.Generic.Bll;
using Ozcorps.Generic.Dal;
using Microsoft.EntityFrameworkCore;


namespace KSozluk.WebAPI.Business
{
    public class WordService : DbServiceBase, IWordService
    {
        private readonly IRepository<Description> _DescriptionRepository;
        private readonly IRepository<Word> _WordRepository;
        private readonly IRepository<WordLike> _WordLikeRepository;
        private readonly IRepository<DescriptionLike> _DescriptionLikeRepository;
        private readonly IRepository<FavoriteWord> _FavoriteWordRepository;

        public WordService(IUnitOfWork _unitOfWork) : base(_unitOfWork)
        {

            _DescriptionRepository = _unitOfWork.GetRepository<Description>();

            _WordRepository = _unitOfWork.GetRepository<Word>();

            _WordLikeRepository = _unitOfWork.GetRepository<WordLike>();

            _DescriptionLikeRepository = _unitOfWork.GetRepository<DescriptionLike>();

            _FavoriteWordRepository = _unitOfWork.GetRepository<FavoriteWord>();
        }

        public async Task<AddWordResultDto> AddWordAsync(string wordContent, List<string> descriptionContent, long? userId, List<string> roles)
        {
            var existedWord = await _WordRepository.GetQueryable()
                .FirstOrDefaultAsync(w => w.WordContent == wordContent);

            if (existedWord != null)
            {
                int greatestOrder = await _DescriptionRepository.GetQueryable()
                    .Where(d => d.WordId == existedWord.Id)
                    .Select(d => d.Order)
                    .DefaultIfEmpty(0)
                    .MaxAsync();

                int order = greatestOrder + 1;

                foreach (var descriptionText in descriptionContent)
                {
                    var existingDescription = await _DescriptionRepository.GetQueryable()
                        .FirstOrDefaultAsync(d => d.DescriptionContent == descriptionText);
                    if (existingDescription == null)
                    {
                        var description = Description.Create(descriptionText, order, userId);
                        existedWord.AddDescription(description);
                        order++;
                    }
                }

                return new AddWordResultDto
                {
                    Id = existedWord.Id,
                    WordContent = existedWord.WordContent,
                    CreatedDate = DateTime.Now,
                    DescriptionCount = existedWord.Descriptions.Count
                };
            }

            var word = Word.Create(wordContent, userId);
            int newOrder = 0;

            _WordRepository.Add(word);

            foreach (var descriptionText in descriptionContent)
            {
                var description = Description.Create(descriptionText, newOrder++, userId);
                word.AddDescription(description);
            }

            _UnitOfWork.Save();

            return new AddWordResultDto
            {
                Id = word.Id,
                WordContent = word.WordContent,
                CreatedDate = DateTime.Now,
                DescriptionCount = word.Descriptions.Count
            };
        }

        public async Task DeleteWordAsync(Guid wordId, long? userId, List<string> roles)
        {
            var word = await _WordRepository.GetQueryable()
                .FirstOrDefaultAsync(w => w.Id == wordId);
            if (word != null)
            {
                _WordRepository.Remove(word);
            }
            _UnitOfWork.Save();
            return;
        }

        public async Task<WordAllDto> GetAllWordsAsync(long? UserId, List<string> Roles)
        {
            var words = await _WordRepository.GetQueryable()
                .Include(w => w.Descriptions)
                .ThenInclude(d => d.PreviousDescription)
                .Include(w => w.User)
                .OrderByDescending(x => x.OperationDate)
                .ToListAsync();

            var wordDtos = words
                .Where(word => word.UserId.HasValue)
                .Select(word => new WordDto
                {
                    Id = word.Id,
                    WordContent = word.WordContent,
                    Status = word.Status,
                    UserId = word.UserId.Value,
                    Users = word.User != null
                        ? new UserDto
                        {
                            Id = word.User.Id,
                            Username = word.User.Username,
                            Name = word.User.Name,
                            Surname = word.User.Surname,
                            Email = word.User.Email
                        }
                        : new UserDto(),
                    LastEditedDate = word.LastEditedDate,
                    Descriptions = word.Descriptions?
                        .OrderByDescending(d => d.LastEditedDate) // Buraya sıralama eklendi
                        .Where(d => d.UserId.HasValue)
                        .Select(d => new DescriptionDto
                        {
                            Id = d.Id,
                            DescriptionContent = d.DescriptionContent,
                            Order = d.Order,
                            WordId = d.WordId,
                            Status = d.Status,
                            User = d.User != null
                                ? new UserDto
                                {
                                    Id = d.User.Id, // word.User yerine d.User
                                    Username = d.User.Username,
                                    Name = d.User.Name,
                                    Surname = d.User.Surname,
                                    Email = d.User.Email
                                }
                                : new UserDto(),
                            UserId = d.UserId ?? 0, // null kontrolü eklendi
                            LastEditedDate = d.LastEditedDate,
                        }).ToList(),
                }).ToList();

            return new WordAllDto
            {
                Items = wordDtos
            };
        }
        public async Task<WordPagedResultDto> GetApprovedWordsPaginatedAsync(int pageNumber, int pageSize, long? userId, List<string> roles)
        {

            var words = await _WordRepository.GetQueryable()
                .Include(w => w.Descriptions)
                .OrderBy(w => w.WordContent)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new WordPagedResultDto
            {
                Items = words.Select(w => new WordListDto
                {
                    Id = w.Id,
                    WordContent = w.WordContent,
                    Status = w.Status
                }).ToList(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<List<WordSearchResultDto>> GetWordsByContainsAsync(string Content, long? UserId, List<string> Roles)
        {
            var words = await _WordRepository.GetQueryable()
                .Where(w =>
                    w.WordContent.ToLower().Contains(Content.ToLower()) &&
                    w.Status == ContentStatus.Onaylı)
                .ToListAsync();

            return words.Select(w => new WordSearchResultDto
            {
                Id = w.Id,
                WordContent = w.WordContent
            }).ToList();
        }

        public async Task<PagedWordDto> GetWordsByLetterAsync(char letter, int pageNumber, int pageSize, long? userId, List<string> roles)
        {
            pageSize = 5;
            var words = await _WordRepository.GetQueryable()
                .Where(w => w.Status == ContentStatus.Onaylı &&
                           w.WordContent.ToLower().StartsWith(letter.ToString().ToLower()))
                .OrderBy(w => w.WordContent)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(w => new WordSearchResultDto
                {
                    Id = w.Id,
                    WordContent = w.WordContent
                })
                .ToListAsync();

            _UnitOfWork.Save();

            return new PagedWordDto
            {
                Letter = letter,
                PageNumber = pageNumber,
                PageSize = pageSize,
                WordResults = words
            };
        }

        public async Task<LikeResultDto> LikeWordAsync(Guid wordId, long? userId, List<string> roles)
        {

            var existingLike = _WordLikeRepository.GetFirst(x => x.WordId == wordId && x.UserId == userId);

            if (existingLike != null)
            {
                _WordLikeRepository.Remove(existingLike);
                _UnitOfWork.Save();

                var totalLikes = _WordLikeRepository.GetQueryable().Count(x => x.WordId == wordId);

                return new LikeResultDto
                {
                    IsLiked = false,
                    TotalLikes = totalLikes
                };
            }
            else
            {
                var now = DateTime.UtcNow;
                var newWordLike = WordLike.Create(Guid.NewGuid(), wordId, userId, now);
                await _WordLikeRepository.AddAsync(newWordLike);
                _UnitOfWork.Save();

                var totalLikes = _WordLikeRepository.GetQueryable().Count(x => x.WordId == wordId);

                return new LikeResultDto
                {
                    IsLiked = true,
                    TotalLikes = totalLikes
                };
            }
        }

        public async Task<RecommendWordResultDto> RecommendNewWordAsync(string WordContent, List<string> DescriptionContent, long? UserId, List<string> Roles)
        {
            var now = DateTime.Now;

            var existingWord = await _WordRepository.GetQueryable()
                .Include(w => w.Descriptions)
                .FirstOrDefaultAsync(w => w.WordContent == WordContent);

            if (existingWord is null)
            {
                var word = Word.Create(WordContent, UserId, now, now);

                foreach (var descriptionText in DescriptionContent)
                {
                    var description = Description.Create(descriptionText, 0, UserId, null);
                    word.AddDescription(description);
                }

                await _WordRepository.AddAsync(word);
                _UnitOfWork.Save();

                return new RecommendWordResultDto
                {
                    Id = word.Id,
                    WordContent = WordContent,
                    SubmitDate = now
                };
            }

            var updated = Word.UpdateOperationDate(existingWord, now);

            foreach (var descriptionText in DescriptionContent)
            {
                var description = Description.Create(descriptionText, 0, UserId, null);
                existingWord.AddDescription(description);
            }

            _UnitOfWork.Save();

            return new RecommendWordResultDto
            {
                Id = existingWord.Id,
                WordContent = existingWord.WordContent,
                SubmitDate = now
            };
        }

        public async Task<UpdateWordResultDto> UpdateWordAsync(Guid WordId, Guid DescriptionId, string WordContent, string DescriptionContent, long? UserId, List<string> Roles)
        {
            var word = await _WordRepository.GetQueryable()
                .Include(w => w.Descriptions)
                .FirstOrDefaultAsync(w => w.Id == WordId);

            word.ChangeContent(WordContent);

            var description = word.Descriptions.SingleOrDefault(d => d.Id == DescriptionId);
            if (description != null)
            {
                description.UpdateContent(DescriptionContent);
                description.UpdateRecommender(UserId);
            }

            _UnitOfWork.Save();

            return new UpdateWordResultDto
            {
                Id = word.Id,
                WordContent = word.WordContent,
                UpdatedDate = DateTime.Now,
                DescriptionId = description?.Id ?? Guid.Empty,
                DescriptionContent = description?.DescriptionContent
            };
        }

        public async Task<UpdateWordByIdResultDto> UpdateWordByIdAsync(Guid wordId, string wordContent, long? userId, List<string> roles)
        {

            var word = await _WordRepository.GetQueryable()
                .FirstOrDefaultAsync(w => w.Id == wordId);


            word.ChangeContent(wordContent);

            _UnitOfWork.Save();

            return new UpdateWordByIdResultDto
            {
                Id = word.Id,
                WordContent = word.WordContent
            };
        }

        public async Task<List<GetLastEditDto>> GetLastEditDateAsync(long? UserId, List<string> Roles)
        {
            return await _WordRepository.GetQueryable()
                .Where(w => w.Status == ContentStatus.Onaylı)
                .OrderByDescending(w => w.LastEditedDate)
                .Take(10)
                .Select(w => new GetLastEditDto
                {
                    WordContent = w.WordContent,
                    LastEditedDate = w.LastEditedDate,
                    Status = w.Status,
                })
                .ToListAsync();
        }

    }
}