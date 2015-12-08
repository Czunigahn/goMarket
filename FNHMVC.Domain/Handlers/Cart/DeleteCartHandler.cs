using FNHMVC.CommandProcessor.Command;
using FNHMVC.Domain.Commands;
using FNHMVC.Data.Repositories;
using FNHMVC.Data.Infrastructure;

namespace FNHMVC.Domain.Handlers
{
    public class DeleteCartHandler : ICommandHandler<DeleteCartCommand>
    {
        private readonly ICartRepository cartRepository;
        private readonly IUnitOfWork unitOfWork;

        public DeleteCartHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork)
        {
            this.cartRepository = cartRepository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(DeleteCartCommand command)
        {
            var cart = cartRepository.GetById(command.CartId);
            cartRepository.Delete(cart);
            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }
}
