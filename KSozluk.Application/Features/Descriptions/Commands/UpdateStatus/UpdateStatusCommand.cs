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
        public string CustomRejectionReason { get; set; }
        public long UserId { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}
