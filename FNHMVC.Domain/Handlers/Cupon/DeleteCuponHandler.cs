using FNHMVC.CommandProcessor.Command;
using FNHMVC.Domain.Commands;
using FNHMVC.Data.Repositories;
using FNHMVC.Data.Infrastructure;

namespace FNHMVC.Domain.Handlers
{
    public class DeleteCuponHandler : ICommandHandler<DeleteCuponCommand>
    {
        private readonly ICuponRepository cuponRepository;
        private readonly IUnitOfWork unitOfWork;

        public DeleteCuponHandler(ICuponRepository cuponRepository, IUnitOfWork unitOfWork)
        {
            this.cuponRepository = cuponRepository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(DeleteCuponCommand command)
        {
            var cupon = cuponRepository.GetById(command.CuponId);
            cuponRepository.Delete(cupon);
            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }
}
