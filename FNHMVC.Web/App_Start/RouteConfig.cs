using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FNHMVC.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("SignIn", "SignIn", new { controller = "Account", action = "Login" });

            routes.MapRoute("Error", "Error", new { controller = "Error", action = "Error404" });

            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
             "UploadImagesRoute", // 
             "{controller}/{action}/{id}/{type}",
             new { controller = "UploadImages", action = "UploadImage", id = UrlParameter.Optional, type = UrlParameter.Optional }
         );

           
        }
    }
}