using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin
{
    class UserCredit
    {
        public string Id { get; set; }

        public int UserId { get; set; }

        public int shares { get; set; }

        public int CoinsMined { get; set; }

        public int WalletBalance { get; set; }

        public int TradeCoins { get; set; }
    }
}
