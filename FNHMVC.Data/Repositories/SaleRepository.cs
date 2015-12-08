using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FNHMVC.Data.Infrastructure;
using FNHMVC.Model;

using NHibernate;

namespace FNHMVC.Data.Repositories
{
    public class SaleRepository : RepositoryBase<Sale>, ISaleRepository
    {
        public SaleRepository(ISession session) : base(session)
        {
        }
    }

    public interface ISaleRepository : IRepository<Sale>
    {
    }


}
