using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using FNHMVC.Model;

namespace FNHMVC.Web.ViewModels
{
    public class UserAddressFormModel
    {
        [Display(Name = "Id")]
        public virtual long UserAddressId { get; set; }

        [Display(Name = "Usuario")]
        public virtual User User { get; set; }
        
        [Display(Name = "Nombre completo")]
        public virtual string FullName { get; set; }

        [Display(Name = "Direccián 1")]
        public virtual string AddressLine1 { get; set; }

        [Display(Name = "Direccián 2")]
        public virtual string AddressLine2 { get; set; }

        [Display(Name = "Ciudad")]
        public virtual string City { get; set; }

        [Display(Name = "Estado")]
        public virtual string State { get; set; }

        [Display(Name = "Codigo postal")]
        public virtual string ZipCode { get; set; }

        [Display(Name = "País")]
        public virtual string Country { get; set; }

        [Display(Name = "Numero telefonico")]
        public virtual string PhoneNumber { get; set; }

        [Display(Name = "Activa")]
        public virtual bool Activated { get; set; }

        public UserAddressFormModel()
        {
            Activated = true;
        }

        public UserAddressFormModel(UserAddress userAddress)
        {
            this.UserAddressId = userAddress.UserAddressId;
            this.User = userAddress.User;
            this.FullName = userAddress.FullName;
            this.AddressLine1 = userAddress.AddressLine1;
            this.AddressLine2 = userAddress.AddressLine2;
            this.City = userAddress.City;
            this.State = userAddress.State;
            this.ZipCode = userAddress.ZipCode;
            this.Country = userAddress.Country;
            this.PhoneNumber = userAddress.PhoneNumber;
            this.Activated = userAddress.Activated;
        }
    }
}