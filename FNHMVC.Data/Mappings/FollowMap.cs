using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FNHMVC.Model;
using FluentNHibernate.Mapping;

namespace FNHMVC.Data.Mappings
{
    public class FollowMap : ClassMap<Follow>
    {
        public FollowMap()
        {
            Id(x => x.FollowId).GeneratedBy.Identity();
            References(x => x.Follower).Not.Nullable();
            References(x => x.User).Not.Nullable();
        }
    }
}
