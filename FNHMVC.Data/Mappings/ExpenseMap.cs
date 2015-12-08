using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using FNHMVC.Model;
using FluentNHibernate.Mapping;

namespace FNHMVC.Data.Mappings
{
    public class ExpenseMap : ClassMap<Expense>
    {
        public ExpenseMap()
        {
            Id(x => x.ExpenseId).GeneratedBy.Identity();
            Map(x => x.Token).Length(100).Not.Nullable();
            Map(x => x.PayerID).Length(100).Nullable();
            Map(x => x.Shipping).Length(100).Nullable();
            Map(x => x.Created).Not.Nullable();
            Map(x => x.Amount).Not.Nullable();
            Map(x => x.CheckoutCompleted).Not.Nullable();
            References(x => x.User).Not.Nullable();
            HasMany(x => x.Transactions).LazyLoad().Inverse().Cascade.All();
        }
    }
}