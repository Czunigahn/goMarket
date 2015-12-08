﻿using System.Web.Mvc;
using System.Web.Routing;
using FluentSecurity;


namespace FNHMVC.Web.Helpers
{

    public class RequireRolePolicyViolationHandler : IPolicyViolationHandler
    {

        public ActionResult Handle(PolicyViolationException exception)
        {
            LoggerManager.WriteAlert("Intento de acceso en area prohibida",exception);
            return new RedirectToRouteResult("Error",
                                       new RouteValueDictionary { { "Error", "Página no encontrada." } });

        }
    }

}