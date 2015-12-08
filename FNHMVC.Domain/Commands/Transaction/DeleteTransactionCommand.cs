using FNHMVC.CommandProcessor.Command;

namespace FNHMVC.Domain.Commands
{
    public class DeleteTransactionCommand : ICommand
    {
        public int TransactionId { get; set; }
    }
}