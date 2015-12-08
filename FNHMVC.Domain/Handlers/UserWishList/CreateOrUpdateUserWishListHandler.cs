using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Data.Infrastructure;
using FNHMVC.Data.Repositories;
using FNHMVC.Domain.Commands;


namespace FNHMVC.Domain.Handlers
{

    public class CreateOrUpdateUserWishListHandler : ICommandHandler<CreateOrUpdateUserWishListCommand>
    {
        private readonly IUserWishListRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public CreateOrUpdateUserWishListHandler(IUserWishListRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(CreateOrUpdateUserWishListCommand command)
        {
            var wishlist = new FNHMVC.Model.UserWishList()
            {
                UserWishListId = command.UserWishListId,
                DateCreate = command.DateCreate,
                Activated = command.Activated,
                Name = command.Name,
                Description = command.Description,
                User = command.User,
                Sale = command.Sale,
            };

            if (wishlist.UserWishListId == 0)
                repository.Add(wishlist);
            else
                repository.Update(wishlist);

            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }
}
