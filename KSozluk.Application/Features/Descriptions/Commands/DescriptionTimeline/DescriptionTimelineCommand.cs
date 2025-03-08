using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KSozluk.Application.Common;

namespace KSozluk.Application.Features.Descriptions.Commands.DescriptionTimeline
{
    public class DescriptionTimelineCommand : CommandBase<DescriptionTimelineResponse>
    {
        public long UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}
