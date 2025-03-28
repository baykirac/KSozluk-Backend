using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ozcorps.Ozt;
using Ozcorps.Logger;
using KSozluk.WebAPI.Business;
using KSozluk.WebAPI.Entities;
using Azure.Core;
using KSozluk.WebAPI.DTOs;
using KSozluk.WebAPI.SharedKernel;
using Microsoft.AspNetCore.RateLimiting;

namespace KSozluk.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [OzLoggerActionFilter]
    [EnableRateLimiting("interact-limit")]
    public class DescriptionController : ControllerBase
    {
        private readonly OztTool _OztTool;
        private readonly IOzLogger _Logger;
        private readonly IDescriptionService _descriptionService;

        public DescriptionController(IOzLogger logger, OztTool oztTool, IDescriptionService descriptionService)
        {
            _Logger = logger;

            _OztTool = oztTool;

            _descriptionService = descriptionService;
        }

        [HttpGet("[action]")]
        [OztActionFilter]
        public async Task<ServiceResponse> GetDescriptions(Guid WordId)
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                var _response = await _descriptionService.GetDescriptionsAsync(WordId, _userId, _roles);

                return new ServiceResponse
                {

                    Data = _response,

                    Success = true,

                    Message = "Açıklamalar Getirildi"

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
        public async Task<ServiceResponse> FavouriteWordsOnScreen()
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                var _response = await _descriptionService.FavouriteWordsOnScreenAsync(_userId, _roles);

                return new ServiceResponse
                {

                    Data = _response,

                    Success = true,

                    Message = "Favori Kelimeler Getirildi"

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
        public async Task<ServiceResponse> DescriptionTimeline()
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                var _response = await _descriptionService.DescriptionTimelineAsync(_userId);

                return new ServiceResponse
                {

                    Data = _response,

                    Success = true,

                    Message = "Açıklama Zaman Çizelgesi Getirildi."

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
        public async Task<ServiceResponse> DeleteDescription(RequestDeleteDescriptionDto _dto)
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                await _descriptionService.DeleteDescriptionAsync(_dto.DescriptionId, _userId, _roles);

                return new ServiceResponse
                {

                    Data = null,

                    Success = true,

                    Message = "Silme İşlemi Gerçekleşti."

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
        public async Task<ServiceResponse> UpdateOrder(RequestUpdateDto _dto)
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                await _descriptionService.UpdateOrderAsync(_dto.DescriptionId, _dto.Order, _userId, _roles);

                return new ServiceResponse
                {

                    Data = null,

                    Success = true,

                    Message = "Order Bilgisi Güncellendi"

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
        public async Task<ServiceResponse> UpdateStatus(RequestUpdateStatusDto _dto)
        {
            try
            {

                var _email = HttpContext.GetOztUser()?.Email;

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                await _descriptionService.UpdateStatusAsync(_dto.DescriptionId, _dto.Status, _dto.RejectionReasons, _dto.CustomRejectionReason, _dto.IsActive, _userId, _email, _roles);

                return new ServiceResponse
                {

                    Data = null,

                    Success = true,

                    Message = "Durum Güncellendi."

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
        public async Task<ServiceResponse> UpdateIsActive(RequestIsActiveDto _dto)
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                await _descriptionService.UpdateIsActiveAsync(_dto.DescriptionId, _dto.IsActive, _userId, _roles);

                return new ServiceResponse
                {

                    Data = null,

                    Success = true,

                    Message = "Durum Buttonuna Tıklandı."

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
        public async Task<ServiceResponse> RecommendDescription(RequestRecommendDescriptionDto _dto)
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                var _response = await _descriptionService.RecommendNewDescriptionAsync(_dto.WordId, _dto.PreviousDescriptionId, _dto.Content, _userId, _roles);

                return new ServiceResponse
                {

                    Data = _response,

                    Success = true,

                    Message = "Tavsiye Açıklama Eklendi"

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
        public async Task<ServiceResponse> DescriptionLike(RequestDescriptionLike _dto)
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                await _descriptionService.LikeDescriptionAsync(_dto.DescriptionId, _userId, _roles);

                return new ServiceResponse
                {

                    Data = null,

                    Success = true,

                    Message = "Açıklama Begenildi."

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
        public async Task<ServiceResponse> HeadersDescription(RequestHeadersDescriptionDto _dto)
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                await _descriptionService.HeaderDescriptionAsync(_dto.WordContent, _userId, _roles);

                return new ServiceResponse
                {

                    Data = null,

                    Success = true,

                    Message = "Kelime Eklendi."

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
        public async Task<ServiceResponse> FavouriteWord(RequestFavouriteWordDto _dto)
        {
            try
            {

                var _userId = HttpContext.GetOztUser()?.UserId;

                var _roles = HttpContext.GetOztUser()?.Roles;

                await _descriptionService.FavouriteWordAsync(_dto.WordId, _userId, _roles);

                return new ServiceResponse
                {

                    Data = null,

                    Success = true,

                    Message = "Kelime Favorilere Eklendi."

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
