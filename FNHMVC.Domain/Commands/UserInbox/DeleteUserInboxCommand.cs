using FNHMVC.CommandProcessor.Command;

namespace FNHMVC.Domain.Commands
{
    public class DeleteUserInboxCommand:ICommand
    {
        public long UserInboxId { get; set; }
    }
}
