using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Domain.Commands;
using FNHMVC.Data.Repositories;
using FNHMVC.Data.Infrastructure;
using FNHMVC.Model;

namespace FNHMVC.Domain.Handlers
{
    public class UpdateUserHandler : ICommandHandler<UpdateUserCommand>
    {
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;

        public UpdateUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(UpdateUserCommand command)
        {
            var user = userRepository.GetById(command.UserId);
            user.UserId = command.UserId;
            user.Email = command.Email;
            user.FirstName = command.FirstName;
            user.LastName = command.LastName;
            user.Activated = command.Activated;
            user.Age = command.Age;
            user.Country = command.Country;
            user.LastLoginTime = command.LastLoginTime;
            user.PaypalAccount = command.PaypalAccount; //editado
            userRepository.Update(user);
            unitOfWork.Commit();
            return new CommandResult(true);

        }

    }
}
