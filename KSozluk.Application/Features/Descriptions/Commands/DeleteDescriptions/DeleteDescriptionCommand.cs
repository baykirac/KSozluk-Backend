using KSozluk.Application.Common;

namespace KSozluk.Application.Features.Descriptions.Commands.DeleteDescriptions
{
    public class DeleteDescriptionCommand : CommandBase<DeleteDescriptionResponse>
    {
        public Guid DescriptionId { get; set; }
    }
}
