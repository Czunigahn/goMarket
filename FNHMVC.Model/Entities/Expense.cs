using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace FNHMVC.Model
{

    public class Expense
    {
        public virtual long ExpenseId { get; set; }
        public virtual string Token { get; set; }
        public virtual string PayerID { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual FNHMVC.Model.User User { get; set; }
        public virtual double Amount { get; set; }
        public virtual bool CheckoutCompleted { get; set; }
        public virtual string Shipping { get; set; }
        public virtual IList<Transaction> Transactions { get; set; }
    }
}
