using FNHMVC.Data.Infrastructure;
using FNHMVC.Model;
using NHibernate;

namespace FNHMVC.Data.Repositories
{
    public class DenounceRepository : RepositoryBase<Denounce>, IDenounceRepository
    {
        public DenounceRepository(ISession session)
            : base(session)
        {
        }
    }

    public interface IDenounceRepository : IRepository<Denounce>
    {
    }
}