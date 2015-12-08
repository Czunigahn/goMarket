
using FNHMVC.Model;
using FNHMVC.CommandProcessor.Command;

namespace FNHMVC.Domain.Commands
{
    public class CreateOrUpdateSaleImagesCommand : ICommand
    {
        public long SaleImagesId { get; set; }
        public string Url { get; set; }
        public bool Activated { get; set; }
        public int Type { get; set; }
        public Model.Sale Sale { get; set; }
        public virtual Model.SalePendingChange SalePendingChange { get; set; }

        public bool CommitAfterAccept { get; set; }

        public CreateOrUpdateSaleImagesCommand() { }

        public CreateOrUpdateSaleImagesCommand(FNHMVC.Model.SaleImages sale, bool commitAfterAccept)
        {
            this.SaleImagesId = sale.SaleImagesId;
            this.Url = sale.Url;
            this.Sale = sale.Sale;
            this.SalePendingChange = sale.SalePendingChange;
            this.Activated = sale.Activated;
            this.CommitAfterAccept = commitAfterAccept;
            this.Type = sale.Type;
        }
    }
}
