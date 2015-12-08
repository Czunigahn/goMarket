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
    public class CreateOrUpdateUserReviewsHandler : ICommandHandler<CreateOrUpdateUserReviewsCommand>
    {
        private readonly IUserReviewsRepository userReviewsRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateOrUpdateUserReviewsHandler(IUserReviewsRepository userReviewsRepository, IUnitOfWork unitOfWork)
        {
            this.userReviewsRepository = userReviewsRepository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(CreateOrUpdateUserReviewsCommand command)
        {
            var review = new Model.UserReviews()
            {
                Date = command.Date,
                ReviewId = command.ReviewId,
                User = command.User,
                Value = command.Value,
                Comment = command.Comment,
                Title = command.Title,
                Active = command.Active,
                Sale = command.Sale
            };

            if (review.ReviewId == 0)
                userReviewsRepository.Add(review);
            else
                userReviewsRepository.Update(review);

            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }
}
