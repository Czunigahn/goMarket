using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Model;

namespace FNHMVC.Domain.Commands
{
    public class UserRegisterCommand : ICommand
    {
        public UserRegisterCommand() { }

        public UserRegisterCommand(FNHMVC.Model.User user, string password)
        {
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Email = user.Email;
            this.Password = password;
            this.RoleId = user.RoleId;
            this.Activated = user.Activated;
            this.Age = user.Age;
            this.PaypalAccount = user.PaypalAccount;//editado
            this.Genre = user.Genre;
            this.Country = user.Country;

        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PaypalAccount { get; set; }//editado
        public string Password { get; set; }
        public int RoleId { get; set; }
        public bool Activated { get; set; }
        public int Age { get; set; }
        public bool Genre { get; set; }
        public string Country { get; set; }
    }
}
