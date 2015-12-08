using FNHMVC.CommandProcessor.Command;

namespace FNHMVC.Domain.Commands
{
    public class DeleteTokenCommand : ICommand
    {
        public int TokenId { get; set; }
    }
}
