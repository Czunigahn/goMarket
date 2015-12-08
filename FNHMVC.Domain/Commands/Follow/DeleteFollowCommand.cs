using FNHMVC.CommandProcessor.Command;

namespace FNHMVC.Domain.Commands
{
    public class DeleteFollowCommand : ICommand
    {
        public long FollowId { get; set; }
    }
}
