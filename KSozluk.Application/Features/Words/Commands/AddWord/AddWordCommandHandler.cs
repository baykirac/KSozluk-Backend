using KSozluk.Application.Common;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain;
using KSozluk.Domain.Resources;
using KSozluk.Domain.SharedKernel;

namespace KSozluk.Application.Features.Words.Commands.AddWord
{
    public class AddWordCommandHandler : RequestHandlerBase<AddWordCommand, AddWordResponse>
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IWordRepository _wordRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDescriptionRepository _descriptionRepository;

        public AddWordCommandHandler(IUserService userService, IUserRepository userRepository, IWordRepository wordRepository, IUnitOfWork unitOfWork, IDescriptionRepository descriptionRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
            _wordRepository = wordRepository;
            _unitOfWork = unitOfWork;
            _descriptionRepository = descriptionRepository;
        }

        public async override Task<AddWordResponse> Handle(AddWordCommand request, CancellationToken cancellationToken) //admin eklemesi
        {
            var userId = _userService.GetUserId();

            if (!await _userRepository.HasPermissionForAdmin(userId))
            {
                return Response.Failure<AddWordResponse>(OperationMessages.PermissionFailure);
            }

            var existedWord = await _wordRepository.FindByContentAsync(request.WordContent);


            if (existedWord != null) // kelime mevcutsa mevcut kelimeye sadece anlamı eklenecek
            {
                var greatestOrder = await _descriptionRepository.FindGreatestOrder(existedWord.Id);
                var order = greatestOrder + 1024;

                var description = Description.Create(request.Description, order, userId);
                existedWord.AddDescription(description);
                await _unitOfWork.SaveChangesAsync();
                return Response.SuccessWithBody<AddWordResponse>(existedWord, OperationMessages.DescriptionAddedtoTheExistingWordSuccessfully);
            }

            var word = Word.Create(request.WordContent, userId);

            double newOrder = 1000;
            var newDescription = Description.Create(request.Description, newOrder, userId);


            word.AddDescription(newDescription);

            await _wordRepository.CreateAsync(word);


            await _unitOfWork.SaveChangesAsync();
            Word.ClearResponse(word);
            return Response.SuccessWithBody<AddWordResponse>(word, OperationMessages.WordAddedSuccessfully);
        }
    }
}
