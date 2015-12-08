
using FNHMVC.CommandProcessor.Command;

namespace FNHMVC.Domain.Commands
{
    public class DeleteSaleImagesCommand:ICommand
    {
        public virtual long SaleImagesId { get; set; }

        public DeleteSaleImagesCommand(long SaleImagesId)
        {
            this.SaleImagesId = SaleImagesId;
        }
    }
}
