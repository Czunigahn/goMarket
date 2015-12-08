using FNHMVC.CommandProcessor.Command;

namespace FNHMVC.Domain.Commands
{
    public class DeleteDenounceCommand : ICommand
    {
        public long DenounceId { get; set; }
    }
}