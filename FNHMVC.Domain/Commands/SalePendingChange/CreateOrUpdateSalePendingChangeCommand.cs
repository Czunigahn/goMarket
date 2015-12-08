using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Model;

namespace FNHMVC.Domain.Commands
{
    public class CreateOrUpdateSalePendingChangeCommand : ICommand
    {
        public  long SalePendingChangeId { get; set; }
        public  string Title { get; set; }
        public  string Description { get; set; }
        public  decimal Cost { get; set; }
        public  int Quantity { get; set; }
        public  string Picture { get; set; }
        public  string YouTubeLink { get; set; }
        public  DateTime Created { get; set; }
        public  DateTime Modified { get; set; }
        public  Category Category { get; set; }
        public  bool Activated { get; set; }
        public  int ReasonId { get; set; }
        public  string ReasonDescription { get; set; }
        public  Sale Sale { get; set; }

        public bool TookItHome { get; set; }
        public string Latitude { get; set; }
        public string Altitude { get; set; }

        public virtual bool CommitAfterAccept { get; set; }

        public CreateOrUpdateSalePendingChangeCommand() { }

        public CreateOrUpdateSalePendingChangeCommand(FNHMVC.Model.SalePendingChange salePending, bool CommitAfterAccept)
        {
            Title = salePending.Title;
            Description = salePending.Description;
            Cost = salePending.Cost;
            Quantity = salePending.Quantity;
            Picture = salePending.Picture;
            YouTubeLink = salePending.YouTubeLink;
            Created = salePending.Created;
            Modified = salePending.Modified;
            Category = salePending.Category;
            Activated = salePending.Activated;
            this.ReasonId = salePending.ReasonId;
            this.ReasonDescription = salePending.ReasonDescription;
            this.CommitAfterAccept = CommitAfterAccept;
            Sale = salePending.Sale;
            SalePendingChangeId = salePending.SalePendingChangeId;
            Altitude = salePending.Altitude;
            Latitude = salePending.Latitude;
            TookItHome = salePending.TookItHome;


        }

        public CreateOrUpdateSalePendingChangeCommand(Sale sale, bool CommitAfterAccept)
        {
            Title = sale.Title;
            Description = sale.Description;
            Cost = sale.Cost;
            Quantity = sale.Quantity;
            Picture = sale.Picture;
            YouTubeLink = sale.YouTubeLink;
            Created = sale.Created;
            Modified = sale.Modified;
            Category = sale.Category;
            Activated = sale.Activated;
            this.ReasonId = -1;
            this.ReasonDescription = "";
            Sale = sale;
            Altitude = sale.Altitude;
            Latitude = sale.Latitude;
            TookItHome = sale.TookItHome;
            this.CommitAfterAccept = CommitAfterAccept;

        }
    }
}
