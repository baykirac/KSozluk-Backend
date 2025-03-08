using KSozluk.Application.Common;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain;
using KSozluk.Domain.DTOs;
using KSozluk.Domain.Resources;
using KSozluk.Domain.SharedKernel;

namespace KSozluk.Application.Features.Words.Commands.AddWord
{
    public class AddWordCommandHandler : RequestHandlerBase<AddWordCommand, AddWordResponse>
    {
        private readonly IWordRepository _wordRepository;
        private readonly IUnit _unit;
        private readonly IDescriptionRepository _descriptionRepository;


        public AddWordCommandHandler( IWordRepository wordRepository, IUnit unit, IDescriptionRepository descriptionRepository)
        {

            _wordRepository = wordRepository;
            _unit = unit;
            _descriptionRepository = descriptionRepository;
        }

        public async override Task<AddWordResponse> Handle(AddWordCommand request, CancellationToken cancellationToken) //admin eklemesi
        {
             var userId = request.UserId;
             var userRoles = request.Roles; 

            if (!userRoles.Contains("admin"))
            {
                return Response.Failure<AddWordResponse>(OperationMessages.PermissionFailure);
            }

            var existedWord = await _wordRepository.FindByContentAsync(request.WordContent);


            if (existedWord != null) // kelime mevcutsa mevcut kelimeye sadece anlamı eklenecek
            {


                int greatestOrder = await _descriptionRepository.FindGreatestOrder(existedWord.Id);
                int order = greatestOrder + 1;

                foreach (var descriptionText in request.Description)
                {
                    var description = Description.Create(descriptionText, order, userId);
                    existedWord.AddDescription(description);
                }

                await _unit.SaveChangesAsync();


                return Response.SuccessWithBody<AddWordResponse>(existedWord, OperationMessages.DescriptionAddedtoTheExistingWordSuccessfully);
            }

            var word = Word.Create(request.WordContent, userId);

            int newOrder = 0;
            await _wordRepository.CreateAsync(word);
            foreach (var descriptionText in request.Description)
            {
                var description = Description.Create(descriptionText, newOrder++, userId);
                word.AddDescription(description);
            }




            await _unit.SaveChangesAsync();
            Word.ClearResponse(word);
            return Response.SuccessWithBody<AddWordResponse>(word, OperationMessages.WordAddedSuccessfully);
        }
    }
}