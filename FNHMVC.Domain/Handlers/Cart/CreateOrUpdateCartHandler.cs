using FNHMVC.Data.Repositories;
using FNHMVC.Data.Infrastructure;
using FNHMVC.Domain.Commands;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Model;

namespace FNHMVC.Domain.Handlers
{
    public class CreateOrUpdateCartHandler : ICommandHandler<CreateOrUpdateCartCommand>
    {
        private readonly ICartRepository cartRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateOrUpdateCartHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork)
        {
            this.cartRepository = cartRepository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(CreateOrUpdateCartCommand command)
        {
            var cart = new Cart()
            {
                CartId = command.CartId,
                Sale = command.Sale,
                User = command.User,
                Quantity = command.Quantity,
                Created = command.Created,
                Cupon = command.Cupon
            };

            if (cart.CartId == 0)
                cartRepository.Add(cart);
            else
                cartRepository.Update(cart);

            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }
}
