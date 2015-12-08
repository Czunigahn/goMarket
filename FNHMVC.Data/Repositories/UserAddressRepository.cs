using FNHMVC.Data.Infrastructure;
using FNHMVC.Model;

using NHibernate;

namespace FNHMVC.Data.Repositories
{
    public class UserAddressRepository : RepositoryBase<FNHMVC.Model.UserAddress>, IUserAddressRepository
    {
        public UserAddressRepository(ISession session)
            : base(session)
        {
        }
    }

    public interface IUserAddressRepository : IRepository<FNHMVC.Model.UserAddress>
    {
    }


 
}