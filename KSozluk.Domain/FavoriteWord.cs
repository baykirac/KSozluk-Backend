﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSozluk.Domain
{
    public class FavoriteWord
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
       
        public Guid WordId { get; set; }

    }
}
