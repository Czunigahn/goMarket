
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Data.Infrastructure;
using FNHMVC.Data.Repositories;
using FNHMVC.Domain.Commands;


namespace FNHMVC.Domain.Handlers
{


    public class DeleteUserWishListHandler : ICommandHandler<DeleteUserWishListCommand>
    {
        private readonly IUserWishListRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public DeleteUserWishListHandler(IUserWishListRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(DeleteUserWishListCommand command)
        {
            var record = repository.GetById(command.UserWishListId);

            record.Activated = false;

            repository.Update(record);
            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }
}
