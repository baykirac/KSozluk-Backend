using KSozluk.Application.Common;
using KSozluk.Domain;

namespace KSozluk.Application.Features.Words.Commands.AddWords
{
    public class AddWordsResponse : ResponseBase
    {
        public bool WordExists { get; set; }
        public Word AddedWord { get; set; }
    }
}
