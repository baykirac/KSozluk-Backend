using KSozluk.Application.Common;
using KSozluk.Domain;
using KSozluk.Domain.Enums;

namespace KSozluk.Application.Features.Descriptions.Commands.UpdateStatus
{
    public class UpdateStatusCommand : CommandBase<UpdateStatusResponse>
    {
        public Guid DescriptionId { get; set; }
        public ContentStatus Status { get; set; }
        public int RejectionReasons { get; set; }
    }
}
