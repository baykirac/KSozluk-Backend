using KSozluk.Application.Common;

namespace KSozluk.Application.Features.Words.Commands.GetWordsByLetter
{
    public class GetWordsByLetterCommand : CommandBase<GetWordsByLetterResponse>
    {
        public char Letter { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}
