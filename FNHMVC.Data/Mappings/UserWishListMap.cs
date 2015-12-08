using FNHMVC.Model;
using FluentNHibernate.Mapping;

namespace FNHMVC.Data.Mappings
{
    public class UserWishListMap : ClassMap<UserWishList>
    {
        public UserWishListMap()
        {
            Id(x => x.UserWishListId).GeneratedBy.Identity();
            Map(x => x.DateCreate).Not.Nullable();
            Map(x => x.Activated).Not.Nullable();
            Map(x => x.Name).Length(30).Not.Nullable();
            Map(x => x.Description).Nullable();
            References(x => x.User).Not.Nullable();
            References(x => x.Sale).Not.Nullable();
           
        }
    }
}