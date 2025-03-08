using KSozluk.Application.Common;
using KSozluk.Application.Features.Words.Commands.GetAllWords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.Application.Features.Descriptions.Commands.WeeklyLiked
{
    public class WeeklyLikedCommand  : CommandBase<WeeklyLikedResponse>
    {
        public long UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}
