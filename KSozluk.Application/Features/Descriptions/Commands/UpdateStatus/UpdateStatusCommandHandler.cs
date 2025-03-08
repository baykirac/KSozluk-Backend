using KSozluk.Application.Common;
using KSozluk.Application.Features.Words.Commands.UpdateWord;
using KSozluk.Application.Services.Authentication;
using KSozluk.Application.Services.Repositories;
using KSozluk.Domain;
using KSozluk.Domain.DTOs;
using KSozluk.Domain.Enums;
using KSozluk.Domain.Resources;
using KSozluk.Domain.SharedKernel;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;


namespace KSozluk.Application.Features.Descriptions.Commands.UpdateStatus
{
    public class UpdateStatusCommandHandler : RequestHandlerBase<UpdateStatusCommand, UpdateStatusResponse>
    {
        private readonly IDescriptionRepository _descriptionRepository;
        private readonly IWordRepository _wordRepository;
        private readonly IEmailService _emailService;
        private readonly IUnit _unit;

        public UpdateStatusCommandHandler(IDescriptionRepository descriptionRepository, IEmailService emailService, IUnit unit, IWordRepository wordRepository = null)
        {
            _descriptionRepository = descriptionRepository;
            _unit = unit;
            _wordRepository = wordRepository;
            _emailService = emailService;
        }

        public async override Task<UpdateStatusResponse> Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
        {
             var userId = request.UserId;
             var userRoles = request.Roles; 

            if (!userRoles.Contains("admin"))
            {
                return Response.Failure<UpdateStatusResponse>(OperationMessages.PermissionFailure);
            }

            var description = await _descriptionRepository.FindAsync(request.DescriptionId);
            var word = await _wordRepository.FindAsync(description.WordId);
            var previousDescriptionId = description.PreviousDescriptionId;

            if (previousDescriptionId.HasValue && request.Status == ContentStatus.Onaylı)
            {
                var previousDescription = await _descriptionRepository.FindAsync(previousDescriptionId);
                previousDescription.UpdateStatus(ContentStatus.Reddedildi);
            }

            var parentDescription = await _descriptionRepository.FindParentDescription(request.DescriptionId);
            if (parentDescription is not null && request.Status == ContentStatus.Onaylı)
            {
                parentDescription.UpdateStatus(ContentStatus.Reddedildi);
            }

            if (request.Status == ContentStatus.Onaylı)
            {
                word.UpdateStatus(ContentStatus.Onaylı);
            }

            else if (request.Status == ContentStatus.Reddedildi || request.Status == ContentStatus.Bekliyor)
            {
                var hasOtherApprovedDescriptions = word.Descriptions
                    .Any(d => d.Id != description.Id && d.Status == ContentStatus.Onaylı);

                if (!hasOtherApprovedDescriptions && word.Descriptions.All(d =>
                    d.Status == ContentStatus.Reddedildi || d.Status == ContentStatus.Bekliyor))
                {
                    word.UpdateStatus(ContentStatus.Reddedildi);
                }
            }

            description.UpdateStatus(request.Status);
            description.UpdateAcceptor(userId);

            
            var descriptionDto = await _descriptionRepository.GetById(x=>x.Id == description.Id && x.RecommenderId == description.RecommenderId);

            if (descriptionDto == null)
            {
                return Response.Failure<UpdateStatusResponse>(OperationMessages.PermissionFailure);
            }

            var _responseDescriptionRecommendDto = new DescriptionReccomendDto()
            {
                DescriptionContent = descriptionDto.DescriptionContent,
                LastEditedDate = descriptionDto.LastEditedDate,
            };

            // Only handle rejection reasons when status is Reddedildi
            if (request.Status == ContentStatus.Reddedildi)
            {
                string rejectionDescription;
            
            //when RejectionReasons is 7
            if (request.RejectionReasons == 7 && !string.IsNullOrEmpty(request.CustomRejectionReason))
            {
                rejectionDescription = request.CustomRejectionReason; 
            }
            else
            {
                //enum reasons (1-6)
                RejectionReasons enumValue = (RejectionReasons)request.RejectionReasons;
                rejectionDescription = GetEnumDescription(enumValue);
            }


                var userEmail = request.Email;
                if (userEmail != null && !string.IsNullOrEmpty(userEmail))
                {
                    try
                    {
                        await _emailService.SendEmailAsync(
                            userEmail,
                            "Öneriniz Reddedildi",
                            $"Merhaba {userEmail}, {_responseDescriptionRecommendDto.LastEditedDate} tarihinde \"{word.WordContent}\" kelimesi için yaptığınız \"{_responseDescriptionRecommendDto.DescriptionContent}\" öneriniz şu sebeple reddedildi: {rejectionDescription}"
                        );
                    }
                    catch (Exception ex)
                    {
                        return Response.Failure<UpdateStatusResponse>(OperationMessages.SuggestionRejectedSuccessfully);
                    }
                }
            }

            // Only handle rejection reasons when status is Onaylı
            if (request.Status == ContentStatus.Onaylı)
            {
                var userEmail = request.Email;

                if (userEmail != null && !string.IsNullOrEmpty(userEmail))
                {
                    try
                    {
                        await _emailService.SendEmailAsync(
                            userEmail,
                            "Öneriniz Onaylandı",
                            $"Merhaba {userEmail}, {_responseDescriptionRecommendDto.LastEditedDate} tarihinde \"{word.WordContent}\" kelimesi için yaptığınız \"{_responseDescriptionRecommendDto.DescriptionContent}\" öneriniz onaylandı."
                        );
                    }
                    catch (Exception ex)
                    {
                        return Response.Failure<UpdateStatusResponse>(OperationMessages.SuggestionRejectedSuccessfully);
                    }
                }
            }


            await _unit.SaveChangesAsync();
            return Response.SuccessWithMessage<UpdateStatusResponse>(OperationMessages.UpdatedDescriptionStatusSuccessfully);
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute != null ? attribute.Description : value.ToString();
        }
    }
    }