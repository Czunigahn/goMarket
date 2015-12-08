using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Data.Infrastructure;
using FNHMVC.Data.Repositories;
using FNHMVC.Domain.Commands;
using FNHMVC.Domain.Commands.SalePendingChange;

namespace FNHMVC.Domain.Handlers
{
    public class DeleteSalePendingChangeHandler : ICommandHandler<DeleteSalePendingChangeCommand>
    {
        private readonly ISalePendingChangeRepository salePendingChangeRepository;
        private readonly IUnitOfWork unitOfWork;

        public DeleteSalePendingChangeHandler(ISalePendingChangeRepository salePendingChangeRepository, IUnitOfWork unitOfWork)
        {
            this.salePendingChangeRepository = salePendingChangeRepository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(DeleteSalePendingChangeCommand command)
        {
            var salePendingChange = salePendingChangeRepository.GetById(command.SalePendingChangeId);
            salePendingChange.Activated = false;
            salePendingChangeRepository.Update(salePendingChange);
            unitOfWork.Commit();
            
            return new CommandResult(true);
        }
    }
}
