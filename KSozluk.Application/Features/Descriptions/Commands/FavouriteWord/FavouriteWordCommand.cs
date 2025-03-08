using KSozluk.Application.Common;
//using KSozluk.Application.Features.Descriptions.Commands.LikeDescription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.Application.Features.Descriptions.Commands.FavouriteWord
{
    public class FavouriteWordCommand : CommandBase<FavouriteWordResponse>
    {
        public Guid WordId { get; set; }
        public long UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}
