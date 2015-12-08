using FNHMVC.Model;
using FluentNHibernate.Mapping;

namespace FNHMVC.Data.Mappings
{
    public class UserAddressMap : ClassMap<UserAddress>
    {
        public UserAddressMap()
        {
            Id(x => x.UserAddressId).GeneratedBy.Identity();
            Map(x => x.AddressLine1).Length(100).Not.Nullable();
            Map(x => x.AddressLine2).Length(100).Nullable();
            Map(x => x.FullName).Length(100).Not.Nullable();
            Map(x => x.City).Length(100).Not.Nullable();
            Map(x => x.State).Length(100).Not.Nullable();
            Map(x => x.Country).Length(100).Not.Nullable();
            Map(x => x.ZipCode).Length(100).Nullable();
            Map(x => x.PhoneNumber).Length(100).Nullable();
            Map(x => x.Activated).Nullable().Default("-1");
            References(x => x.User).Not.Nullable();
        }

        
    }
}