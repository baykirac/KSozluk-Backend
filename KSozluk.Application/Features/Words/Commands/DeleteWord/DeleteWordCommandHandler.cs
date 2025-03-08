using KSozluk.Application.Common;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain.Resources;
using KSozluk.Domain.SharedKernel;
using MediatR.Wrappers;
namespace KSozluk.Application.Features.Words.Commands.DeleteWord
{
    public class DeleteWordCommandHandler : RequestHandlerBase<DeleteWordCommand, DeleteWordResponse>
    {
        private readonly IWordRepository _wordRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly IUnit _unit;

        public DeleteWordCommandHandler(IWordRepository wordRepository, ILikeRepository likeRepository, IUnit unit)
        {
            _wordRepository = wordRepository;
            _likeRepository = likeRepository;
            _unit = unit;
        }

        public async override Task<DeleteWordResponse> Handle(DeleteWordCommand request, CancellationToken cancellationToken)
        {               
             var userId = request.UserId;
             var userRoles = request.Roles; 

            if (!userRoles.Contains("admin"))
            {
                return Response.Failure<DeleteWordResponse>(OperationMessages.PermissionFailure);
            }

            await _likeRepository.DeleteWordLikesByWordIdAsync(request.WordId);
            await _wordRepository.DeleteAsync(request.WordId);
            await _unit.SaveChangesAsync();

            return Response.SuccessWithMessage<DeleteWordResponse>(OperationMessages.DeletedWordSuccessfully);
        }
    }
}
