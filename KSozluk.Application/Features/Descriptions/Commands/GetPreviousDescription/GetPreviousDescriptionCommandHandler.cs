using KSozluk.Application.Common;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain.SharedKernel;

namespace KSozluk.Application.Features.Descriptions.Commands.GetPreviousDescription
{
    public class GetPreviousDescriptionCommandHandler : RequestHandlerBase<GetPreviousDescriptionCommand, GetPreviousDescriptionResponse>
    {
        private readonly IDescriptionRepository _descriptionRepository;
        private readonly IUnit _unit;

        public GetPreviousDescriptionCommandHandler(IDescriptionRepository descriptionRepository, IUnit unit)
        {
            _descriptionRepository = descriptionRepository;
            _unit = unit;
        }

        public override Task<GetPreviousDescriptionResponse> Handle(GetPreviousDescriptionCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
