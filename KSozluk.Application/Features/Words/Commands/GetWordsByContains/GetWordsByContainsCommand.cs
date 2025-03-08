using KSozluk.Application.Common;

namespace KSozluk.Application.Features.Words.Commands.GetWordsByContains
{
    public class GetWordsByContainsCommand : CommandBase<GetWordsByContainsResponse>
    {
        public string Content { get; set; }
        public long UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}
