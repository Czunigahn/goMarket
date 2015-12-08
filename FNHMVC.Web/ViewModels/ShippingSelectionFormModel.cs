using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FNHMVC.Web.ViewModels
{
    public class ShippingSelectionFormModel
    {
        public IEnumerable<SelectListItem> Shippings { get; set; }
        public string ShippingAdress { get; set; }
    }
}