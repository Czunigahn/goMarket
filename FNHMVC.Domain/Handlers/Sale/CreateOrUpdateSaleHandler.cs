using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Domain.Commands;
using FNHMVC.Data.Repositories;
using FNHMVC.Data.Infrastructure;
using FNHMVC.Model;

namespace FNHMVC.Domain.Handlers
{


    public class CreateOrUpdateSaleHandler : ICommandHandler<CreateOrUpdateSaleCommand>
    {
        private readonly ISaleRepository saleRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateOrUpdateSaleHandler(ISaleRepository saleRepository, IUnitOfWork unitOfWork)
        {
            this.saleRepository = saleRepository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(CreateOrUpdateSaleCommand command)
        {
            var sale = new Sale
            {
                SaleId = command.SaleId,
                Activated = command.Activated,
                Category = command.Category,
                Cost = command.Cost,
                Created = command.Created,
                Description = command.Description,
                Modified = command.Modified,
                Picture = command.Picture,
                Quantity = command.Quantity,
                Title = command.Title,
                YouTubeLink = command.YouTubeLink,
                User = command.User,
                ActiveForSales = command.ActiveForSales,
                PendingChange = command.PendingChange,
                Altitude = command.Altitude,
                Latitude = command.Latitude,
                TookItHome = command.TookItHome,
            
                HasDeal = command.HasDeal,
                DescriptionDeal = command.DescriptionDeal,
                CostDeal = command.CostDeal,
                DateFromDeal = command.DateFromDeal,

                DateToDeal = command.DateToDeal,
                Spotlight = command.Spotlight,
                SpotlightApprove = command.SpotlightApprove

            };

            if (sale.SaleId == 0)
                saleRepository.Add(sale);
            else
                saleRepository.Update(sale);

            if (command.CommitAfterAccept)
                unitOfWork.Commit();

            command.SaleId = sale.SaleId;

            return new CommandResult(true);
        }
    }
}
