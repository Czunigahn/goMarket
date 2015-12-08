using FNHMVC.Data.Infrastructure;
using FNHMVC.Model;
using NHibernate;

namespace FNHMVC.Data.Repositories
{
    public class CuponRepository : RepositoryBase<FNHMVC.Model.Cupon>, ICuponRepository
    {
        public CuponRepository(ISession session)
            : base(session)
        {
        }
    }

    public interface ICuponRepository : IRepository<FNHMVC.Model.Cupon>
    {
    }
}