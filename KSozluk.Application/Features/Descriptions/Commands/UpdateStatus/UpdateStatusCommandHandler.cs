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
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IDescriptionRepository _descriptionRepository;
        private readonly IWordRepository _wordRepository;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateStatusCommandHandler(IUserService userService, IUserRepository userRepository, IDescriptionRepository descriptionRepository, IEmailService emailService, IUnitOfWork unitOfWork, IWordRepository wordRepository = null)
        {
            _userService = userService;
            _userRepository = userRepository;
            _descriptionRepository = descriptionRepository;
            _unitOfWork = unitOfWork;
            _wordRepository = wordRepository;
            _emailService = emailService;
        }

        public async override Task<UpdateStatusResponse> Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetUserId();
            if (!await _userRepository.HasPermissionForAdmin(userId))
            {
                return Response.Failure<UpdateStatusResponse>(OperationMessages.PermissionFailure);
            }

            var description = await _descriptionRepository.FindAsync(request.DescriptionId);
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

            var word = await _wordRepository.FindAsync(description.WordId);
            if (word.Descriptions.Count == 1 || word.Descriptions.All(d => d.Status == ContentStatus.Bekliyor))
            {
                word.UpdateStatus(request.Status);
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
                RejectionReasons enumValue = (RejectionReasons)request.RejectionReasons;
                var rejectionDescription = GetEnumDescription(enumValue);

                // Email sending part
                var user = await _userRepository.FindAsync(description.RecommenderId);
                if (user != null && !string.IsNullOrEmpty(user.Email))
                {
                    try
                    {
                        await _emailService.SendEmailAsync(
                            user.Email,
                            "Öneriniz Reddedildi",
                            $"Merhaba {user.FullName}, {_responseDescriptionRecommendDto.LastEditedDate} tarihinde \"{word.WordContent}\" kelimesi için yaptığınız \"{_responseDescriptionRecommendDto.DescriptionContent}\" öneriniz şu sebeple reddedildi: {rejectionDescription}"
                        );
                    }
                    catch (Exception ex)
                    {
                        return Response.Failure<UpdateStatusResponse>(OperationMessages.SuggestionRejectedSuccessfully);
                    }
                }
            }

            await _unitOfWork.SaveChangesAsync();
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