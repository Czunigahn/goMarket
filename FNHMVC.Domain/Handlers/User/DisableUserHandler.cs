using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Data.Infrastructure;
using FNHMVC.Data.Repositories;
using FNHMVC.Domain.Commands.User;

namespace FNHMVC.Domain.Handlers.User
{
    public class DisableUserHandler : ICommandHandler<DisableUserCommand>
    {
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;

        public DisableUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(DisableUserCommand command)
        {
            var record = userRepository.GetById(command.UserId);
            record.Activated = command.Active;
            record.Locked = command.Locked;

            userRepository.Update(record);

            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }
}
