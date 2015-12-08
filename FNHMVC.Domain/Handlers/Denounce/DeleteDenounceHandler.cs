using FNHMVC.CommandProcessor.Command;
using FNHMVC.Data.Infrastructure;
using FNHMVC.Data.Repositories;
using FNHMVC.Domain.Commands;

namespace FNHMVC.Domain.Handlers
{
    public class DeleteDenounceHandler : ICommandHandler<DeleteDenounceCommand>
    {
        private readonly IDenounceRepository denounceRepository;
        private readonly IUnitOfWork unitOfWork;

        public DeleteDenounceHandler(IDenounceRepository denounceRepository, IUnitOfWork unitOfWork)
        {
            this.denounceRepository = denounceRepository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(DeleteDenounceCommand command)
        {
            var denounce = denounceRepository.GetById(command.DenounceId);
            denounceRepository.Delete(denounce);
            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }
}