﻿using System.Web.Mvc;
using System.Web.Routing;
using FluentSecurity;


namespace FNHMVC.Web.Helpers
{
    public class DenyAnonymousAccessPolicyViolationHandler : IPolicyViolationHandler
    {
        public ActionResult Handle(PolicyViolationException exception)
        {
            LoggerManager.WriteAlert("Intento de acceso en area prohibida.", exception);

            return new RedirectToRouteResult("SignIn",
                                             new RouteValueDictionary { { "Error", "Debes iniciar sesion para poder utilizar esta sección" } });
        }

    }
}