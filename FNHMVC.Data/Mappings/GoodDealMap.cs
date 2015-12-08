using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FNHMVC.Model;
using FluentNHibernate.Mapping;

namespace FNHMVC.Data.Mappings
{
    class GoodDealMap: ClassMap<GoodDeal>
    {
        public GoodDealMap()
        {
            Id(x => x.GoodDealId).GeneratedBy.Identity();
            References(x => x.User).Not.Nullable();
            References(x => x.Sale).Not.Nullable();
        }

    }
}
