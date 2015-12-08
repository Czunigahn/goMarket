using FNHMVC.CommandProcessor.Command;
using FNHMVC.Domain.Commands;
using FNHMVC.Data.Repositories;
using FNHMVC.Data.Infrastructure;

namespace FNHMVC.Domain.Handlers
{
    public class DeleteFollowHandler : ICommandHandler<DeleteFollowCommand>
    {
        private readonly IFollowRepository followRepository;
        private readonly IUnitOfWork unitOfWork;

        public DeleteFollowHandler(IFollowRepository followRepository, IUnitOfWork unitOfWork)
        {
            this.followRepository = followRepository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(DeleteFollowCommand command)
        {
            var follow = followRepository.GetById(command.FollowId);
            followRepository.Delete(follow);
            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }
}
