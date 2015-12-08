
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Data.Infrastructure;
using FNHMVC.Data.Repositories;
using FNHMVC.Domain.Commands;


namespace FNHMVC.Domain.Handlers
{


    public class DeleteUserInboxHandler : ICommandHandler<DeleteUserInboxCommand>
    {
        private readonly IUserInboxRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public DeleteUserInboxHandler(IUserInboxRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(DeleteUserInboxCommand command)
        {
            var record = repository.GetById(command.UserInboxId);

            record.Activated = false;

            repository.Update(record);
            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }
}
