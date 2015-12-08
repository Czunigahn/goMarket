using FNHMVC.Data.Repositories;
using FNHMVC.Data.Infrastructure;
using FNHMVC.Domain.Commands;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Model;

namespace FNHMVC.Domain.Handlers
{
    public class CreateFollowHandler : ICommandHandler<CreateFollowCommand>
    {
        private readonly IFollowRepository followRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateFollowHandler(IFollowRepository followRepository, IUnitOfWork unitOfWork)
        {
            this.followRepository = followRepository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(CreateFollowCommand command)
        {
            var follow = new Follow
            {
                FollowId = command.FollowId,
                Follower = command.Follower,
                User = command.User
                
            };

            if (follow.FollowId == 0)
                followRepository.Add(follow);
            else
                followRepository.Update(follow);

            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }
}
