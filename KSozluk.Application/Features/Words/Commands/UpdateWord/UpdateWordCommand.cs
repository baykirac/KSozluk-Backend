using KSozluk.Application.Common;
using KSozluk.Domain;

namespace KSozluk.Application.Features.Words.Commands.UpdateWord
{
    public class UpdateWordCommand : CommandBase<UpdateWordResponse>
    {
        public Guid WordId { get; set; }
        public Guid DescriptionId { get; set; }
        public string WordContent { get; set; }
        public string DescriptionContent { get; set; }
        public long UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}
