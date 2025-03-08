using KSozluk.Application.Common;

namespace KSozluk.Application.Features.Descriptions.Commands.RecommendNewDescription
{
    public class RecommendNewDescriptionCommand : CommandBase<RecommendNewDescriptionResponse>
    {
        public Guid WordId { get; set; }
        public Guid? PreviousDescriptionId { get; set; }
        public string Content { get; set; }
        public long UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}
