using KSozluk.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.Application.Features.Words.Commands.UpdateWordById
{
    public class UpdateWordByIdCommand : CommandBase<UpdateWordByIdResponse>
    {
        public Guid WordId { get; set; }
        public string WordContent { get; set; }
        public long UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}
