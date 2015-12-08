using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNHMVC.Model
{
    public class GoodDeal
    {
        public virtual long GoodDealId { get; set; }
        public virtual User User { get; set; }
        public virtual Sale Sale { get; set; }
    }
}
