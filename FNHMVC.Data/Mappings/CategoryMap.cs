using FNHMVC.Model;
using FluentNHibernate.Mapping;

namespace FNHMVC.Data.Mappings
{
    public class CategoryMap : ClassMap<Category>
    {
        public CategoryMap()
        {
            Id(x => x.CategoryId).GeneratedBy.Identity();
            Map(x => x.Name).Length(100).Not.Nullable();
            Map(x => x.Description).Length(256).Nullable();
            Map(x => x.Activated).Not.Nullable();
            HasMany(x => x.Sales).LazyLoad().Inverse().Cascade.All();
            References(x => x.Parent).Nullable();
        }
    }
}