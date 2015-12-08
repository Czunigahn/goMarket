using FNHMVC.CommandProcessor.Command;

namespace FNHMVC.Domain.Commands
{
    public class DeleteUserAddressCommand
    {
        public long UserAddressId { get; set; }
    }
}
