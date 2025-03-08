using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.WebAPI.DTOs
{
    public class ResponseTopWordListDto
    {
        public Guid WordId { get; set; }
        public string Word {  get; set; }
        public int Count { get; set; }
    }
}
