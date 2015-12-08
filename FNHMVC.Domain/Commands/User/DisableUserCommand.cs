using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FNHMVC.CommandProcessor.Command;

namespace FNHMVC.Domain.Commands.User
{
    public class DisableUserCommand : ICommand
    {
        public DisableUserCommand(long UserId, bool Active, bool Locked)
        {
            this.UserId = UserId;
            this.Active = Active;
            this.Locked = Locked;
        }

        public long UserId { get; set; }
        public bool Active { get; set; }
        public bool Locked { get; set; }
    }
}
