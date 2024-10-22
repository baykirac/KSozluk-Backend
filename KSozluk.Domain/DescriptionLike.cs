using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.Domain
{
    public class DescriptionLike
    {
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
        public Guid DescriptionId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
