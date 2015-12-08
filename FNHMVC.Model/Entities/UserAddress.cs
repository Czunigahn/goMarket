using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FNHMVC.Model
{
    public class UserAddress
    {
        public virtual long UserAddressId { get; set; }
        public virtual User User { get; set; }
        public virtual string FullName { get; set; }
        public virtual string AddressLine1 { get; set; }
        public virtual string AddressLine2 { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual string ZipCode { get; set; }
        public virtual string Country { get; set; }
        public virtual string PhoneNumber { get; set; }

        public virtual bool Activated { get; set; }
    }
}
