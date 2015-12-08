using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using FluentSecurity;
using FNHMVC.Web.Controllers;
using FNHMVC.Web.Core.Models;

namespace FNHMVC.Web.Helpers
{
    public static class FluentSecurityConfig
    {

        public static void Configure()
        {
            SecurityConfigurator.Configure(configuration =>
            {
                configuration.GetAuthenticationStatusFrom(() => HttpContext.Current.User.Identity.IsAuthenticated);

                configuration.ForAllControllers().DenyAnonymousAccess();


                configuration.For<AccountController>(x => x.Facebook()).Ignore();
                configuration.For<AccountController>(x => x.FacebookCallback("")).Ignore();
                configuration.For<AccountController>(x => x.Login()).Ignore();
                configuration.For<AccountController>(x => x.Logout()).Ignore();

                configuration.For<AccountController>(x => x.ForgotPassword()).Ignore();
                configuration.For<AccountController>(x => x.ForgotPassword(new Models.ForgotPasswordModel())).Ignore();

                configuration.For<AccountController>(x => x.JsonLogin(new ViewModels.LogOnFormModel(), "index")).Ignore();
                configuration.For<AccountController>(x => x.JsonRegister(new ViewModels.UserFormModel())).Ignore();


                configuration.For<AccountController>(x => x.Confirm("", "")).Ignore();
                configuration.For<AccountController>(x => x.ForgotPassword()).Ignore();
                configuration.For<AccountController>(x => x.ForgotPassword(new Models.ForgotPasswordModel())).Ignore();
                configuration.For<AccountController>(x => x.ValidateForgotPassword("")).Ignore();

                configuration.For<AccountController>(x => x.Login(new ViewModels.LogOnFormModel(), "Index")).Ignore();
                configuration.For<AccountController>(x => x.Register()).Ignore();
                configuration.For<AccountController>(x => x.Register(new ViewModels.UserFormModel())).Ignore();

                configuration.For<HomeController>(x => x.About()).Ignore();
                configuration.For<HomeController>(x => x.Index()).Ignore();
                configuration.For<HomeController>(x => x.Search()).Ignore();
                configuration.For<HomeController>(x => x.Search(new ViewModels.SearchFormModel())).Ignore();

                configuration.For<HomeController>(x => x.GetSales()).Ignore();

                //configuration.For<AdminController>().RequireRole(new object[] { Roles.Admin });


                configuration.ResolveServicesUsing(type =>
                {
                    if (type == typeof(IPolicyViolationHandler))
                    {
                        var types = Assembly
                            .GetAssembly(typeof(MvcApplication))
                            .GetTypes()
                            .Where(x => typeof(IPolicyViolationHandler).IsAssignableFrom(x)).ToList();

                        var handlers = types.Select(t => Activator.CreateInstance(t) as IPolicyViolationHandler).ToList();

                        return handlers;
                    }
                    return Enumerable.Empty<object>();
                });
            });

        }

    }
}