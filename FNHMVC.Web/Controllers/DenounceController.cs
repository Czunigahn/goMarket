using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BootstrapMvcSample.Controllers;
using FNHMVC.CommandProcessor.Dispatcher;
using FNHMVC.Data.Repositories;
using FNHMVC.Domain.Commands;
using FNHMVC.Model;
using FNHMVC.Web.Core.ActionFilters;
using FNHMVC.Web.Core.Extensions;
using FNHMVC.Web.Core.Models;
using FNHMVC.Web.Helpers;
using FNHMVC.Web.Models;
using FNHMVC.Web.ViewModels;
using Facebook;

namespace FNHMVC.Web.Controllers
{
    [CompressResponse]
    [FNHMVCAuthorize(Roles.User, Roles.Admin)]
    public class DenounceController : BootstrapBaseController
    {
        private readonly ICommandBus commandBus;
        private readonly ISaleRepository saleRepository;
        private readonly IUserRepository userRepository;
        public DenounceController(ICommandBus commandBus, ISaleRepository saleRepository, IUserRepository userRepository)
        {
            this.commandBus = commandBus;
            this.saleRepository = saleRepository;
            this.userRepository = userRepository;
        }

        public ActionResult DenounceSale(long id)
        {
            var sale = saleRepository.Get(x => x.SaleId == id && x.PendingChange == false && x.ActiveForSales);
            if (sale == null)
            {
                Attention("La venta no existe, o esta temporalmente deshabilitada");
            }
            else
            {
                var viewDenounce = new DenounceSaleFormModel(sale.Title);
                viewDenounce.Reasons = new List<SelectListItem>
                       {
                         new SelectListItem {Text = "Desnudos o Pornografía", Value = "Desnudos o Pornografía"},
                         new SelectListItem {Text = "Drogas", Value = "Drogas"},
                         new SelectListItem {Text = "Contenido Fraudulento", Value = "Contenido Fraudulento"},
                         new SelectListItem {Text = "Incito al Odio", Value = "Incito al Odio"},
                         new SelectListItem {Text = "Violencia Gráfica", Value = "Violencia Gráfica"}
                       };
                return View(viewDenounce);
            }
            return RedirectToAction("Index", "Sale");
        }

        [HttpPost]
        public ActionResult DenounceSale(DenounceSaleFormModel model, long id)
        {
            var user = userRepository.GetById(HttpContext.User.GetFNHMVCUser().UserId);

            var sale = saleRepository.Get(x => x.SaleId == id && x.PendingChange == false && x.ActiveForSales);
            var denounce = new Denounce
            {
                Comment = model.Comment,
                Created = DateTime.Now,
                DenounceId = 0,
                Reason = model.Reason,
                SaleToDenounce = sale,
                TookAction = false,
                UserDenouncing = user,
                UserToDenounce = null
            };

            var command = new CreateOrUpdateDenounceCommand(denounce);
            var result = commandBus.Submit(command);
            if (result.Success)
                Success("Su denuncia se envió correctamente!");
            return RedirectToAction("ViewSale", "Sale", new { id = sale.SaleId });
        }

        public ActionResult DenounceUser(long id)
        {
            var user = userRepository.Get(x => x.UserId == id && x.Activated && x.Locked == false);
            if (user == null)
            {
                Attention("El usuario no existe, o esta temporalmente deshabilitado");
            }
            else
            {
                var viewDenounce = new DenounceUserFormModel(user.FullName);
                viewDenounce.Reasons = new List<SelectListItem>
                       {
                         new SelectListItem {Text = "Desnudos o Pornografía", Value = "Desnudos o Pornografía"},
                         new SelectListItem {Text = "Drogas", Value = "Drogas"},
                         new SelectListItem {Text = "Contenido Fraudulento", Value = "Contenido Fraudulento"},
                         new SelectListItem {Text = "Incito al Odio", Value = "Incito al Odio"},
                         new SelectListItem {Text = "Violencia Gráfica", Value = "Violencia Gráfica"}
                       };
                return View(viewDenounce);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult DenounceUser(DenounceSaleFormModel model, long id)
        {
            var user = userRepository.GetById(HttpContext.User.GetFNHMVCUser().UserId);

            var userToDenounce = userRepository.Get(x => x.UserId == id && x.Activated && x.Locked == false);
            var denounce = new Denounce
            {
                Comment = model.Comment,
                Created = DateTime.Now,
                DenounceId = 0,
                Reason = model.Reason,
                SaleToDenounce = null,
                TookAction = false,
                UserDenouncing = user,
                UserToDenounce = userToDenounce
            };

            var command = new CreateOrUpdateDenounceCommand(denounce);
            var result = commandBus.Submit(command);
            if (result.Success)
                Success("Su denuncia se envió correctamente!");
            return RedirectToAction("UserProfile", "Home", new { id = userToDenounce.UserId });
        }
    }
}