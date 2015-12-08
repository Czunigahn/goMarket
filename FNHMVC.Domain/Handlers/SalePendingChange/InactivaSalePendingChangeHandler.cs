using System.Linq;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Data.Infrastructure;
using FNHMVC.Data.Repositories;
using FNHMVC.Domain.Commands.SalePendingChange;

namespace FNHMVC.Domain.Handlers
{
    public class InactivaSalePendingChangeHandler:ICommandHandler<InactivaSalePendingChangeCommand>
    {
        private readonly ISalePendingChangeRepository salePendingChangeRepository;
        private readonly IUnitOfWork unitOfWork;

        public InactivaSalePendingChangeHandler(ISalePendingChangeRepository salePendingChangeRepository, IUnitOfWork unitOfWork)
        {
            this.salePendingChangeRepository = salePendingChangeRepository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(InactivaSalePendingChangeCommand command)
        {            
            for (int i=0;i< command.changes.Count();i++)
            {
                command.changes[i].Activated=false ;
                salePendingChangeRepository.Update(command.changes[i]);
            }
            
            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }
}