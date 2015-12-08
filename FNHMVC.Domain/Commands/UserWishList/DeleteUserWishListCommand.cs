using FNHMVC.CommandProcessor.Command;

namespace FNHMVC.Domain.Commands
{
    public class DeleteUserWishListCommand
    {
        public long UserWishListId { get; set; }
    }
}
