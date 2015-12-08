using System;
using FNHMVC.Model;
using FluentNHibernate.Mapping;

namespace FNHMVC.Data.Mappings
{
    public class UserInboxMap : ClassMap<UserInbox>
    {
        public UserInboxMap()
        {
            Id(x => x.UserInboxId).GeneratedBy.Identity();
            Map(x => x.DateCreate).Not.Nullable();
            Map(x => x.DateRead).Nullable();
            Map(x => x.WasRead).Not.Nullable();
            Map(x => x.Activated).Not.Nullable();
            Map(x => x.SentEmail).Not.Nullable();
            Map(x => x.Subject).Not.Nullable();
            Map(x => x.Message).CustomSqlType("varchar(max)").Not.Nullable();
            Map(x => x.LastMessage).CustomSqlType("varchar(max)").Nullable();
            References(x => x.User).Not.Nullable();
            References(x => x.Seller).Not.Nullable();

        }
    }
}