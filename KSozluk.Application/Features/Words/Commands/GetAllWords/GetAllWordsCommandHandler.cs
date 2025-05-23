﻿using KSozluk.Application.Common;
using KSozluk.Application.Features.Words.Commands.GetWordsByLetter;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain.Resources;

namespace KSozluk.Application.Features.Words.Commands.GetAllWords
{
    public class GetAllWordsCommandHandler : RequestHandlerBase<GetAllWordsCommand, GetAllWordsResponse>
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IWordRepository _wordRepository;

        public GetAllWordsCommandHandler(IUserService userService, IUserRepository userRepository, IWordRepository wordRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
            _wordRepository = wordRepository;
        }

        public async override Task<GetAllWordsResponse> Handle(GetAllWordsCommand request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetUserId();

            if (!await _userRepository.HasPermissionForAdmin(userId))
            {
                return Response.Failure<GetAllWordsResponse>(OperationMessages.PermissionFailure);
            }

            var words = await _wordRepository.GetAllWordsAsync();

            if (!words.Any())
            {
                return Response.Failure<GetAllWordsResponse>(OperationMessages.PermissionFailure);
            }

            return Response.SuccessWithBody<GetAllWordsResponse>(words, OperationMessages.GettedAllWords);
        }
    }
}
