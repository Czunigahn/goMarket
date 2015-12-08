using FNHMVC.CommandProcessor.Command;
using FNHMVC.Data.Infrastructure;
using FNHMVC.Data.Repositories;
using FNHMVC.Domain.Commands;
using FNHMVC.Model;

namespace FNHMVC.Domain.Handlers
{
    public class CreateOrUpdateDenounceHandler : ICommandHandler<CreateOrUpdateDenounceCommand>
    {
        private readonly IDenounceRepository denounceRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateOrUpdateDenounceHandler(IDenounceRepository denounceRepository, IUnitOfWork unitOfWork)
        {
            this.denounceRepository = denounceRepository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(CreateOrUpdateDenounceCommand command)
        {
            var denounce = new Denounce
            {
                DenounceId = command.DenounceId,
                UserDenouncing = command.UserDenouncing,
                UserToDenounce = command.UserToDenounce,
                SaleToDenounce = command.SaleToDenounce,
                Reason = command.Reason,
                Comment = command.Comment,
                Created = command.Created,
                TookAction = command.TookAction
            };

            if (denounce.DenounceId == 0)
                denounceRepository.Add(denounce);
            else
                denounceRepository.Update(denounce);

            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }
}