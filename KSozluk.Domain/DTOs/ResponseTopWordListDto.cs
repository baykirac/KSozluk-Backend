using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.Domain.DTOs
{
    public class ResponseTopWordListDto
    {
        public Guid WordId { get; set; }
        public string Word {  get; set; }
        public int Count { get; set; }
    }
}
