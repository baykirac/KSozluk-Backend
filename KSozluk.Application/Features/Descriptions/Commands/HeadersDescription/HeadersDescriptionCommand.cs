using KSozluk.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KSozluk.Application.Features.Descriptions.Commands.HeadersDescription
{
    public class HeadersDescriptionCommand : CommandBase<HeadersDescriptionResponse>
    {
        public string WordContent { get; set; } 
        public long UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}
