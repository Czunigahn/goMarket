using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using FNHMVC.Model;

namespace FNHMVC.Data.Mappings
{
    public class SalePendingChangeMap : ClassMap<SalePendingChange>
    {
        public SalePendingChangeMap()
        {
            Id(x => x.SalePendingChangeId).GeneratedBy.Identity();
            Map(x => x.Title).Not.Nullable();
            Map(x => x.Description).Nullable();
            Map(x => x.Cost).Not.Nullable();
            Map(x => x.Quantity).Not.Nullable();
            Map(x => x.Picture).Nullable();
            Map(x => x.YouTubeLink).Nullable();
            Map(x => x.Created).Not.Nullable();
            Map(x => x.Modified).Nullable();
            Map(x => x.Activated).Not.Nullable();

            Map(x => x.ReasonId).Nullable().Default("-1");
            Map(x => x.ReasonDescription).Nullable().Default("");

            Map(x => x.Altitude).Nullable();
            Map(x => x.Latitude).Nullable();
            Map(x => x.TookItHome).Nullable();

            References(x => x.Category).Not.Nullable();
            References(x => x.Sale).Not.Nullable();
           HasMany(x => x.SaleImages).LazyLoad().Inverse().Cascade.All();
        }
    }
}
