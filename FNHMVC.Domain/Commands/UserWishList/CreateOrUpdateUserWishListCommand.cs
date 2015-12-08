using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FNHMVC.CommandProcessor.Command;

namespace FNHMVC.Domain.Commands
{
    public class CreateOrUpdateUserWishListCommand : ICommand
    {
        public virtual long UserWishListId { get; set; }
        public virtual DateTime DateCreate { get; set; }
        public virtual bool Activated { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual Model.User User { get; set; }
        public virtual Model.Sale Sale { get; set; }

        public CreateOrUpdateUserWishListCommand(long UserWishListId, DateTime DateCreate, bool Activated, string Name, string Description, Model.User User, Model.Sale Sale)
        {
            this.UserWishListId = UserWishListId;
            this.DateCreate = DateCreate;
            this.Activated = Activated;
            this.Name = Name;
            this.Description = Description;
            this.User = User;
            this.Sale = Sale;
        }

        public CreateOrUpdateUserWishListCommand(FNHMVC.Model.UserWishList WishList)
        {
            this.UserWishListId = WishList.UserWishListId;
            this.DateCreate = WishList.DateCreate;
            this.Activated = WishList.Activated;
            this.Name = WishList.Name;
            this.Description = WishList.Description;
            this.User = WishList.User;
            this.Sale = WishList.Sale;
        }
    }
}
