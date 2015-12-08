using FNHMVC.CommandProcessor.Command;

namespace FNHMVC.Domain.Commands
{
    public class DeleteCuponCommand : ICommand
    {
        public long CuponId { get; set; }
    }
}
