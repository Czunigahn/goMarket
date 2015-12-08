using FNHMVC.CommandProcessor.Command;

namespace FNHMVC.Domain.Commands
{
    public class DeleteSaleCommand : ICommand
    {
        public long SaleId { get; set; }
        public bool Activated { get; set; }
        public bool PendingChange { get; set; }
        public bool ActiveForSales { get; set; }

        public bool Spotlight { get; set; }
        public bool SpotlightApprove { get; set; }

        public DeleteSaleCommand(long SaleId, bool Activated, bool ActiveForSales, bool PendingChange, bool Spotlight = false, bool SpotlightApprove = false)
        {
            this.SaleId = SaleId;
            this.ActiveForSales = ActiveForSales;
            this.Activated = Activated;
            this.PendingChange = PendingChange;
            this.Spotlight = Spotlight;
            this.SpotlightApprove = SpotlightApprove;
        }
    }


}
