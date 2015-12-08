using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Data.Infrastructure;
using FNHMVC.Data.Repositories;
using FNHMVC.Domain.Commands;


namespace FNHMVC.Domain.Handlers
{
    public class DeleteUserReviewsHandler : ICommandHandler<DeleteUserReviewsCommand>
    {
        private readonly IUserReviewsRepository userReviewsRepository;
        private readonly IUnitOfWork unitOfWork;

        public DeleteUserReviewsHandler(IUserReviewsRepository userReviewsRepository, IUnitOfWork unitOfWork)
        {
            this.userReviewsRepository = userReviewsRepository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(DeleteUserReviewsCommand command)
        {
            var record = userReviewsRepository.GetById(command.ReviewId); 
            record.Active = false;
            userReviewsRepository.Update(record);
            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }
}
