using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.Domain.DTOs
{
    public class ResponseFavouriteWordsDto
    {
        public Guid WordId { get; set; }
        public Guid UserId { get; set; }
    }

    public class ResponseFavouriteWordContentDto
    {
        public string WordContent { get; set; }
    }
}
