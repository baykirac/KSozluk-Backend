using KSozluk.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.Application.Features.Words.Commands.LikeWord
{
    public class LikeWordCommand : CommandBase<LikeWordResponse>
    {
        public Guid WordId { get; set; }
    }
}
