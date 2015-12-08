using System;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Model;
using System.Collections.Generic;

namespace FNHMVC.Domain.Commands
{
    public class CreateFollowCommand : ICommand
    {
        public CreateFollowCommand(long FollowId, FNHMVC.Model.User Follower, FNHMVC.Model.User User)
        {
            this.FollowId = FollowId;
            this.Follower = Follower;
            this.User = User;
        }

        public CreateFollowCommand(Follow follow)
        {
            this.FollowId = follow.FollowId;
            this.Follower = follow.Follower;
            this.User = follow.User;
        }

        public long FollowId { get; set; }
        public FNHMVC.Model.User Follower { get; set; }
        public FNHMVC.Model.User User { get; set; }
    }
}
