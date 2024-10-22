using KSozluk.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.Application.Features.Descriptions.Commands.FavouriteWord
{
    public class FavouriteWordResponse : ResponseBase
    {
        public bool IsFavourite { get; set; }
    }
}
