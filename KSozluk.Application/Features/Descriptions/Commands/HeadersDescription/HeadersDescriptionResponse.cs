using KSozluk.Application.Common;
using KSozluk.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KSozluk.Application.Features.Descriptions.Commands.HeadersDescription
{
    public class HeadersDescriptionResponse : ResponseBase
    {
            public Guid WordId { get; set; }
            public List<DescriptionHeaderNameDto> Descriptions { get; set; }

            public HeadersDescriptionResponse()
            {
                Descriptions = new List<DescriptionHeaderNameDto>();
            }

    }
}
