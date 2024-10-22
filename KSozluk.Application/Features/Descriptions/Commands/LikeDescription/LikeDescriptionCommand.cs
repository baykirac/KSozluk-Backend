using KSozluk.Application.Common;

namespace KSozluk.Application.Features.Descriptions.Commands.LikeDescription
{
    public class LikeDescriptionCommand : CommandBase<LikeDescriptionResponse>
    {
        public Guid DescriptionId { get; set; }
    }
}