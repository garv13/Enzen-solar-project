using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    class Share
    {
        public int ShareId { get; set; }
        public int RoofId { get; set; }
        public int UserId { get; set; }
        public string Id { get; set; }
        public bool IsTradeable { get; set; }
        public double Price { get; set; }

    }
}
