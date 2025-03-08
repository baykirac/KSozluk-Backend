using KSozluk.Application.Common;
using KSozluk.Domain;

namespace KSozluk.Application.Features.Words.Commands.AddWord
{
    public class AddWordCommand : CommandBase<AddWordResponse>
    {
        public string WordContent { get; set; }
        public List<string> Description {  get; set; }
        public long UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}