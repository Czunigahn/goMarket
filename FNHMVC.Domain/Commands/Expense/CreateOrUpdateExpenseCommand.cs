using System;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Model;
using System.Collections.Generic;

namespace FNHMVC.Domain.Commands
{
    public class CreateOrUpdateExpenseCommand : ICommand
    {
        public long ExpenseId { get; set; }
        public string Token { get; set; }
        public string PayerID { get; set; }
        public DateTime Created { get; set; }
        public FNHMVC.Model.User User { get; set; }
        public double Amount { get; set; }
        public bool CheckoutCompleted { get; set; }
    }
}
