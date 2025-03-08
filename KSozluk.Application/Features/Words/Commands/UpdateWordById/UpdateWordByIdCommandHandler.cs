using KSozluk.Application.Common;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain.Resources;
using KSozluk.Domain.SharedKernel;


namespace KSozluk.Application.Features.Words.Commands.UpdateWordById
{
    public class UpdateWordByIdCommandHandler : RequestHandlerBase<UpdateWordByIdCommand, UpdateWordByIdResponse>
    {
        private readonly IWordRepository _wordRepository;
        private readonly IUnit _unit;

        public UpdateWordByIdCommandHandler(IWordRepository wordRepository, IUnit unit)
        {
            _wordRepository = wordRepository;
            _unit = unit;
        }

        public async override Task<UpdateWordByIdResponse> Handle(UpdateWordByIdCommand request,CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var userRoles = request.Roles; 

            if (!userRoles.Contains("admin"))
            {
                return Response.Failure<UpdateWordByIdResponse>(OperationMessages.PermissionFailure);
            }

           

            var word = await _wordRepository.FindByIdAsync(request.WordId);

            if (word == null)
            {
                return Response.Failure<UpdateWordByIdResponse>(OperationMessages.PermissionFailure);
            }

            word.ChangeContent(request.WordContent);

            await _unit.SaveChangesAsync();
            return Response.SuccessWithBody<UpdateWordByIdResponse>(word,OperationMessages.WordUpdatedSuccessfully);
        }
    }
}