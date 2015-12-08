using System.Collections.Generic;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Domain.Commands;
using FNHMVC.Core.Common;
using FNHMVC.Data.Repositories;
using FNHMVC.Data.Infrastructure;
using FNHMVC.Model;

namespace FNHMVC.Domain.Handlers
{
    public class CanAddToken : IValidationHandler<CreateOrUpdateTokenCommand>
    {
        private readonly ITokenRepository tokenRepository;

        public CanAddToken(ITokenRepository tokenRepository, IUnitOfWork unitOfWork)
        {
            this.tokenRepository = tokenRepository;
        }

        public IEnumerable<ValidationResult> Validate(CreateOrUpdateTokenCommand command)
        {
            Token isTokenExists = null;
            if (command.TokenId == 0)
                isTokenExists = tokenRepository.Get(c => c.ConfirmationToken == command.ConfirmationToken);
            else
                isTokenExists = tokenRepository.Get(c => c.ConfirmationToken == command.ConfirmationToken && c.TokenId != command.TokenId);

            if (isTokenExists != null)
            {
                yield return new ValidationResult("ConfirmationToken", Resources.TokenExist);
            }
        }
    }
}
