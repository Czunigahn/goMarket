using FNHMVC.Data.Infrastructure;
using FNHMVC.Model;
using NHibernate;

namespace FNHMVC.Data.Repositories
{
    public class SaleImagesRepository : RepositoryBase<SaleImages>, ISaleImagesRepository
    {
        public SaleImagesRepository(ISession session)
            : base(session)
        {
        }
    }

    public interface ISaleImagesRepository : IRepository<SaleImages>
    {
    }
}