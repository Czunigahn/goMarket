using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNHMVC.Model
{
    public class SalePendingChange
    {
        public virtual long SalePendingChangeId { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal Cost { get; set; }
        public virtual int Quantity { get; set; }
        public virtual string Picture { get; set; }
        public virtual string YouTubeLink { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual Category Category { get; set; }
        public virtual bool Activated { get; set; }
        public virtual int ReasonId { get; set; }
        public virtual string ReasonDescription { get; set; }
        public virtual Sale Sale { get; set; }

        public virtual bool TookItHome { get; set; }
        public virtual string Latitude { get; set; }
        public virtual string Altitude { get; set; }
        public virtual IList<Model.SaleImages> SaleImages { get; set; }
        
    }
}
