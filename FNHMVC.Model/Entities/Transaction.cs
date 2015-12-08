using System;

namespace FNHMVC.Model
{
    public class Transaction
    {
        public virtual long TransactionId { get; set; }
        public virtual User Seller { get; set; }
        public virtual User Buyer { get; set; }
        public virtual Sale Sale { get; set; }
        public virtual int Quantity { get; set; }
        public virtual double TotalPrice { get; set; }
        public virtual double TotalTax { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual Expense Expense { get; set; }
    }
}