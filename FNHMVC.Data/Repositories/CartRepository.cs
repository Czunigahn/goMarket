using FNHMVC.Data.Infrastructure;
using FNHMVC.Model;
using NHibernate;

namespace FNHMVC.Data.Repositories
{
    public class CartRepository : RepositoryBase<Cart>, ICartRepository
    {
        public CartRepository(ISession session)
            : base(session)
        {
        }
    }

    public interface ICartRepository : IRepository<Cart>
    {
    }
}