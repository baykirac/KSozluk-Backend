using KSozluk.Application.Common;

namespace KSozluk.Application.Features.Words.Commands.GetAllWords
{
    public class GetAllWordsCommand : CommandBase<GetAllWordsResponse>
    {
        public long UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}
