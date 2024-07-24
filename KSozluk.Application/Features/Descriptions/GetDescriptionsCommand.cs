using KSozluk.Application.Common;

namespace KSozluk.Application.Features.Descriptions
{
    public class GetDescriptionsCommand : CommandBase<GetDescriptionsResponse>
    {
        public Guid WordId { get; set; }
    }
}
