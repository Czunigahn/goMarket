using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Data.Infrastructure;
using FNHMVC.Data.Repositories;
using FNHMVC.Domain.Commands;
using FNHMVC.Model;

namespace FNHMVC.Domain.Handlers
{

    public class CreateOrUpdateSaleImagesHandler : ICommandHandler<CreateOrUpdateSaleImagesCommand>
    {
        private readonly ISaleImagesRepository saleImagesRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateOrUpdateSaleImagesHandler(ISaleImagesRepository saleRepository, IUnitOfWork unitOfWork)
        {
            this.saleImagesRepository = saleRepository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(CreateOrUpdateSaleImagesCommand command)
        {
            var sale = new FNHMVC.Model.SaleImages
            {
                Activated = command.Activated,
                Sale = command.Sale,
                SaleImagesId = command.SaleImagesId,
                Url = command.Url,
                Type = command.Type,
                SalePendingChange = command.SalePendingChange

            };

            if (sale.SaleImagesId == 0)
                saleImagesRepository.Add(sale);
            else
                saleImagesRepository.Update(sale);

            if (command.CommitAfterAccept)
                unitOfWork.Commit();

            command.SaleImagesId = sale.SaleImagesId;

            return new CommandResult(true);
        }
    }
}
