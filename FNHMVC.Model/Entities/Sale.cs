using System;
using System.Collections.Generic;

namespace FNHMVC.Model
{

    public class Sale
    {
        public virtual long SaleId { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal Cost { get; set; }
        public virtual int Quantity { get; set; }
        public virtual string Picture { get; set; }
        public virtual string YouTubeLink { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual Category Category { get; set; }
        public virtual User User { get; set; }

        public virtual bool Activated { get; set; }
        public virtual bool PendingChange { get; set; }
        public virtual bool ActiveForSales { get; set; }

        public virtual bool TookItHome { get; set; }
        public virtual string Latitude { get; set; }
        public virtual string Altitude { get; set; }

        public virtual bool HasDeal { get; set; }
        public virtual string DescriptionDeal { get; set; }
        public virtual decimal CostDeal { get; set; }
        public virtual DateTime DateFromDeal { get; set; }
        public virtual DateTime DateToDeal { get; set; }

        public virtual bool Spotlight { get; set; }
        public virtual bool SpotlightApprove { get; set; }


        public virtual IList<GoodDeal> GoodDeals { get; set; }
        public virtual IList<UserReviews> UserReviews { get; set; }
        public virtual IList<SalePendingChange> SalePendingChange { get; set; }
        public virtual IList<Model.SaleImages> SaleImages { get; set; }
        public virtual IList<Cart> Cart { get; set; }



    }
}
