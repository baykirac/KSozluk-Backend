using KSozluk.Application.Common;

namespace KSozluk.Application.Features.Descriptions.Commands.GetDescriptions
{
    public class GetDescriptionsCommand : CommandBase<GetDescriptionsResponse>
    {
        public Guid WordId { get; set; }
    }
}
