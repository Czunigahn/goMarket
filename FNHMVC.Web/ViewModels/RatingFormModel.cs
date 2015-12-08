using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FNHMVC.Model;

namespace FNHMVC.Web.ViewModels
{
    public class RatingFormModel
    {
        public virtual long ReviewId { get; set; }
        public virtual bool Active { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual int Value { get; set; }
        public virtual string Title { get; set; }
        public virtual string Comment { get; set; }


        public virtual long SaleId { get; set; }
        public virtual string TitleSale { get; set; }

        public virtual int Start5Count { get; set; }
        public virtual int Start4Count { get; set; }
        public virtual int Start3Count { get; set; }
        public virtual int Start2Count { get; set; }
        public virtual int Start1Count { get; set; }

        public virtual decimal Start5Avg { get; set; }
        public virtual decimal Start4Avg { get; set; }
        public virtual decimal Start3Avg { get; set; }
        public virtual decimal Start2Avg { get; set; }
        public virtual decimal Start1Avg { get; set; }

        public virtual decimal SaleCount { get; set; }
        public virtual decimal SaleAvg { get; set; }

        public virtual UserReviews UserReview { get; set; }

        public RatingFormModel()
        {
 
        }

        public RatingFormModel(SaleFormModel sale)
        {
            this.ReviewId = sale.RatingFormModel.ReviewId;
            this.Active = sale.RatingFormModel.Active;
            this.Date = sale.RatingFormModel.Date;
            this.Value = sale.RatingFormModel.Value;
            this.Title = sale.RatingFormModel.Title;
            this.SaleId = sale.RatingFormModel.SaleId;
            this.TitleSale = sale.RatingFormModel.TitleSale;
            this.Start5Count = sale.RatingFormModel.Start5Count;
            this.Start4Count = sale.RatingFormModel.Start4Count;
            this.Start3Count = sale.RatingFormModel.Start3Count;
            this.Start2Count = sale.RatingFormModel.Start2Count;
            this.Start1Count = sale.RatingFormModel.Start1Count;

            this.Start5Avg = sale.RatingFormModel.Start5Avg;
            this.Start4Avg = sale.RatingFormModel.Start4Avg;
            this.Start3Avg = sale.RatingFormModel.Start3Avg;
            this.Start2Avg = sale.RatingFormModel.Start2Avg;
            this.Start1Avg = sale.RatingFormModel.Start1Avg;

            this.SaleCount = sale.RatingFormModel.SaleCount;
            this.SaleAvg = sale.RatingFormModel.SaleAvg;
            this.UserReview = sale.RatingFormModel.UserReview;

        }
    }
}