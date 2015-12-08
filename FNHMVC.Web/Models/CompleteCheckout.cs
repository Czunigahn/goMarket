using System.Collections.Generic;
using FNHMVC.Model;
using FNHMVC.Web.ViewModels;

namespace FNHMVC.Web.Models
{
    public class CompleteCheckout
    {
        public CompleteCheckout()
        {
        }

        public IEnumerable<Cart> Cart { get; set; }
        public ShippingSelectionFormModel FormModel { get; set; }
    }
}