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

    public class CreateOrUpdateUserInboxHandler : ICommandHandler<CreateOrUpdateUserInboxCommand>
    {
        private readonly IUserInboxRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public CreateOrUpdateUserInboxHandler(IUserInboxRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(CreateOrUpdateUserInboxCommand command)
        {
            var inbox = new FNHMVC.Model.UserInbox
            {
                UserInboxId = command.UserInboxId,
                DateCreate = command.DateCreate,
                DateRead = command.DateRead,
                WasRead = command.WasRead,
                Activated = command.Activated,
                SentEmail = command.SentEmail,
                Subject = command.Subject,
                Message = command.Message,
                User = command.User,
                Seller = command.Seller,
                LastMessage=command.LastMessage,
            };

            if (inbox.UserInboxId == 0)
                repository.Add(inbox);
            else
                repository.Update(inbox);

            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }
}
