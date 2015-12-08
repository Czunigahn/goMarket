using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FNHMVC.Domain.Commands;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Data.Repositories;
using FNHMVC.Data.Infrastructure;

namespace FNHMVC.Domain.Handlers
{
    public class DeleteSaleHandler : ICommandHandler<DeleteSaleCommand>
    {
        private readonly ISaleRepository saleRepository;
        private readonly IUnitOfWork unitOfWork;

        public DeleteSaleHandler(ISaleRepository saleRepository, IUnitOfWork unitOfWork)
        {
            this.saleRepository = saleRepository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(DeleteSaleCommand command)
        {
            var sale = saleRepository.GetById(command.SaleId);
            
            sale.Activated = command.Activated;
            sale.ActiveForSales = command.ActiveForSales;
            sale.PendingChange = command.PendingChange;

            sale.Spotlight = command.Spotlight;
            sale.SpotlightApprove = command.SpotlightApprove;

            saleRepository.Update(sale);
            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }

}
