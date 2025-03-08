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
        private readonly ILikeRepository _wordLikeRepository;
        private readonly IUnit _unit;
        private readonly IWordRepository _wordRepository;

        public WeeklyLikedCommandHandler(ILikeRepository wordLikeRepository, IUnit unit, IWordRepository wordRepository)
        {
            _wordLikeRepository = wordLikeRepository;
            _unit = unit;
            _wordRepository = wordRepository;
        }

        public async override Task<WeeklyLikedResponse> Handle(WeeklyLikedCommand request, CancellationToken cancellationToken)
        {
           var userId = request.UserId;

            var response = new WeeklyLikedResponse();

            var _data = await _wordRepository.GetMostLikedWeekly();

            response.responseTopWordListDtos = _data;

        
            return Response.SuccessWithBody<WeeklyLikedResponse>(response, OperationMessages.DescriptionsGettedSuccessfully);

        }
    }
}
