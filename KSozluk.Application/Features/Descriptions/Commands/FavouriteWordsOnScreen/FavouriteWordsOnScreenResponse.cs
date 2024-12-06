using KSozluk.Application.Common;
using KSozluk.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.Application.Features.Descriptions.Commands.FavouriteWordsOnScreen
{
    public class FavouriteWordsOnScreenResponse : ResponseBase
    {
        public List<ResponseFavouriteWordContentDto> responseFavouriteWordsDtos { get; set; }
    }
}
