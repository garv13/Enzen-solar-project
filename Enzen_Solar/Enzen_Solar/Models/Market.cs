﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enzen_Solar.Models
{
    public class Market
    {
        //TODO : Add mechanism for market

        public string Id { get; set; }

        public int UserId { get; set; }

        public string CoinQuantity { get; set; }

        public string PerCoinRate { get; set; }

    }
}
