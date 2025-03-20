using Ozcorps.Ozt;
using Ozcorps.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KSozluk.WebAPI.Business;
using KSozluk.WebAPI.DTOs;
using KSozluk.WebAPI.SharedKernel;
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
        private readonly IWordService _wordService;
        private readonly IDescriptionService _descriptionService;

        public WordController(IOzLogger logger, OztTool oztTool, IWordService wordService, IDescriptionService descriptionService)
        {
            _Logger = logger;
            _OztTool = oztTool;
            _wordService = wordService;
            _descriptionService = descriptionService;
        }

        [HttpGet("[action]")]
        [OztActionFilter]
        public async Task<ServiceResponse> WeeklyLiked()
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                var _response = await _descriptionService.WeeklyLikedAsync(_userId, _roles);

                return new ServiceResponse
                {
                    Data = _response,
                    Success = true, 
                    Message = "Begenilmişleri Getirildi"
                };

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

               _username: HttpContext.GetOztUser()?.Username,

               _userId: (long)(HttpContext.GetOztUser()?.UserId));

                
                return new ServiceResponse
                {

                    Data = null,

                    Success = false,

                    Message = _ex.Message
                    
                };
            }
        }

        [HttpGet("[action]")]
        [OztActionFilter(Permissions = "admin")]
        public async Task<ServiceResponse> GetAllWords(int pageNumber = 1 , int pageSize = 10)
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                var _response = await _wordService.GetAllWordsAsync(_userId, _roles, pageNumber, pageSize);

                return new ServiceResponse
                {
                    Data = _response,
                    Success = true, 
                    Message = "Kelimeleri getir"
                };

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

                    _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

                    _username: HttpContext.GetOztUser()?.Username,

                    _userId: (long)(HttpContext.GetOztUser()?.UserId));

                
                return new ServiceResponse
                {

                    Data = null,

                    Success = false,

                    Message = _ex.Message
                    
                };

            }
        }

        [HttpGet("[action]")]
        [OztActionFilter]
        public async Task<ServiceResponse> GetLastEdit()
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                var _response = await _wordService.GetLastEditDateAsync(_userId, _roles);

                return new ServiceResponse
                {

                    Data = _response,

                    Success = true, 

                    Message = "Son Yüklenenler Getirildi"

                };

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

                _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

                _username: HttpContext.GetOztUser()?.Username,

                _userId: (long)(HttpContext.GetOztUser()?.UserId));

                return new ServiceResponse
                {

                    Data = null,

                    Success = false,

                    Message = _ex.Message
                    
                };
            }
        }

        [HttpGet("[action]")]
        [OztActionFilter]
        public async Task<ServiceResponse> GetWordsByLetter([FromQuery] RequestWordsByLetterDto _dto)
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                var _response = await _wordService.GetWordsByLetterAsync(_dto.Letter, _dto.PageNumber, _dto.PageSize, _userId, _roles);

               return new ServiceResponse
                {

                    Data = _response,

                    Success = true, 

                    Message = "Kelime Harfleri getirildi"
                    
                };

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

               _username: HttpContext.GetOztUser()?.Username,

               _userId: (long)(HttpContext.GetOztUser()?.UserId));

                
                return new ServiceResponse
                {

                    Data = null,

                    Success = false,

                    Message = _ex.Message
                    
                };

            }
        }

        [HttpGet("[action]")]
        [OztActionFilter]
        public async Task<ServiceResponse> GetWordsByContains([FromQuery]  RequestWordsContainsDto _dto)
        {
            try
            {
                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                var _response = await _wordService.GetWordsByContainsAsync(_dto.Content, _userId, _roles);

                return new ServiceResponse
                {

                    Data = _response,

                    Success = true, 

                    Message = "Kelimeler getirildi"
                    
                };
            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

               _username: HttpContext.GetOztUser()?.Username,

               _userId: (long)(HttpContext.GetOztUser()?.UserId));

                
                return new ServiceResponse
                {

                    Data = null,

                    Success = false,

                    Message = _ex.Message
                    
                };

            }
        }


        [HttpGet("[action]")]
        [OztActionFilter(Permissions = "admin")]
        public async Task<ServiceResponse> GetApprovedWordsPaginated([FromQuery]  RequestApprovedDto _dto)
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                var _response = await _wordService.GetApprovedWordsPaginatedAsync(_dto.PageNumber, _dto.PageSize, _userId, _roles);

                return new ServiceResponse
                {

                    Data = _response,

                    Success = true, 

                    Message = "Kelime Sayfası Getirildi"
                    
                };

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

               _username: HttpContext.GetOztUser()?.Username,

               _userId: (long)(HttpContext.GetOztUser()?.UserId));

                
                return new ServiceResponse
                {

                    Data = null,

                    Success = false,

                    Message = _ex.Message
                    
                };

            }
        }

        [HttpPost("[action]")]
        [OztActionFilter(Permissions = "admin")]
        public async Task<ServiceResponse> AddWord(RequestAddWords _dto)
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                 await _wordService.AddWordAsync(_dto.WordContent, _dto.Description, _userId, _roles);

                return new ServiceResponse
                {

                    Data = null,

                    Success = true, 

                    Message = "Kelime Eklendi"
                    
                };

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

               _username: HttpContext.GetOztUser()?.Username,

               _userId: (long)(HttpContext.GetOztUser()?.UserId));

                
                return new ServiceResponse
                {

                    Data = null,

                    Success = false,

                    Message = _ex.Message
                    
                };

            }
        }


        [HttpPost("[action]")]
        [OztActionFilter(Permissions = "admin")]
        public async Task<ServiceResponse> UpdateWord(RequestUpdateWordDto _dto)
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                 await _wordService.UpdateWordAsync(_dto.WordId, _dto.DescriptionId, _dto.WordContent, _dto.DescriptionContent, _userId, _roles);

                return new ServiceResponse
                {

                    Data = null,

                    Success = true, 

                    Message = "Kelime Güncelendi."
                    
                };

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

               _username: HttpContext.GetOztUser()?.Username,

               _userId: (long)(HttpContext.GetOztUser()?.UserId));

                
                return new ServiceResponse
                {

                    Data = null,

                    Success = false,

                    Message = _ex.Message
                    
                };
            }
        }


        [HttpPost("[action]")]
        [OztActionFilter(Permissions = "admin")]
        public async Task<ServiceResponse> DeleteWord(RequestDeletedDto _dto)
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                await _wordService.DeleteWordAsync(_dto.WordId, _userId, _roles);

                return new ServiceResponse
                {

                    Data = null,

                    Success = true, 

                    Message = "Kelime Silindi."
                    
                };

            }
            catch (Exception _ex)
            {

               _Logger.Error(_ex,

               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

               _username: HttpContext.GetOztUser()?.Username,

               _userId: (long)(HttpContext.GetOztUser()?.UserId));

                
                return new ServiceResponse
                {

                    Data = null,

                    Success = false,

                    Message = _ex.Message
                    
                };

            }
        }

        [HttpPost("[action]")]
        [OztActionFilter]
        public async Task<ServiceResponse> RecommendWord(RequestRecommendWordDto _dto)
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                 await _wordService.RecommendNewWordAsync(_dto.WordContent, _dto.DescriptionContent, _userId, _roles);

                return new ServiceResponse
                {

                    Data = null,

                    Success = true, 

                    Message = "Öneri Kelime Eklendi"
                    
                };

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

               _username: HttpContext.GetOztUser()?.Username,

               _userId: (long)(HttpContext.GetOztUser()?.UserId));

                
                return new ServiceResponse
                {

                    Data = null,

                    Success = false,

                    Message = _ex.Message
                    
                };

            }
        }

        [HttpPost("[action]")]
        [OztActionFilter]
        public async Task<ServiceResponse> LikeWord(RequestLikeWord _dto)
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                await _wordService.LikeWordAsync(_dto.WordId, _userId, _roles);

                return new ServiceResponse
                {

                    Data = null,

                    Success = true, 

                    Message = "Kelime Begenildi"
                    
                };

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

                _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

                _username: HttpContext.GetOztUser()?.Username,

                _userId: (long)(HttpContext.GetOztUser()?.UserId));

                
                return new ServiceResponse
                {

                    Data = null,

                    Success = false,

                    Message = _ex.Message
                    
                };

            }
        }

        [HttpPost("[action]")]
        [OztActionFilter(Permissions = "admin")]
        public async Task<ServiceResponse> UpdateWordById(RequestUpdateWordById _dto)
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;
              
                await _wordService.UpdateWordByIdAsync(_dto.WordId, _dto.WordContent, _userId, _roles);

                return new ServiceResponse
                {

                    Data = null,

                    Success = true, 

                    Message = "Kelime Id Güncellendi."
                    
                };

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

                _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

                _username: HttpContext.GetOztUser()?.Username,

                _userId: (long)(HttpContext.GetOztUser()?.UserId));

                
                return new ServiceResponse
                {

                    Data = null,

                    Success = false,

                    Message = _ex.Message
                    
                };

            }
        }
    }

}
