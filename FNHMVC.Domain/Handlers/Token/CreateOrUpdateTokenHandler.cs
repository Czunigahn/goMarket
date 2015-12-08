using FNHMVC.Data.Repositories;
using FNHMVC.Data.Infrastructure;
using FNHMVC.Domain.Commands;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Model;

namespace FNHMVC.Domain.Handlers
{
    public class CreateOrUpdateTokenHandler : ICommandHandler<CreateOrUpdateTokenCommand>
    {
        private readonly ITokenRepository tokenRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateOrUpdateTokenHandler(ITokenRepository tokenRepository, IUnitOfWork unitOfWork)
        {
            this.tokenRepository = tokenRepository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(CreateOrUpdateTokenCommand command)
        {
            var token = new Token
            {
                TokenId = command.TokenId,
                ConfirmationToken = command.ConfirmationToken,
                UserId = command.UserId,
                Activated = command.Activated,
                Action = command.Action
            };

            if (token.TokenId == 0)
                tokenRepository.Add(token);
            else
                tokenRepository.Update(token);
            unitOfWork.Commit();
            return new CommandResult(true);
        }

    }
}
