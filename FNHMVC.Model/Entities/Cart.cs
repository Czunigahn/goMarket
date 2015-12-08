using System;
using System.Collections.Generic;

namespace FNHMVC.Model
{
    public class Cart
    {
        public virtual long CartId { get; set; }
        public virtual User User { get; set; }
        public virtual Sale Sale { get; set; }
        public virtual int Quantity { get; set; }
        public virtual Cupon Cupon { get; set; }
        public virtual DateTime Created { get; set; }

    }
}