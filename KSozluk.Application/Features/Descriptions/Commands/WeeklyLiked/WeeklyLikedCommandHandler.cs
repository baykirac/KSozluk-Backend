using KSozluk.Application.Common;
using KSozluk.Application.Features.Descriptions.Commands.RecommendNewDescription;
using KSozluk.Application.Features.Words.Commands.LikeWord;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain.Resources;
using KSozluk.Domain;
using KSozluk.Domain.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KSozluk.Application.Features.Descriptions.Commands.GetDescriptions;
using KSozluk.Application.Features.Words.Commands.AddWord;

namespace KSozluk.Application.Features.Descriptions.Commands.WeeklyLiked
{
    public class WeeklyLikedCommandHandler : RequestHandlerBase<WeeklyLikedCommand, WeeklyLikedResponse>

    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly ILikeRepository _wordLikeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWordRepository _wordRepository;

        public WeeklyLikedCommandHandler(IUserService userService, IUserRepository userRepository, ILikeRepository wordLikeRepository, IUnitOfWork unitOfWork, IWordRepository wordRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
            _wordLikeRepository = wordLikeRepository;
            _unitOfWork = unitOfWork;
            _wordRepository = wordRepository;
        }

        public async override Task<WeeklyLikedResponse> Handle(WeeklyLikedCommand request, CancellationToken cancellationToken)
        {

            var userId = _userService.GetUserId();

            var response = new WeeklyLikedResponse();

            var _data = await _wordRepository.GetMostLikedWeekly();

            response.responseTopWordListDtos = _data;

        
            return Response.SuccessWithBody<WeeklyLikedResponse>(response, OperationMessages.DescriptionsGettedSuccessfully);

        }
    }
}
