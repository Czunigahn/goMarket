using FNHMVC.Model;
using FluentNHibernate.Mapping;

namespace FNHMVC.Data.Mappings
{
    public class SaleImagesMap : ClassMap<SaleImages>
    {
        public SaleImagesMap()
        {
            Id(x => x.SaleImagesId).GeneratedBy.Identity();
            Map(x => x.Url).Not.Nullable();
            Map(x => x.Activated).Not.Nullable();
            Map(x => x.Type).Default("1").Not.Nullable();
            References(x => x.Sale).Nullable();
            References(x => x.SalePendingChange).Nullable();
            
        }
    }
}