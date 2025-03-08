using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.WebAPI.DTOs
{
    public class ResponseFavouriteWordsDto
    {
        public Guid WordId { get; set; }
        public long? UserId { get; set; }
    }

    public class ResponseFavouriteWordContentDto
    {
        public string WordContent { get; set; }
        public Guid Id { get; set; }
    }
}
