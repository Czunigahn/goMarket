using FNHMVC.CommandProcessor.Command;

namespace FNHMVC.Domain.Commands
{
    public class DeleteCartCommand : ICommand
    {
        public long CartId { get; set; }
    }
}
