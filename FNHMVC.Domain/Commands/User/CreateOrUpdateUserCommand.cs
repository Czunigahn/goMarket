using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FNHMVC.CommandProcessor.Command;

namespace FNHMVC.Domain.Commands.User
{
    public class CreateOrUpdateUserCommand : ICommand
    {
        public virtual long UserId { get; set; }
        public virtual string Email { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual int Age { get; set; }
        public virtual bool Genre { get; set; }
        public virtual string Country { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? LastLoginTime { get; set; }
        public virtual bool Activated { get; set; }
        public virtual string PaypalAccount { get; set; }

        public virtual bool Locked { get; set; }
        public virtual string Picture { get; set; }
        public virtual string About { get; set; }
        public virtual int Rate { get; set; }

        public virtual int RoleId { get; set; }
    }
}
