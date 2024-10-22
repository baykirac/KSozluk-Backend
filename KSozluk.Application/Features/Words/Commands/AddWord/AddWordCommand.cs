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
//"b5925d7b-b496-41c4-91b2-0df9fc481b4a"  "bengisu"   "tugrull.bengisu@gmail.com" "123"   2   "nzne4SXTyWCFJ3QRukp9gQ=="  "2024-10-17 14:12:41.752982"
//    "0695c7d2-6338-4159-9ecf-662292cd499f"  "abc"   "abc@mail.com"  "123"   1   "EowiBeNL4HLF+4t/q7ZxWA=="  "2024-10-07 18:57:44.129301"