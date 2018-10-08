using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    class UserCredit
    {
        public string Id { get; set; }

        public int UserId { get; set; }

        public string TotalCoins { get; set; }

        public string CoinsMined { get; set; }

        public string WalletBalance { get; set; }

        public string TradeCoins { get; set; }
    }
}
