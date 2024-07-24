using KSozluk.Application.Common;

namespace KSozluk.Application.Features.Words.Commands
{
    public class GetWordsByLetterCommand : CommandBase<GetWordsByLetterResponse>
    {
        public char Letter { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }
}
