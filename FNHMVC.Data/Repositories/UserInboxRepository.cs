using FNHMVC.Data.Infrastructure;
using NHibernate;

namespace FNHMVC.Data.Repositories
{
    public class UserInboxRepository : RepositoryBase<FNHMVC.Model.UserInbox>, IUserInboxRepository
    {
        public UserInboxRepository(ISession session)
            : base(session)
        {
        }
    }

    public interface IUserInboxRepository : IRepository<FNHMVC.Model.UserInbox>
    {
    }
}