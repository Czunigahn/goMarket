using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using FNHMVC.Model;
using FluentNHibernate.Mapping;

namespace FNHMVC.Data.Mappings
{
    public partial class UserMap: ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.UserId).GeneratedBy.Identity();
            Map(x => x.Email).Length(256).Unique().Not.Nullable();
            Map(x => x.FirstName).Length(100).Not.Nullable();
            Map(x => x.LastName).Length(100).Not.Nullable();
            Map(x => x.PaypalAccount).Length(100).Nullable();
            Map(x => x.Age).Not.Nullable();
            Map(x => x.Country).Not.Nullable();
            Map(x => x.Genre).Not.Nullable();
            Map(x => x.PasswordHash).Length(256).Nullable();
            Map(x => x.RoleId).Not.Nullable();
            Map(x => x.DateCreated).Not.Nullable();
            Map(x => x.LastLoginTime).Nullable();
            Map(x => x.Activated).Not.Nullable();
            
            Map(x => x.Locked).Not.Nullable();
            Map(x => x.Rate).Nullable().Default("0");
            Map(x => x.About).Nullable();
            Map(x => x.Picture).Nullable();
            
            HasMany(x => x.Sales).LazyLoad().Inverse().Cascade.All();
            HasMany(x => x.Followers).LazyLoad().Inverse().Cascade.All();
            HasMany(x => x.Reviews).LazyLoad().Inverse().Cascade.All();
            HasMany(x => x.UserAddress).LazyLoad().Inverse().Cascade.All();
            HasMany(x => x.UserInbox).LazyLoad().Inverse().Cascade.All();
        }
    }
}
