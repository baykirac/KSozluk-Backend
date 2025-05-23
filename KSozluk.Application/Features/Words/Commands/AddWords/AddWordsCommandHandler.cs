﻿using KSozluk.Application.Common;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain;
using KSozluk.Domain.Resources;
using KSozluk.Domain.SharedKernel;

namespace KSozluk.Application.Features.Words.Commands.AddWords
{
    public class AddWordsCommandHandler : RequestHandlerBase<AddWordsCommand, AddWordsResponse>
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IWordRepository _wordRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddWordsCommandHandler(IUserService userService, IUserRepository userRepository, IWordRepository wordRepository, IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _userRepository = userRepository;
            _wordRepository = wordRepository;
            _unitOfWork = unitOfWork;
        }

        public async override Task<AddWordsResponse> Handle(AddWordsCommand request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetUserId();
            if (!await _userRepository.HasPermissionForAdmin(userId))
            {
                return Response.Failure<AddWordsResponse>(OperationMessages.PermissionFailure);
            }

            var existedWord = await _wordRepository.FindByContentAsync(request.WordContent);
            if (existedWord != null)
            {
                return Response.SuccessWithBody<AddWordsResponse>(new AddWordsResponse
                {
                    WordExists = true,
                    AddedWord = null
                }, OperationMessages.WordExist);
            }

            var word = Word.Create(request.WordContent, userId);
            await _wordRepository.CreateAsync(word);
            await _unitOfWork.SaveChangesAsync();
            Word.ClearResponse(word);

            return Response.SuccessWithBody<AddWordsResponse>(new AddWordsResponse
            {
                WordExists = false,
                AddedWord = word
            }, OperationMessages.WordAddedSuccessfully);
        }
    }
}