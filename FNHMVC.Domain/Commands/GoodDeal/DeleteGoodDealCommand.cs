using FNHMVC.CommandProcessor.Command;

namespace FNHMVC.Domain.Commands
{
    public class DeleteGoodDealCommand : ICommand
    {
        public long GoodDealId { get; set; }
    }
}
