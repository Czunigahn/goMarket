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
  
    public class UserReviewsRepository : RepositoryBase<UserReviews>, IUserReviewsRepository
    {
        public UserReviewsRepository(ISession session)
            : base(session)
        {

        }
    }

    public interface IUserReviewsRepository : IRepository<UserReviews>
    {
    }


 
}
