﻿using KSozluk.Application.Common;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain.Resources;

namespace KSozluk.Application.Features.Words.Commands.GetApprovedWordsPaginated
{
    public class GetApprovedWordsPaginatedCommandHandler : RequestHandlerBase<GetApprovedWordsPaginatedCommand, GetApprovedWordsPaginatedResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IWordRepository _wordRepository;

        public GetApprovedWordsPaginatedCommandHandler(IUserRepository userRepository, IUserService userService, IWordRepository wordRepository)
        {
            _userRepository = userRepository;
            _userService = userService;
            _wordRepository = wordRepository;
        }

        public async override Task<GetApprovedWordsPaginatedResponse> Handle(GetApprovedWordsPaginatedCommand request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetUserId();

            if (!await _userRepository.HasPermissionForAdmin(userId))
            {
                return Response.Failure<GetApprovedWordsPaginatedResponse>(OperationMessages.PermissionFailure);
            }

            var words = await _wordRepository.GetAllWordsByPaginate(request.PageNumber, request.PageSize);

            if (!words.Any())
            {
                return Response.Failure<GetApprovedWordsPaginatedResponse>(OperationMessages.PermissionFailure);
            }

            return Response.SuccessWithBody<GetApprovedWordsPaginatedResponse>(words, OperationMessages.WordsGettedSuccessfully);
        }
    }
}
