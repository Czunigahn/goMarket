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

    public class SalePendingChangeRepository : RepositoryBase<SalePendingChange>, ISalePendingChangeRepository
    {
        public SalePendingChangeRepository(ISession session): base(session)
        {
        }
    }

    public interface ISalePendingChangeRepository : IRepository<SalePendingChange>
    {
    }
}
