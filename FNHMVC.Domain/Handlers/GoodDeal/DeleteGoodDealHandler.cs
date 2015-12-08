using FNHMVC.CommandProcessor.Command;
using FNHMVC.Domain.Commands;
using FNHMVC.Data.Repositories;
using FNHMVC.Data.Infrastructure;

namespace FNHMVC.Domain.Handlers
{
    public class DeleteGoodDealHandler : ICommandHandler<DeleteGoodDealCommand>
    {
        private readonly IGoodDealRepository goodDealRepository;
        private readonly IUnitOfWork unitOfWork;

        public DeleteGoodDealHandler(IGoodDealRepository goodDealRepository, IUnitOfWork unitOfWork)
        {
            this.goodDealRepository = goodDealRepository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(DeleteGoodDealCommand command)
        {
            var goodDeal = goodDealRepository.GetById(command.GoodDealId);
            goodDealRepository.Delete(goodDeal);
            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }
}
