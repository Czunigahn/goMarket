using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FNHMVC.CommandProcessor.Command;

namespace FNHMVC.Domain.Commands
{
    public class CreateOrUpdateUserAddressCommand : ICommand
    {
        public virtual long UserAddressId { get; set; }
        public virtual FNHMVC.Model.User User { get; set; }
        public virtual string FullName { get; set; }
        public virtual string AddressLine1 { get; set; }
        public virtual string AddressLine2 { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual string ZipCode { get; set; }
        public virtual string Country { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual bool Activated { get; set; }
        public bool CommitAfterAccept { get; set; }

        public CreateOrUpdateUserAddressCommand(long UserAddressId, FNHMVC.Model.User User, string FullName, string AddressLine1, string AddressLine2, string City, string State, string ZipCode, string Country, string PhoneNumber, bool Activated)
        {
            this.UserAddressId = UserAddressId;
            this.User = User;
            this.FullName = FullName;
            this.AddressLine1 = AddressLine1;
            this.AddressLine2 = AddressLine2;
            this.City = City;
            this.State = State;
            this.ZipCode = ZipCode;
            this.Country = Country;
            this.PhoneNumber = PhoneNumber;
            this.Activated = Activated;
            CommitAfterAccept = true;
        }

        public CreateOrUpdateUserAddressCommand(FNHMVC.Model.UserAddress Address)
        {
            this.UserAddressId = Address.UserAddressId;
            this.User = Address.User;
            this.FullName = Address.FullName;
            this.AddressLine1 = Address.AddressLine1;
            this.AddressLine2 = Address.AddressLine2;
            this.City = Address.City;
            this.State = Address.State;
            this.ZipCode = Address.ZipCode;
            this.Country = Address.Country;
            this.PhoneNumber = Address.PhoneNumber;
            this.Activated = Address.Activated;
            CommitAfterAccept = true;

        }

        public CreateOrUpdateUserAddressCommand()
        {
            CommitAfterAccept = true;
        }
    }
}
