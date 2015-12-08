
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Data.Infrastructure;
using FNHMVC.Data.Repositories;
using FNHMVC.Domain.Commands;


namespace FNHMVC.Domain.Handlers
{


    public class DeleteUserAddressHandler : ICommandHandler<DeleteUserAddressCommand>
    {
        private readonly IUserAddressRepository userAddressRepository;
        private readonly IUnitOfWork unitOfWork;

        public DeleteUserAddressHandler(IUserAddressRepository userAddressRepository, IUnitOfWork unitOfWork)
        {
            this.userAddressRepository = userAddressRepository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(DeleteUserAddressCommand command)
        {
            var address = userAddressRepository.GetById(command.UserAddressId);

            address.Activated = false;

            userAddressRepository.Update(address);
            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }
}
