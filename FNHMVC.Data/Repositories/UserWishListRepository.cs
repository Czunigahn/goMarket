using FNHMVC.Data.Infrastructure;
using FNHMVC.Model;
using NHibernate;

namespace FNHMVC.Data.Repositories
{
    public class UserWishListRepository : RepositoryBase<UserWishList>, IUserWishListRepository
    {
        public UserWishListRepository(ISession session)
            : base(session)
        {

        }
    }

    public interface IUserWishListRepository : IRepository<UserWishList>
    {

    }
}