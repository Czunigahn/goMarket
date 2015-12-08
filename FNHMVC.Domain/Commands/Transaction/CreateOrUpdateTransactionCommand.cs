using System;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Model;

namespace FNHMVC.Domain.Commands
{
    public class CreateOrUpdateTransactionCommand : ICommand
    {
        public long TransactionId { get; set; }
        public Model.User Seller { get; set; }
        public Model.User Buyer { get; set; }
        public Sale Sale { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public double TotalTax { get; set; }
        public DateTime Created { get; set; }
        public Expense Expense { get; set; }
    }
}
