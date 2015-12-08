using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FNHMVC.Model;
using System.Web.Mvc;

namespace FNHMVC.Web.Helpers
{
    public static class ToSelectListItemsHelper
    {
        public static IEnumerable<SelectListItem> ToSelectListItems(
              this IEnumerable<Category> categories, long selectedId)
        {
            return

                categories.OrderBy(category => category.Name)
                      .Select(category =>
                          new SelectListItem
                          {
                              Selected = (category.CategoryId == selectedId),
                              Text = category.Name,
                              Value = category.CategoryId.ToString()
                          });
        }
    }

    public static class ToSelectListItemsShippingHelper
    {
        public static IEnumerable<SelectListItem> ToSelectListItems(
              this IEnumerable<UserAddress> shippings, long selectedId = 0)
        {
            return

                shippings.OrderBy(ship => ship.AddressLine1)
                      .Select(ship =>
                          new SelectListItem
                          {
                              Selected = (ship.UserAddressId == selectedId),
                              Text = string.Format("{0}, {1}. Country:{2}", ship.AddressLine1, ship.AddressLine2, ship.Country),
                              Value = string.Format("{0}, {1}. Country:{2}", ship.AddressLine1, ship.AddressLine2, ship.Country)
                          });
        }
    }
}