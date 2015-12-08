using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Data.Infrastructure;
using FNHMVC.Data.Repositories;
using FNHMVC.Domain.Commands;
using FNHMVC.Domain.Commands.SalePendingChange;

namespace FNHMVC.Domain.Handlers
{

    public class CreateOrUpdateSalePendingChangeHandler : ICommandHandler<CreateOrUpdateSalePendingChangeCommand>
    {
        private readonly ISalePendingChangeRepository salePendingChangeRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateOrUpdateSalePendingChangeHandler(ISalePendingChangeRepository salePendingChangeRepository, IUnitOfWork unitOfWork)
        {
            this.salePendingChangeRepository = salePendingChangeRepository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(CreateOrUpdateSalePendingChangeCommand command)
        {
            var salePendingChange = new FNHMVC.Model.SalePendingChange
            {
                SalePendingChangeId = command.SalePendingChangeId,
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
                ReasonId = command.ReasonId,
                ReasonDescription = command.ReasonDescription,
                Sale = command.Sale,
                Altitude = command.Altitude,
                Latitude = command.Latitude,
                TookItHome = command.TookItHome

            };

            if (salePendingChange.SalePendingChangeId == 0)
                salePendingChangeRepository.Add(salePendingChange);
            else
                salePendingChangeRepository.Update(salePendingChange);

            if (command.CommitAfterAccept)
                unitOfWork.Commit();

            command.SalePendingChangeId = salePendingChange.SalePendingChangeId;

            return new CommandResult(true);
        }
    }
}
