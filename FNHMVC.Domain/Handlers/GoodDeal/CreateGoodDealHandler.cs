using FNHMVC.Data.Repositories;
using FNHMVC.Data.Infrastructure;
using FNHMVC.Domain.Commands;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Model;

namespace FNHMVC.Domain.Handlers
{
    public class CreateGoodDealHandler : ICommandHandler<CreateGoodDealCommand>
    {
        private readonly IGoodDealRepository goodDealRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateGoodDealHandler(IGoodDealRepository goodDealRepository, IUnitOfWork unitOfWork)
        {
            this.goodDealRepository = goodDealRepository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(CreateGoodDealCommand command)
        {
            var goodDeal = new GoodDeal()
            {
               GoodDealId = command.GoodDealId,
                Sale = command.Sale,
                User = command.User
                
            };

                goodDealRepository.Add(goodDeal);

            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }
}
