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
    public class CreateOrUpdateExpenseHandler : ICommandHandler<CreateOrUpdateExpenseCommand>
    {
        private readonly IExpenseRepository expenseRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateOrUpdateExpenseHandler(IExpenseRepository expenseRepository, IUnitOfWork unitOfWork)
        {
            this.expenseRepository = expenseRepository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(CreateOrUpdateExpenseCommand command)
        {
            var expense = new Expense
            {
                ExpenseId = command.ExpenseId,
                Amount = command.Amount,
                CheckoutCompleted = command.CheckoutCompleted,
                Created = command.Created,
                PayerID = command.PayerID,
                Token = command.Token,
                User = command.User
            };

            if (expense.ExpenseId == 0)
                expenseRepository.Add(expense);
            else
                expenseRepository.Update(expense);

            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }
}
