using Ozcorps.Ozt;
using Ozcorps.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KSozluk.WebAPI.Repositories;
using KSozluk.WebAPI.Business;
using KSozluk.WebAPI.DTOs;
using KSozluk.WebAPI.SharedKernel;
using Azure.Core;

namespace KSozluk.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [OztActionFilter]
    //[EnableRateLimiting("interact-limit")]
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
        public async Task<IActionResult> WeeklyLiked()
        {
            try{
               var UserId = HttpContext.GetOztUser()?.UserId; 
               var Roles = HttpContext.GetOztUser()?.Roles;
               var response = await _descriptionService.WeeklyLikedAsync(UserId, Roles);

                return Ok(response);
            }
            catch (Exception _ex)
            {
                _Logger.Error(_ex,
               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
               _username: HttpContext.GetOztUser()?.Username,
               _userId: (long)(HttpContext.GetOztUser()?.UserId));
                return BadRequest(_ex.Message);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllWords()
        {  
            try{
               var UserId = HttpContext.GetOztUser()?.UserId; 
               var Roles = HttpContext.GetOztUser()?.Roles;

                var response = await _wordService.GetAllWordsAsync(UserId, Roles);

                return Ok(response);
            }
            catch (Exception _ex)
            {
                _Logger.Error(_ex,
               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
               _username: HttpContext.GetOztUser()?.Username,
               _userId: (long)(HttpContext.GetOztUser()?.UserId));
                return BadRequest(_ex.Message);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetWordsByLetter(char Letter, int PageNumber, int PageSize)
        {
            try{
               var UserId = HttpContext.GetOztUser()?.UserId;  
               var Roles = HttpContext.GetOztUser()?.Roles;

                var response = await _wordService.GetWordsByLetterAsync(Letter, PageNumber, PageSize, UserId, Roles);

                return Ok(response);
            }
            catch (Exception _ex)
            {
                _Logger.Error(_ex,
               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
               _username: HttpContext.GetOztUser()?.Username,
               _userId: (long)(HttpContext.GetOztUser()?.UserId));
                return BadRequest(_ex.Message);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetWordsByContains(string Content)
        {
            try{
               var UserId = HttpContext.GetOztUser()?.UserId; 
               var Roles = HttpContext.GetOztUser()?.Roles;

                var response = await _wordService.GetWordsByContainsAsync(Content, UserId, Roles);

                return Ok(response);
            }
            catch (Exception _ex)
            {
                _Logger.Error(_ex,
               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
               _username: HttpContext.GetOztUser()?.Username,
               _userId: (long)(HttpContext.GetOztUser()?.UserId));
                return BadRequest(_ex.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<bool>> AddWord(RequestAddWords _dto)
        {       
            try{
                var UserId = HttpContext.GetOztUser()?.UserId;
                var Roles = HttpContext.GetOztUser()?.Roles;
                var response = await _wordService.AddWordAsync(_dto.WordContent, _dto.Description, UserId, Roles);
                return new ServiceResponse<bool>(true, true, "Kelime başarıyla eklendi.");
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

        [HttpGet("[action]")]
        public async Task<IActionResult> GetApprovedWordsPaginated(int PageNumber, int PageSize)
        {
            try{
               var UserId = HttpContext.GetOztUser()?.UserId; 
               var Roles = HttpContext.GetOztUser()?.Roles;

                var response = await _wordService.GetApprovedWordsPaginatedAsync(PageNumber, PageSize, UserId, Roles);

                return Ok(response);
            }
            catch (Exception _ex)
            {
                _Logger.Error(_ex,
               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
               _username: HttpContext.GetOztUser()?.Username,
               _userId: (long)(HttpContext.GetOztUser()?.UserId));
                return BadRequest(_ex.Message);
            }
        }


        [HttpPost("[action]")]
        public async Task<ServiceResponse<bool>> UpdateWord(RequestUpdateWordDto _dto)
        {   
            try{
                var UserId = HttpContext.GetOztUser()?.UserId ?? 0; 

                var Roles = HttpContext.GetOztUser()?.Roles;
                await _wordService.UpdateWordAsync(_dto.WordId, _dto.DescriptionId, _dto.WordContent, _dto.DescriptionContent, UserId, Roles);  
                return new ServiceResponse<bool>(true, true, "Kelime başarıyla güncellendi."); 
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
        public async Task<ServiceResponse<bool>> DeleteWord(Guid WordId)
        {
            try{
                var UserId = HttpContext.GetOztUser()?.UserId;  
                var Roles = HttpContext.GetOztUser()?.Roles;
                await _wordService.DeleteWordAsync(WordId, UserId, Roles);
                return new ServiceResponse<bool>(true, true, "Kelime başarıyla silindi.");
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
        public async Task<ServiceResponse<bool>> RecommendWord(RequestRecommendWordDto _dto)
        {
            try{
                var UserId = HttpContext.GetOztUser()?.UserId; 
                var Roles = HttpContext.GetOztUser()?.Roles;
                var response = await _wordService.RecommendNewWordAsync(_dto.WordContent, _dto.DescriptionContent, UserId, Roles);
                return new ServiceResponse<bool>(true, true, "Kelime önerisi başarıyla yapıldı.");
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
        //[EnableRateLimiting("interact-limit")]
        public async Task<ServiceResponse<bool>> LikeWord(RequestLikeWord _dto)
        {
            try{
                var UserId = HttpContext.GetOztUser()?.UserId;  
                var Roles = HttpContext.GetOztUser()?.Roles;
                var Response = await _wordService.LikeWordAsync(_dto.WordId, UserId, Roles);
                return new ServiceResponse<bool>(true, true, "Kelime beğenildi.");       
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
        public async Task<ServiceResponse<bool>> UpdateWordById(RequestUpdateWordById _dto)
        {
            try{
                var UserId = HttpContext.GetOztUser()?.UserId;  
                var Roles = HttpContext.GetOztUser()?.Roles;
                await _wordService.UpdateWordByIdAsync(_dto.WordId, _dto.WordContent, UserId, Roles);
                return new ServiceResponse<bool>(true, true, "Kelime başarıyla güncellendi.");
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
    }

}
