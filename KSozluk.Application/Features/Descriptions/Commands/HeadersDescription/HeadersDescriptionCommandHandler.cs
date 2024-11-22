using KSozluk.Application.Common;
using KSozluk.Application.Features.Words.Commands.LikeWord;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.Application.Features.Descriptions.Commands.HeadersDescription
{
    public class HeadersDescriptionCommandHandler : RequestHandlerBase<HeadersDescriptionCommand, HeadersDescriptionResponse>
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IWordRepository _wordRepository;
        private readonly IDescriptionRepository _descriptionRepository;

        public HeadersDescriptionCommandHandler(IUserService userService, IUserRepository userRepository, IWordRepository wordRepository, IDescriptionRepository descriptionRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
            _wordRepository = wordRepository;
            _descriptionRepository = descriptionRepository;
        }

        public async override Task<HeadersDescriptionResponse> Handle(HeadersDescriptionCommand request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetUserId();

            if (!await _userRepository.HasPermissionView(userId))
            {
                return Response.Failure<HeadersDescriptionResponse>(OperationMessages.PermissionFailure);
            }


            var word = await _wordRepository.FindByContentAsync(request.WordContent);
            if (word == null)
            {
                return Response.Failure<HeadersDescriptionResponse>(OperationMessages.PermissionFailure);
            }

            var descriptions = await _descriptionRepository.FindHeaderByWordAsync(word.Id);


            return Response.SuccessWithBody<HeadersDescriptionResponse>(new HeadersDescriptionResponse
            {
                WordId = word.Id,
                Descriptions = descriptions
            }, OperationMessages.DescriptionsGettedSuccessfully);
        }
    }
}
