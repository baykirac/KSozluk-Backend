using KSozluk.Application.Common;
using KSozluk.Application.Features.Words.Commands.LikeWord;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain.Resources;


namespace KSozluk.Application.Features.Descriptions.Commands.HeadersDescription
{
    public class HeadersDescriptionCommandHandler : RequestHandlerBase<HeadersDescriptionCommand, HeadersDescriptionResponse>
    {
        private readonly IWordRepository _wordRepository;
        private readonly IDescriptionRepository _descriptionRepository;


        public HeadersDescriptionCommandHandler(IWordRepository wordRepository, IDescriptionRepository descriptionRepository)
        {
            _wordRepository = wordRepository;
            _descriptionRepository = descriptionRepository;
        }

        public async override Task<HeadersDescriptionResponse> Handle(HeadersDescriptionCommand request, CancellationToken cancellationToken)
        {
             var userId = request.UserId;
             var userRoles = request.Roles; 

            if (!userRoles.Contains("admin"))
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
