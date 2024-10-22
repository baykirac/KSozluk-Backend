using KSozluk.Application.Common;
using KSozluk.Domain;

namespace KSozluk.Application.Features.Words.Commands.AddWords
{
    public class AddWordsCommand : CommandBase<AddWordsResponse>
    {
        public string WordContent { get; set; }
    }
}
