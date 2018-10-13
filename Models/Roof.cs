using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    class Roof
    {
        public string Id { get; set; }
        public double Potential { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int TotalShares { get; set; }
        public int SharesAvailable { get; set; }
        public int Percentage { get; set; }
        public int RoofId { get; set; }
        public int Investment { get; set; }
        public int Size { get; set; }
        public double CostPerShare { get; set; }
        public int UserId { get; set; }

    }
}
