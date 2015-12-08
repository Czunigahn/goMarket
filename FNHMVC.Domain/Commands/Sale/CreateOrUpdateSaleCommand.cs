using System;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Model;
using System.Collections.Generic;

namespace FNHMVC.Domain.Commands
{
    

    public class CreateOrUpdateSaleCommand : ICommand
    {
        public long SaleId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public int Quantity { get; set; }
        public string Picture { get; set; }
        public string YouTubeLink { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public Category Category { get; set; }
        public FNHMVC.Model.User User { get; set; }
        public bool Activated { get; set; }
        public bool PendingChange { get; set; }
        public bool ActiveForSales { get; set; }
        public bool CommitAfterAccept { get; set; }
        
        public bool TookItHome { get; set; }
        public string Latitude { get; set; }
        public string Altitude { get; set; }


        public virtual bool HasDeal { get; set; }
        public virtual string DescriptionDeal { get; set; }
        public virtual decimal CostDeal { get; set; }
        public virtual DateTime DateFromDeal { get; set; }
        public virtual DateTime DateToDeal { get; set; }

        public virtual bool Spotlight { get; set; }
        public virtual bool SpotlightApprove { get; set; }

        public CreateOrUpdateSaleCommand()
        {
            CommitAfterAccept = true;
        }

        public CreateOrUpdateSaleCommand(Sale sale,bool CommitAfterAccept)
        {
            SaleId = sale.SaleId;
            Title = sale.Title;
            Description = sale.Description;
            Cost = sale.Cost;
            Quantity = sale.Quantity;
            Picture = sale.Picture;
            YouTubeLink = sale.YouTubeLink;
            Created = sale.Created;
            Modified = sale.Modified;
            Category = sale.Category;
            User = sale.User;
            Activated = sale.Activated;
            PendingChange = sale.PendingChange;
            ActiveForSales = sale.ActiveForSales;
            TookItHome = sale.TookItHome;
            Latitude = sale.Latitude;
            Altitude = sale.Altitude;

            HasDeal = sale.HasDeal;
            DescriptionDeal = sale.DescriptionDeal;
            CostDeal = sale.CostDeal;
            DateFromDeal = sale.DateFromDeal;
            DateToDeal = sale.DateToDeal;

            Spotlight = sale.Spotlight;

            this.CommitAfterAccept = CommitAfterAccept;

        }
    }
}
