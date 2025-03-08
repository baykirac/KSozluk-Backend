
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ozcorps.Ozt;
using Ozcorps.Logger;
using KSozluk.WebAPI.Repositories;
using KSozluk.WebAPI.Business;
using KSozluk.WebAPI.Entities;
using Azure.Core;
using KSozluk.WebAPI.DTOs;
using KSozluk.WebAPI.SharedKernel;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Npgsql.Internal;


namespace KSozluk.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [OztActionFilter]
    public class DescriptionController : ControllerBase
    {
        private readonly OztTool _OztTool;
        private readonly IOzLogger _Logger;
        private readonly IDescriptionService _descriptionService;

        public DescriptionController( IOzLogger logger, OztTool oztTool, IDescriptionService descriptionService)
        {
            _Logger = logger;
            _OztTool = oztTool;
            _descriptionService = descriptionService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetDescriptions(Guid WordId)
        {
            try{
               var UserId = HttpContext.GetOztUser()?.UserId; 
               var Roles = HttpContext.GetOztUser()?.Roles;

               var response = await _descriptionService.GetDescriptionsAsync(WordId, UserId, Roles);
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
        public async Task<BaseResponse<bool>> DeleteDescription(Guid DescriptionId)
        {
            try{
               var UserId = HttpContext.GetOztUser()?.UserId; 
               var Roles = HttpContext.GetOztUser()?.Roles;

               await _descriptionService.DeleteDescriptionAsync(DescriptionId, UserId, Roles);
               return new BaseResponse<bool>(true, "Açıklama silindi.", true);
            }
            catch (Exception _ex)
            {
                _Logger.Error(_ex,
               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
               _username: HttpContext.GetOztUser()?.Username,
               _userId: (long)(HttpContext.GetOztUser()?.UserId));
                return new BaseResponse<bool>(false, _ex.Message, false);
            }
        }

        [HttpPost("[action]")]
        public async Task<BaseResponse<bool>> UpdateOrder(RequestUpdateDto _dto)
        {
            try{
               var UserId = HttpContext.GetOztUser()?.UserId; 
               var Roles = HttpContext.GetOztUser()?.Roles;

               await _descriptionService.UpdateOrderAsync(_dto.DescriptionId, _dto.Order, UserId, Roles);

                return new BaseResponse<bool>(true, "Sıralama güncellendi.", true);
            }
            catch (Exception _ex)
            {
                _Logger.Error(_ex,
               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
               _username: HttpContext.GetOztUser()?.Username,
               _userId: (long)(HttpContext.GetOztUser()?.UserId));
                return new BaseResponse<bool>(false, _ex.Message, false);
            }
        }

        [HttpPost("[action]")]
        public async Task<BaseResponse<bool>> UpdateStatus(RequestUpdateStatusDto _dto)
        {
            try{
               var UserId = HttpContext.GetOztUser()?.UserId; 
               var Email =   HttpContext.GetOztUser()?.Email;
               var Roles = HttpContext.GetOztUser()?.Roles;

               await _descriptionService.UpdateStatusAsync(_dto.DescriptionId, _dto.Status, _dto.RejectionReasons, _dto.CustomRejectionReason, UserId,Email, Roles);

                return new BaseResponse<bool>(true, "Durum güncellendi.", true);

            }
            catch (Exception _ex)
            {
                _Logger.Error(_ex,
               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
               _username: HttpContext.GetOztUser()?.Username,
               _userId: (long)(HttpContext.GetOztUser()?.UserId));
                return new BaseResponse<bool>(false, _ex.Message, false);
            }
        }

        [HttpPost("[action]")]
        public async Task<BaseResponse<bool>> RecommendDescription(RequestRecommendDescriptionDto _dto)
        {
            try{
               var UserId = HttpContext.GetOztUser()?.UserId; 
               var Roles = HttpContext.GetOztUser()?.Roles;

              var response= await _descriptionService.RecommendNewDescriptionAsync(_dto.WordId, _dto.PreviousDescriptionId, _dto.Content, UserId, Roles);

             return new BaseResponse<bool>(true, "Açıklama önerildi.", true);

            }
            catch (Exception _ex)
            {
                _Logger.Error(_ex,
               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
               _username: HttpContext.GetOztUser()?.Username,
               _userId: (long)(HttpContext.GetOztUser()?.UserId));
                return new BaseResponse<bool>(false, _ex.Message, false);
            }
        }

        [HttpPost("[action]")]
        //[EnableRateLimiting("interact-limit")]
        public async Task<BaseResponse<bool>> DescriptionLike(RequestDescriptionLike _dto)
        {
            try{
               var UserId = HttpContext.GetOztUser()?.UserId; 
               var Roles = HttpContext.GetOztUser()?.Roles;

                var response = await _descriptionService.LikeDescriptionAsync(_dto.DescriptionId, UserId, Roles);

                return new BaseResponse<bool>(true, "Açıklama beğenildi.", true);

            }
            catch (Exception _ex)
            {
                _Logger.Error(_ex,
               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
               _username: HttpContext.GetOztUser()?.Username,
               _userId: (long)(HttpContext.GetOztUser()?.UserId));
                return new BaseResponse<bool>(false, _ex.Message, false);
            }
        }

        [HttpPost("[action]")]
        public async Task<BaseResponse<bool>> HeadersDescription(RequestHeadersDescriptionDto _dto)
        {
            try{
               var UserId = HttpContext.GetOztUser()?.UserId; 
               var Roles = HttpContext.GetOztUser()?.Roles;

                var response = await _descriptionService.HeaderDescriptionAsync(_dto.WordContent, UserId, Roles);

                return new BaseResponse<bool>(true, "Başlık açıklamaları alındı.", true);

            }
            catch (Exception _ex)
            {
                _Logger.Error(_ex,
               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
               _username: HttpContext.GetOztUser()?.Username,
               _userId: (long)(HttpContext.GetOztUser()?.UserId));
                return new BaseResponse<bool>(false, _ex.Message, false);
            }
        }

        [HttpPost("[action]")]
        public async Task<BaseResponse<bool>> FavouriteWord(RequestFavouriteWordDto _dto)
        {
            try{
               var UserId = HttpContext.GetOztUser()?.UserId; 
               var Roles = HttpContext.GetOztUser()?.Roles;

                var response = await _descriptionService.FavouriteWordAsync(_dto.WordId, UserId, Roles);

                return new BaseResponse<bool>(true, "Favori kelime eklendi.", true);

            }
            catch (Exception _ex)
            {
                _Logger.Error(_ex,
               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),
               _username: HttpContext.GetOztUser()?.Username,
               _userId: (long)(HttpContext.GetOztUser()?.UserId));
                return new BaseResponse<bool>(false, _ex.Message, false);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> FavouriteWordsOnScreen()
        {
            try{
               var UserId = HttpContext.GetOztUser()?.UserId; 
               var Roles = HttpContext.GetOztUser()?.Roles;

                var response = await _descriptionService.FavouriteWordsOnScreenAsync(UserId, Roles);

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
        public async Task<IActionResult> DescriptionTimeline()
        {
            try{
               var UserId = HttpContext.GetOztUser()?.UserId; 
               var Roles = HttpContext.GetOztUser()?.Roles;

                var response = await _descriptionService.DescriptionTimelineAsync(UserId);

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
    }
}
