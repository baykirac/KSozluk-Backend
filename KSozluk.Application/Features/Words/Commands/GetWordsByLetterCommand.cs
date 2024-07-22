using KSozluk.Application.Common;

namespace KSozluk.Application.Features.Words.Commands
{
    public class GetWordsByLetterCommand : CommandBase<GetWordsByLetterResponse>
    {
        public char Letter { get; set; }
    }
}
