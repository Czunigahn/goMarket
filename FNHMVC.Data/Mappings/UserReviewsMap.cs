using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using FNHMVC.Model;
using FluentNHibernate.Mapping;

namespace FNHMVC.Data.Mappings
{
    public class UserReviewsMap : ClassMap<UserReviews>
    {
        public UserReviewsMap()
        {
            Id(x => x.ReviewId).GeneratedBy.Identity();
            Map(x => x.Date).Not.Nullable();
            Map(x => x.Value).Not.Nullable();
            Map(x => x.Comment).Not.Nullable();
            Map(x => x.Title).Not.Nullable();
            Map(x => x.Active).Not.Nullable();
            References(x => x.User).Not.Nullable();
            References(x => x.Sale).Not.Nullable();

        }
    }
}
