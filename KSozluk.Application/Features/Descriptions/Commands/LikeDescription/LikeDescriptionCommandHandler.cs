// LikeDescriptionCommandHandler.cs
using KSozluk.Application.Common;
using KSozluk.Application.Features.Words.Commands.GetAllWords;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain;
using KSozluk.Domain.Resources;
using KSozluk.Domain.SharedKernel;
using MediatR.Wrappers;
using System;

namespace KSozluk.Application.Features.Descriptions.Commands.LikeDescription
{
    public class LikeDescriptionCommandHandler : RequestHandlerBase<LikeDescriptionCommand, LikeDescriptionResponse>
    {
        private readonly ILikeRepository _descriptionLikeRepository;
        private readonly IUnit _unit;


        public LikeDescriptionCommandHandler(ILikeRepository descriptionLikeRepository,IUnit unit)
        {
            _descriptionLikeRepository = descriptionLikeRepository;
            _unit = unit;
        }

        public async override Task<LikeDescriptionResponse> Handle(LikeDescriptionCommand request, CancellationToken cancellationToken)
        {

             var userId = request.UserId;
             var userRoles = request.Roles; 

            if (!userRoles.Contains("admin"))
            {
                return Response.Failure<LikeDescriptionResponse>(OperationMessages.PermissionFailure);
            }
           
            var existingLike = await _descriptionLikeRepository.GetByDescriptionAndUserAsync(request.DescriptionId, userId);

            if (existingLike != null)
            {
                _descriptionLikeRepository.DeleteDescriptionLike(existingLike);
                await _unit.SaveChangesAsync();
                
                return Response.SuccessWithBody<LikeDescriptionResponse>(request.DescriptionId, OperationMessages.DescriptionUnlikedSuccessfully);
            }
            else
            {
                var newLike = new DescriptionLike
                {
                    Id = Guid.NewGuid(),
                    DescriptionId = request.DescriptionId,
                    UserId = userId,
                    Timestamp = DateTime.UtcNow
                };

                await _descriptionLikeRepository.CreateDescriptionLike(newLike);
                await _unit.SaveChangesAsync();
                return Response.SuccessWithBody<LikeDescriptionResponse>(request.DescriptionId, OperationMessages.DescriptionLikedSuccessfully);

            }
        }
    }
}