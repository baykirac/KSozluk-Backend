using Ozcorps.Ozt;
using Ozcorps.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KSozluk.WebAPI.Repositories;
using KSozluk.WebAPI.Business;
using KSozluk.WebAPI.DTOs;
using KSozluk.WebAPI.SharedKernel;
using Azure.Core;
using KSozluk.WebAPI.Entities;
using Microsoft.AspNetCore.RateLimiting;

namespace KSozluk.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [OzLoggerActionFilter]
    [EnableRateLimiting("interact-limit")]
    public class WordController : ControllerBase
    {
        private readonly OztTool _OztTool;
        private readonly IOzLogger _Logger;
        private readonly IWordRepository _wordRepository;
        private readonly IWordService _wordService;
        private readonly IDescriptionService _descriptionService;


        public WordController(IOzLogger logger, OztTool oztTool, IWordRepository wordRepository, IWordService wordService, IDescriptionService descriptionService)
        {
            _Logger = logger;
            _OztTool = oztTool;
            _wordRepository = wordRepository;
            _wordService = wordService;
            _descriptionService = descriptionService;
        }

        [HttpGet("[action]")]
        [OztActionFilter]
        public async Task<ServiceResponse<List<ResponseTopWordListDto>>> WeeklyLiked()
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                var _response = await _descriptionService.WeeklyLikedAsync(_userId, _roles);

                return _response;

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

               _username: HttpContext.GetOztUser()?.Username,

               _userId: (long)(HttpContext.GetOztUser()?.UserId));

                return new ServiceResponse<List<ResponseTopWordListDto>>(null, false, _ex.Message);
            }
        }

        [HttpGet("[action]")]
        [OztActionFilter(Permissions = "admin")]
        public async Task<ServiceResponse<List<Word>>> GetAllWords()
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                var _response = await _wordService.GetAllWordsAsync(_userId, _roles);

                return _response;

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

                _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

                _username: HttpContext.GetOztUser()?.Username,

                _userId: (long)(HttpContext.GetOztUser()?.UserId));

                return new ServiceResponse<List<Word>>(null, false, _ex.Message);

            }
        }

        [HttpGet("[action]")]
        public async Task<ServiceResponse<List<ResponseGetLastEditDto>>> GetLastEdit()
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                var _response = await _wordService.GetLastEditDateAsync(_userId, _roles);

                return _response;

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

                _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

                _username: HttpContext.GetOztUser()?.Username,

                _userId: (long)(HttpContext.GetOztUser()?.UserId));

                return new ServiceResponse<List<ResponseGetLastEditDto>>(null, false, _ex.Message);

            }
        }

        [HttpGet("[action]")]
        [OztActionFilter]
        public async Task<ServiceResponse<List<Word>>> GetWordsByLetter(char Letter, int PageNumber, int PageSize)
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                var _response = await _wordService.GetWordsByLetterAsync(Letter, PageNumber, PageSize, _userId, _roles);

                return _response;

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

               _username: HttpContext.GetOztUser()?.Username,

               _userId: (long)(HttpContext.GetOztUser()?.UserId));

                return new ServiceResponse<List<Word>>(null, false, _ex.Message);

            }
        }

        [HttpGet("[action]")]
        [OztActionFilter]
        public async Task<ServiceResponse<List<Word>>> GetWordsByContains(string Content)
        {
            try
            {
                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                var _response = await _wordService.GetWordsByContainsAsync(Content, _userId, _roles);

                return _response;

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

               _username: HttpContext.GetOztUser()?.Username,

               _userId: (long)(HttpContext.GetOztUser()?.UserId));

                return new ServiceResponse<List<Word>>(null, false, _ex.Message);

            }
        }

        [HttpPost("[action]")]
        [OztActionFilter(Permissions = "admin")]
        public async Task<ServiceResponse<Word>> AddWord(RequestAddWords _dto)
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                var _response = await _wordService.AddWordAsync(_dto.WordContent, _dto.Description, _userId, _roles);

                return _response;

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

               _username: HttpContext.GetOztUser()?.Username,

               _userId: (long)(HttpContext.GetOztUser()?.UserId));

                return new ServiceResponse<Word>(null, false, _ex.Message);

            }
        }

        [HttpGet("[action]")]
        [OztActionFilter(Permissions = "admin")]
        public async Task<ServiceResponse<List<Word>>> GetApprovedWordsPaginated(int PageNumber, int PageSize)
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                var _response = await _wordService.GetApprovedWordsPaginatedAsync(PageNumber, PageSize, _userId, _roles);

                return _response;

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

               _username: HttpContext.GetOztUser()?.Username,

               _userId: (long)(HttpContext.GetOztUser()?.UserId));

                return new ServiceResponse<List<Word>>(null, false, _ex.Message);

            }
        }


        [HttpPost("[action]")]
        [OztActionFilter(Permissions = "admin")]
        public async Task<ServiceResponse<Word>> UpdateWord(RequestUpdateWordDto _dto)
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                var _response = await _wordService.UpdateWordAsync(_dto.WordId, _dto.DescriptionId, _dto.WordContent, _dto.DescriptionContent, _userId, _roles);

                return _response;

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

               _username: HttpContext.GetOztUser()?.Username,

               _userId: (long)(HttpContext.GetOztUser()?.UserId));

                return new ServiceResponse<Word>(null, false, _ex.Message);
            }
        }


        [HttpPost("[action]")]
        [OztActionFilter(Permissions = "admin")]
        public async Task<ServiceResponse<bool>> DeleteWord(Guid WordId)
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                var _response = await _wordService.DeleteWordAsync(WordId, _userId, _roles);

                return _response;

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

               _username: HttpContext.GetOztUser()?.Username,

               _userId: (long)(HttpContext.GetOztUser()?.UserId));

                return new ServiceResponse<bool>(false, false, _ex.Message);

            }
        }

        [HttpPost("[action]")]
        [OztActionFilter]
        public async Task<ServiceResponse<Word>> RecommendWord(RequestRecommendWordDto _dto)
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                var _response = await _wordService.RecommendNewWordAsync(_dto.WordContent, _dto.DescriptionContent, _userId, _roles);

                return _response;

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

               _username: HttpContext.GetOztUser()?.Username,

               _userId: (long)(HttpContext.GetOztUser()?.UserId));

                return new ServiceResponse<Word>(null, false, _ex.Message);

            }
        }

        [HttpPost("[action]")]
        [OztActionFilter]
        public async Task<ServiceResponse<Guid>> LikeWord(RequestLikeWord _dto)
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                var _response = await _wordService.LikeWordAsync(_dto.WordId, _userId, _roles);

                return _response;

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

                _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

                _username: HttpContext.GetOztUser()?.Username,

                _userId: (long)(HttpContext.GetOztUser()?.UserId));

                return new ServiceResponse<Guid>(Guid.Empty, false, _ex.Message);

            }
        }

        [HttpPost("[action]")]
        [OztActionFilter(Permissions = "admin")]
        public async Task<ServiceResponse<Word>> UpdateWordById(RequestUpdateWordById _dto)
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                if (_roles == null || !_roles.Any())
                {

                    _roles = new List<string> { "user" };

                }

                var _response = await _wordService.UpdateWordByIdAsync(_dto.WordId, _dto.WordContent, _userId, _roles);

                return _response;

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

                _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

                _username: HttpContext.GetOztUser()?.Username,

                _userId: (long)(HttpContext.GetOztUser()?.UserId));

                return new ServiceResponse<Word>(null, false, _ex.Message);

            }
        }
    }

}
