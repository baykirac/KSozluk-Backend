using KSozluk.Application.Common;
using KSozluk.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.Application.Features.Descriptions.Commands.WeeklyLiked
{
    public class WeeklyLikedResponse : ResponseBase
    {
        public List<ResponseTopWordListDto> responseTopWordListDtos {  get; set; }
    }
}
