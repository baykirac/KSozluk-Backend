using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KSozluk.WebAPI.Configurations;
using KSozluk.WebAPI.DTOs;
using KSozluk.WebAPI.Entities;
using KSozluk.WebAPI.Repositories;
using KSozluk.WebAPI.SharedKernel;

namespace KSozluk.WebAPI.Business
{
    public class WordService : IWordService
    {
        private readonly IWordRepository _wordRepository;
        private readonly IDescriptionRepository _descriptionRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly ILikeRepository _wordLikeRepository;
        private readonly IFavoriteWordRepository _favoriteWordRepository;
        private readonly IUnit _unit;

        public WordService(

            IWordRepository wordRepository,

            IDescriptionRepository descriptionRepository,

            ILikeRepository likeRepository,

            IFavoriteWordRepository favoriteWordRepository,

            IUnit unit,

            ILikeRepository wordLikeRepository

        )
        {
            _wordRepository = wordRepository;

            _descriptionRepository = descriptionRepository;

            _likeRepository = likeRepository;

            _favoriteWordRepository = favoriteWordRepository;

            _unit = unit;

            _wordLikeRepository = wordLikeRepository;
        }

        public async Task<ServiceResponse<Word>> AddWordAsync(string WordContent, List<string> DescriptionContent, long? UserId, List<string> Roles)
        {

            var existedWord = await _wordRepository.FindByContentAsync(WordContent);


            if (existedWord != null) // kelime mevcutsa mevcut kelimeye sadece anlamı eklenecek
            {

                int greatestOrder = await _descriptionRepository.FindGreatestOrder(existedWord.Id);

                int order = greatestOrder + 1;

                foreach (var descriptionText in DescriptionContent)
                {
                    var existingDescription = await _descriptionRepository.FindByContentAsync(descriptionText);

                    if (existingDescription == null)
                    {
                        var description = Description.Create(descriptionText, order, UserId);

                        existedWord.AddDescription(description);

                        order++;
                    }

                    else
                    {
                        return new ServiceResponse<Word>(null, false, "Bu açıklama daha önce eklenmiş.");
                    }
                }

                await _unit.SaveChangesAsync();

                return new ServiceResponse<Word>(existedWord, true, "Kelimeye yeni anlamlar eklendi.");
            }

            var word = Word.Create(WordContent, UserId);

            int newOrder = 0;

            await _wordRepository.CreateAsync(word);

            foreach (var descriptionText in DescriptionContent)
            {
                var description = Description.Create(descriptionText, newOrder++, UserId);

                word.AddDescription(description);
            }

            await _unit.SaveChangesAsync();

            Word.ClearResponse(word);

            return new ServiceResponse<Word>(word, false, "Kelime ve anlam başarıyla eklendi.");
        }

        public async Task<ServiceResponse<bool>> DeleteWordAsync(Guid WordId, long? UserId, List<string> Roles)
        {

            await _likeRepository.DeleteWordLikesByWordIdAsync(WordId);

            await _wordRepository.DeleteAsync(WordId);

            await _unit.SaveChangesAsync();

            return new ServiceResponse<bool>(true, true, "Kelime başarıyla silindi.");
        }

        public async Task<ServiceResponse<List<Word>>> GetAllWordsAsync(long? UserId, List<string> Roles)
        {

            var words = await _wordRepository.GetAllWordsAsync();

            if (!words.Any())
            {

                return new ServiceResponse<List<Word>>(null, false, "Henüz kelime eklenmemiş.");

            }

            return new ServiceResponse<List<Word>>(words, true, "Kelime listesi başarıyla getirildi.");
        }

        public async Task<ServiceResponse<List<Word>>> GetApprovedWordsPaginatedAsync(int PageNumber, int PageSize, long? UserId, List<string> Roles)
        {

            var words = await _wordRepository.GetAllWordsByPaginate(PageNumber, PageSize);

            if (!words.Any())
            {

                return new ServiceResponse<List<Word>>(null, false, "Hiç kelime bulunamadı.");

            }

            return new ServiceResponse<List<Word>>(words, true, "Kelime listesi başarıyla getirildi.");
        }

        public async Task<ServiceResponse<List<Word>>> GetWordsByContainsAsync(string Content, long? UserId, List<string> Roles)
        {

            var words = await _wordRepository.GetWordsByContainsAsync(Content);

            return new ServiceResponse<List<Word>>(words, true, "Kelime listesi başarıyla getirildi.");
        }

        public async Task<ServiceResponse<List<Word>>> GetWordsByLetterAsync(char Letter, int PageNumber, int PageSize, long? UserId, List<string> Roles)
        {

            var words = await _wordRepository.GetWordsByLetterAsync(Letter, PageNumber, PageSize);

            if (!words.Any())
            {

                return new ServiceResponse<List<Word>>(null, false, "Hiç kelime bulunamadı.");

            }

            await _unit.SaveChangesAsync();

            return new ServiceResponse<List<Word>>(words, true, "Kelime listesi başarıyla getirildi.");
        }

        public async Task<ServiceResponse<Guid>> LikeWordAsync(Guid WordId, long? UserId, List<string> Roles)
        {

            var existingLike = await _wordLikeRepository.GetByWordAndUserAsync(WordId, UserId);

            if (existingLike != null)
            {

                _wordLikeRepository.DeleteWordLike(existingLike);

                await _unit.SaveChangesAsync();

                return new ServiceResponse<Guid>(WordId, true, "Kelime beğenisi kaldırıldı.");

            }

            var now = DateTime.UtcNow;

            var newWordLike = WordLike.Create(Guid.NewGuid(), WordId, UserId, now);

            await _wordLikeRepository.CreateWordLike(newWordLike);

            await _unit.SaveChangesAsync();

            return new ServiceResponse<Guid>(WordId, true, "Kelime beğenildi.");

        }

        public async Task<ServiceResponse<Word>> RecommendNewWordAsync(string WordContent, List<string> DescriptionContent, long? UserId, List<string> Roles)
        {

            var now = DateTime.Now;

            var existingWord = await _wordRepository.FindByContentAsync(WordContent);

            if (existingWord is null)
            {
                var word = Word.Create(WordContent, UserId, now, now);

                foreach (var descriptionText in DescriptionContent)
                {

                    var description = Description.Create(descriptionText, 0, UserId, null);

                    word.AddDescription(description);

                }

                await _wordRepository.CreateAsync(word);

                await _unit.SaveChangesAsync();

                return new ServiceResponse<Word>(word, true, "Kelime başarıyla eklendi.");
            }

            var updated = Word.UpdateOperationDate(existingWord, now);

            foreach (var descriptionText in DescriptionContent)
            {

                var description = Description.Create(descriptionText, 0, UserId, null);

                existingWord.AddDescription(description);

            }

            await _unit.SaveChangesAsync();

            return new ServiceResponse<Word>(existingWord, true, "Kelime başarıyla eklendi.");
        }

        public async Task<ServiceResponse<Word>> UpdateWordAsync(Guid WordId, Guid DescriptionId, string WordContent, string DescriptionContent, long? UserId, List<string> Roles)
        {


            var word = await _wordRepository.FindAsync(WordId);

            if (word == null)
            {
                return new ServiceResponse<Word>(null, false, "Hiç kelime bulunamadı.");
            }

            word.ChangeContent(WordContent);

            word.Descriptions.SingleOrDefault(d => d.Id == DescriptionId).UpdateContent(DescriptionContent);

            word.Descriptions.SingleOrDefault(d => d.Id == DescriptionId).UpdateRecommender(UserId);

            await _unit.SaveChangesAsync();

            return new ServiceResponse<Word>(word, true, "Kelime ve açıklama başarıyla güncellendi.");

        }

        public async Task<ServiceResponse<Word>> UpdateWordByIdAsync(Guid WordId, string WordContent, long? UserId, List<string> Roles)
        {

            var word = await _wordRepository.FindByIdAsync(WordId);

            if (word == null)
            {
                return new ServiceResponse<Word>(null, false, "Hiç kelime bulunamadı.");
            }

            word.ChangeContent(WordContent);

            await _unit.SaveChangesAsync();

            return new ServiceResponse<Word>(word, true, "Kelime başarıyla güncellendi.");
        }

        public async Task<ServiceResponse<List<ResponseGetLastEditDto>>> GetLastEditDateAsync(long? UserId, List<string> Roles)
        {
            var wordLast = await _wordRepository.GetOperationDateAsync();

            if (wordLast == null || !wordLast.Any())
            {
                return new ServiceResponse<List<ResponseGetLastEditDto>>(null, false, "Hiç kelime bulunamadı.");
            }

            return new ServiceResponse<List<ResponseGetLastEditDto>>(wordLast, true, "Kelime listesi başarıyla getirildi.");
        }

    }
}