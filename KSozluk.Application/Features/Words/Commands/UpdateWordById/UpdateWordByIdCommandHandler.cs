using KSozluk.Application.Common;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain.Resources;
using KSozluk.Domain.SharedKernel;

namespace KSozluk.Application.Features.Words.Commands.UpdateWordById
{
    public class UpdateWordByIdCommandHandler : RequestHandlerBase<UpdateWordByIdCommand, UpdateWordByIdResponse>
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IWordRepository _wordRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateWordByIdCommandHandler(IUserService userService, IUserRepository userRepository,IWordRepository wordRepository, IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _userRepository = userRepository;
            _wordRepository = wordRepository;
            _unitOfWork = unitOfWork;
        }

        public async override Task<UpdateWordByIdResponse> Handle(UpdateWordByIdCommand request,CancellationToken cancellationToken)
        {
            var userId = _userService.GetUserId();
            if (!await _userRepository.HasPermissionForAdmin(userId))
            {
                return Response.Failure<UpdateWordByIdResponse>(OperationMessages.PermissionFailure);
            }

           

            var word = await _wordRepository.FindByIdAsync(request.WordId);

            if (word == null)
            {
                return Response.Failure<UpdateWordByIdResponse>(OperationMessages.PermissionFailure);
            }

            word.ChangeContent(request.WordContent);

            await _unitOfWork.SaveChangesAsync();
            return Response.SuccessWithBody<UpdateWordByIdResponse>(word,OperationMessages.WordUpdatedSuccessfully);
        }
    }
}