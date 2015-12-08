using System;
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
    public class CreateOrUpdateTransactionHandler : ICommandHandler<CreateOrUpdateTransactionCommand>
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateOrUpdateTransactionHandler(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork)
        {
            this.transactionRepository = transactionRepository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(CreateOrUpdateTransactionCommand command)
        {
            var transaction = new Transaction()
            {
                TransactionId = command.TransactionId,
                Buyer = command.Buyer,
                Created = command.Created,
                Expense = command.Expense,
                Quantity = command.Quantity,
                Sale = command.Sale,
                Seller = command.Seller,
                TotalPrice = command.TotalPrice,
                TotalTax = command.TotalTax
            };

            if (transaction.TransactionId == 0)
                transactionRepository.Add(transaction);
            else
                transactionRepository.Update(transaction);

            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }
}