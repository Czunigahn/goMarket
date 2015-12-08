using FNHMVC.Model;
using FluentNHibernate.Mapping;

namespace FNHMVC.Data.Mappings
{
    public class CartMap : ClassMap<Cart>
    {
        public CartMap()
        {
            Id(x => x.CartId).GeneratedBy.Identity();
            References(x => x.User).Not.Nullable();
            References(x => x.Sale).Not.Nullable();
            References(x => x.Cupon).Nullable();
            Map(x => x.Quantity).Not.Nullable();
            Map(x => x.Created).Not.Nullable();
        }
    }
}