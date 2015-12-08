using FNHMVC.Model;
using FluentNHibernate.Mapping;

namespace FNHMVC.Data.Mappings
{
    public class TransactionMap : ClassMap<Transaction>
    {
        public TransactionMap()
        {
            Id(x => x.TransactionId).GeneratedBy.Identity();
            Map(x => x.Created).Not.Nullable();
            Map(x => x.TotalPrice).Not.Nullable();
            Map(x => x.Quantity).Not.Nullable();
            Map(x => x.TotalTax).Not.Nullable();
            References(x => x.Sale).Not.Nullable();
            References(x => x.Buyer).Not.Nullable();
            References(x => x.Seller).Not.Nullable();
            References(x => x.Expense).Not.Nullable();
        }
    }
}

