using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNHMVC.Model
{
    public class Follow
    {
        public virtual long FollowId { get; set; }
        public virtual User Follower { get; set; }
        public virtual User User { get; set; } 
    }
}
