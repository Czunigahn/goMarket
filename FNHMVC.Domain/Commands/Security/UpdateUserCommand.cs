using System;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Model;
using System.Collections.Generic;

namespace FNHMVC.Domain.Commands
{
    public class UpdateUserCommand : ICommand
    {
        public long UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public bool Genre { get; set; }
        public string Country { get; set; }
        public string PaypalAccount { get; set; } //editado
        public DateTime? LastLoginTime { get; set; }
        public bool Activated { get; set; }
        public int RoleId { get; set; }
    }
}
