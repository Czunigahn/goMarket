using FNHMVC.Data.Repositories;
using FNHMVC.Data.Infrastructure;
using FNHMVC.Domain.Commands;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Model;

namespace FNHMVC.Domain.Handlers
{
    public class CreateOrUpdateCuponHandler : ICommandHandler<CreateOrUpdateCuponCommand>
    {
        private readonly ICuponRepository cuponRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateOrUpdateCuponHandler(ICuponRepository cuponRepository, IUnitOfWork unitOfWork)
        {
            this.cuponRepository = cuponRepository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(CreateOrUpdateCuponCommand command)
        {
            var cupon = new Cupon()
            {
                CuponId = command.CuponId,
                CuponName = command.CuponName,
                User = command.User,
                Discount = command.Discount,
                IsActive = command.IsActive,
                Created = command.Created,
                TimesUsed = command.TimesUsed
            };

            if (cupon.CuponId == 0)
                cuponRepository.Add(cupon);
            else
                cuponRepository.Update(cupon);

            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }
}
