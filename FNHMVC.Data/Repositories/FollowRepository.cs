﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FNHMVC.Data.Infrastructure;
using FNHMVC.Model;
using NHibernate;

namespace FNHMVC.Data.Repositories
{
    class FollowRepository : RepositoryBase<Follow>, IFollowRepository
    {
        public FollowRepository(ISession session) : base(session)
        {
        }
    }

    public interface IFollowRepository : IRepository<Follow>
    {
        
    }
}
