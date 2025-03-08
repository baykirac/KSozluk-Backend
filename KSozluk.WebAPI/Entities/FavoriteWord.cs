using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.WebAPI.Entities
{
    public class FavoriteWord
    {
        public Guid Id { get; set; }
        public long? UserId { get; set; }     
        public Guid WordId { get; set; }

    }
}
