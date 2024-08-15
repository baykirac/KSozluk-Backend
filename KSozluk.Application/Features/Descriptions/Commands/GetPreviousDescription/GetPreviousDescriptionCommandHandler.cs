using KSozluk.Application.Common;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain.SharedKernel;
using MediatR.Wrappers;

namespace KSozluk.Application.Features.Descriptions.Commands.GetPreviousDescription
{
    public class GetPreviousDescriptionCommandHandler : RequestHandlerBase<GetPreviousDescriptionCommand, GetPreviousDescriptionResponse>
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IDescriptionRepository _descriptionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GetPreviousDescriptionCommandHandler(IUserService userService, IUserRepository userRepository, IDescriptionRepository descriptionRepository, IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _userRepository = userRepository;
            _descriptionRepository = descriptionRepository;
            _unitOfWork = unitOfWork;
        }

        public override Task<GetPreviousDescriptionResponse> Handle(GetPreviousDescriptionCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
