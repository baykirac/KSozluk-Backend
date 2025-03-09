
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
        public async Task<ServiceResponse<List<DescriptionWithIsLikeDto>>> GetDescriptions(Guid WordId)
        {
            try{

               var _userId = HttpContext.GetOztUser()?.UserId; 

               var _roles = HttpContext.GetOztUser()?.Roles;

               var _response = await _descriptionService.GetDescriptionsAsync(WordId, _userId, _roles);

               return _response;

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

               _username: HttpContext.GetOztUser()?.Username,

               _userId: (long)(HttpContext.GetOztUser()?.UserId));

                return new ServiceResponse<List<DescriptionWithIsLikeDto>>(null, false, _ex.Message);
                
            }
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<Description>> DeleteDescription(Guid DescriptionId)
        {
            try{

               var _userId = HttpContext.GetOztUser()?.UserId; 

               var _roles = HttpContext.GetOztUser()?.Roles;

               var _response = await _descriptionService.DeleteDescriptionAsync(DescriptionId, _userId, _roles);
               
               return _response;

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

               _username: HttpContext.GetOztUser()?.Username,

               _userId: (long)(HttpContext.GetOztUser()?.UserId));

                return new ServiceResponse<Description>(null, false, _ex.Message);
                
            }
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<Description>> UpdateOrder(RequestUpdateDto _dto)
        {
            try{

               var _userId = HttpContext.GetOztUser()?.UserId; 

               var _roles = HttpContext.GetOztUser()?.Roles;

               var _response =await _descriptionService.UpdateOrderAsync(_dto.DescriptionId, _dto.Order, _userId, _roles);

                return _response;

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

               _username: HttpContext.GetOztUser()?.Username,

               _userId: (long)(HttpContext.GetOztUser()?.UserId));

                return new ServiceResponse<Description>(null, false, _ex.Message);
                
            }
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<Description>> UpdateStatus(RequestUpdateStatusDto _dto)
        {
            try{

               var _email =   HttpContext.GetOztUser()?.Email;

               var _userId = HttpContext.GetOztUser()?.UserId; 

               var _roles = HttpContext.GetOztUser()?.Roles;

               var _response = await _descriptionService.UpdateStatusAsync(_dto.DescriptionId, _dto.Status, _dto.RejectionReasons, _dto.CustomRejectionReason, _userId, _email, _roles);

                return _response;

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

               _username: HttpContext.GetOztUser()?.Username,

               _userId: (long)(HttpContext.GetOztUser()?.UserId));

                return new ServiceResponse<Description>(null, false, _ex.Message);
                
            }
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<Description>> RecommendDescription(RequestRecommendDescriptionDto _dto)
        {
            try{

               var _userId = HttpContext.GetOztUser()?.UserId; 

               var _roles = HttpContext.GetOztUser()?.Roles;

               var _response = await _descriptionService.RecommendNewDescriptionAsync(_dto.WordId, _dto.PreviousDescriptionId, _dto.Content, _userId, _roles);

                return _response;

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

               _username: HttpContext.GetOztUser()?.Username,

               _userId: (long)(HttpContext.GetOztUser()?.UserId));

                return new ServiceResponse<Description>(null, false, _ex.Message);
                
            }
        }

        [HttpPost("[action]")]
        //[EnableRateLimiting("interact-limit")]
        public async Task<ServiceResponse<Guid>> DescriptionLike(RequestDescriptionLike _dto)
        {
            try{

               var _userId = HttpContext.GetOztUser()?.UserId; 

               var _roles = HttpContext.GetOztUser()?.Roles;

               var _response =await _descriptionService.LikeDescriptionAsync(_dto.DescriptionId, _userId, _roles);

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
        public async Task<ServiceResponse<List<DescriptionHeaderNameDto>>> HeadersDescription(RequestHeadersDescriptionDto _dto)
        {
            try{

               var _userId = HttpContext.GetOztUser()?.UserId; 

               var _roles = HttpContext.GetOztUser()?.Roles;

               var _response = await _descriptionService.HeaderDescriptionAsync(_dto.WordContent, _userId, _roles);

                return _response;

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

               _username: HttpContext.GetOztUser()?.Username,

               _userId: (long)(HttpContext.GetOztUser()?.UserId));

                return new ServiceResponse<List<DescriptionHeaderNameDto>>(null, false, _ex.Message);
                
            }
        }

        [HttpPost("[action]")]
        public async Task<ServiceResponse<Guid>> FavouriteWord(RequestFavouriteWordDto _dto)
        {
            try{

               var _userId = HttpContext.GetOztUser()?.UserId; 

               var _roles = HttpContext.GetOztUser()?.Roles;

               var _response = await _descriptionService.FavouriteWordAsync(_dto.WordId, _userId, _roles);

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

        [HttpGet("[action]")]
        public async Task<ServiceResponse<List<ResponseFavouriteWordContentDto>>> FavouriteWordsOnScreen()
        {
            try{

               var _userId = HttpContext.GetOztUser()?.UserId; 

               var _roles = HttpContext.GetOztUser()?.Roles;

               var _response = await _descriptionService.FavouriteWordsOnScreenAsync(_userId, _roles);

                return _response;

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

               _username: HttpContext.GetOztUser()?.Username,

               _userId: (long)(HttpContext.GetOztUser()?.UserId));

                return new ServiceResponse<List<ResponseFavouriteWordContentDto>>(null, false, _ex.Message);
                
            }
        }

        [HttpGet("[action]")]
        public async Task<ServiceResponse<List<DescriptionTimelineDto>>> DescriptionTimeline()
        {
            try{

               var _userId = HttpContext.GetOztUser()?.UserId; 

               var _roles = HttpContext.GetOztUser()?.Roles;

               var _response = await _descriptionService.DescriptionTimelineAsync(_userId);

                return _response;

            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex,

               _ip: HttpContext.Connection.RemoteIpAddress?.ToString(),

               _username: HttpContext.GetOztUser()?.Username,

               _userId: (long)(HttpContext.GetOztUser()?.UserId));

                return new ServiceResponse<List<DescriptionTimelineDto>>(null, false, _ex.Message);

            }
        }
    }
}
