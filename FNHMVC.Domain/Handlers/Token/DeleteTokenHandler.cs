using FNHMVC.CommandProcessor.Command;
using FNHMVC.Domain.Commands;
using FNHMVC.Data.Repositories;
using FNHMVC.Data.Infrastructure;

namespace FNHMVC.Domain.Handlers
{
    public class DeleteTokenHandler : ICommandHandler<DeleteTokenCommand>
    {
        private readonly ITokenRepository tokenRepository;
        private readonly IUnitOfWork unitOfWork;

        public DeleteTokenHandler(ITokenRepository tokenRepository, IUnitOfWork unitOfWork)
        {
            this.tokenRepository = tokenRepository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(DeleteTokenCommand command)
        {
            var token = tokenRepository.GetById(command.TokenId);
            tokenRepository.Delete(token);
            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }
}
