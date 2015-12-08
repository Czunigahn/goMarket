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
    public class CreateOrUpdateUserHandler : ICommandHandler<CreateOrUpdateUserCommand>
    {
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateOrUpdateUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(CreateOrUpdateUserCommand command)
        {
            var user = new Model.User()
            {
                About = command.About,
                Age = command.Age,
                Country = command.Country,
                DateCreated = command.DateCreated,
                Email = command.Email,
                FirstName = command.FirstName,
                Genre = command.Genre,
                LastLoginTime = command.LastLoginTime,
                LastName = command.LastName,
                Picture = command.Picture,
                UserId = command.UserId,
                PaypalAccount = command.PaypalAccount,
                PasswordHash = command.PasswordHash,
                Rate = command.Rate,
                Activated = command.Activated,
                RoleId = command.RoleId
            };

            if (user.UserId == 0)
                userRepository.Add(user);
            else
                userRepository.Update(user);

            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }
}