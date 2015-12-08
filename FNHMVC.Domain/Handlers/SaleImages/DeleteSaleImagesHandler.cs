using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Data.Infrastructure;
using FNHMVC.Data.Repositories;
using FNHMVC.Domain.Commands;

namespace FNHMVC.Domain.Handlers.SaleImages
{

    public class DeleteSaleImagesHandler : ICommandHandler<DeleteSaleImagesCommand>
    {
        private readonly ISaleImagesRepository saleImagesRepository;
        private readonly IUnitOfWork unitOfWork;

        public DeleteSaleImagesHandler(ISaleImagesRepository saleImagesRepository, IUnitOfWork unitOfWork)
        {
            this.saleImagesRepository = saleImagesRepository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(DeleteSaleImagesCommand command)
        {
            var saleImage = saleImagesRepository.GetById(command.SaleImagesId);

            saleImagesRepository.Delete(saleImage);
            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }
}
