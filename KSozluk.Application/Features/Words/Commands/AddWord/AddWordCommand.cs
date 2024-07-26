using KSozluk.Application.Common;
using KSozluk.Domain;

namespace KSozluk.Application.Features.Words.Commands.AddWord
{
    public class AddWordCommand : CommandBase<AddWordResponse>
    {
        public string WordContent { get; set; }
        public string Description {  get; set; }
    }
}
