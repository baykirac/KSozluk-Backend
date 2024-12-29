using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KSozluk.Application.Common;
using KSozluk.Domain.DTOs;

namespace KSozluk.Application.Features.Descriptions.Commands.DescriptionTimeline
{
    public class DescriptionTimelineResponse : ResponseBase
    {
        public List<DescriptionTimelineDto> DescriptionTimelineDtos { get; set; }
    }
}
