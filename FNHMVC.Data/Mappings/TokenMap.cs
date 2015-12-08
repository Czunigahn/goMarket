using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FNHMVC.Model;
using FluentNHibernate.Mapping;

namespace FNHMVC.Data.Mappings
{
    class TokenMap : ClassMap<Token>
    {
        public TokenMap()
        {
            Id(x => x.TokenId).GeneratedBy.Identity();
            Map(x => x.ConfirmationToken).Unique().Not.Nullable();
            Map(x => x.UserId).Not.Nullable();
            Map(x => x.Action).Not.Nullable();
            Map(x => x.Activated).Not.Nullable();
        }
    }
}
