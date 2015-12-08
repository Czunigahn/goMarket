using FNHMVC.Model;
using FluentNHibernate.Mapping;

namespace FNHMVC.Data.Mappings
{
    public class CuponMap : ClassMap<FNHMVC.Model.Cupon>
    {
        public CuponMap()
        {
            Id(x => x.CuponId).GeneratedBy.Identity();
            Map(x => x.CuponName).Length(10).Not.Nullable();
            Map(x => x.Discount).Not.Nullable();
            References(x => x.User).Not.Nullable();
            Map(x => x.IsActive).Not.Nullable();
            Map(x => x.Created).Not.Nullable();
            Map(x => x.TimesUsed).Not.Nullable();
        }
    }
}