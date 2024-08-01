using KSozluk.Application.Common;
using KSozluk.Application.Features.Words.Commands.AddWord;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain.Resources;
using KSozluk.Domain.SharedKernel;
using MediatR.Wrappers;

namespace KSozluk.Application.Features.Words.Commands.DeleteWord
{
    public class DeleteWordCommandHandler : RequestHandlerBase<DeleteWordCommand, DeleteWordResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IWordRepository _wordRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteWordCommandHandler(IUserRepository userRepository, IUserService userService, IWordRepository wordRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _userService = userService;
            _wordRepository = wordRepository;
            _unitOfWork = unitOfWork;
        }

        public async override Task<DeleteWordResponse> Handle(DeleteWordCommand request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetUserId();

            if (!await _userRepository.HasPermissionForAdmin(userId))
            {
                return Response.Failure<DeleteWordResponse>(OperationMessages.PermissionFailure);
            }

            await _wordRepository.DeleteAsync(request.WordId);
            await _unitOfWork.SaveChangesAsync();

            return Response.SuccessWithMessage<DeleteWordResponse>(OperationMessages.DeletedWordSuccessfully);
        }
    }
}
