using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BootstrapMvcSample.Controllers;
using FNHMVC.Web.Models;
using FNHMVC.Web.ViewModels;
using FNHMVC.Domain.Commands;
using FNHMVC.Web.Core.Models;
using FNHMVC.CommandProcessor.Dispatcher;
using FNHMVC.Data.Repositories;
using FNHMVC.Core.Common;
using FNHMVC.Web.Core.Extensions;
using FNHMVC.Web.Core.Authentication;
using FNHMVC.Model;
using FNHMVC.Web.Core.ActionFilters;
using FNHMVC.Domain.Commands.SalePendingChange;


namespace FNHMVC.Web.Controllers
{
    [CompressResponse]
    [FNHMVCAuthorize(Roles.User, Roles.Admin)]
    public class AdminController : BootstrapBaseController
    {
        private readonly ICommandBus commandBus;
        private readonly IUserRepository userRepository;
        private readonly ISalePendingChangeRepository salePendingChangeRepository;
        private readonly ISaleRepository saleRepository;
        private readonly IDenounceRepository denounceRepository;
        //private readonly ITokenRepository tokenRepository;
        //private readonly IFollowRepository followRepository;
        //private readonly IFormsAuthentication formAuthentication;

        public AdminController(ICommandBus commandBus, IUserRepository userRepository, ISaleRepository saleRepository, ISalePendingChangeRepository salePendingChangeRepository, IDenounceRepository denounceRepository)
        {
            this.denounceRepository = denounceRepository;
            this.commandBus = commandBus;
            this.userRepository = userRepository;
            this.salePendingChangeRepository = salePendingChangeRepository;
            this.saleRepository = saleRepository;
        }

        public ActionResult Menu()
        {
            return View();
        }

        #region User


        [HttpGet]
        public ActionResult DeleteUser(long id)
        {
            var record = userRepository.GetById(id);

            return View(record);
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult DeleteUser(FNHMVC.Model.User form)
        {
            if (ModelState.IsValid)
            {
                form = userRepository.GetById(form.UserId);

                var command = new FNHMVC.Domain.Commands.User.DisableUserCommand(form.UserId, false, form.Locked);
                //IEnumerable<ValidationResult> errors = commandBus.Validate(command);
                //ModelState.AddModelErrors(errors);
                //if (ModelState.IsValid)
                //{
                var result = commandBus.Submit(command);
                if (result.Success)
                {
                    Information("Usuario:" + form.FirstName + " ha sido eliminado.");
                    new MailController().SendUserAccountDelete(form).Deliver();
                    return RedirectToAction("UserControl", "Admin");
                }
                //}
            }

            Error("Usuario:" + form.FirstName + " no se pudo eliminar");
            return RedirectToAction("UserControl", "Admin");
        }

        [HttpGet]
        public ActionResult DisableUser(long id, long denounce = 0)
        {

            var record = userRepository.GetById(id);

            return View(record);
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult DisableUser(FNHMVC.Model.User form, long id, long denounce)
        {
            if (ModelState.IsValid)
            {

                form = userRepository.GetById(form.UserId);

                var command = new FNHMVC.Domain.Commands.User.DisableUserCommand(form.UserId, form.Activated, true);
                //IEnumerable<ValidationResult> errors = commandBus.Validate(command);
                //ModelState.AddModelErrors(errors);
                //if (ModelState.IsValid)
                //{
                if (denounce != 0)
                {
                    var denounceVal = denounceRepository.Get(x => x.DenounceId == denounce && x.TookAction == false);
                    denounceVal.TookAction = true;
                    denounceRepository.Update(denounceVal);
                }
                var result = commandBus.Submit(command);
                if (result.Success)
                {
                    Information("Usuario:" + form.FirstName + " ha sido bloqueado.");
                    if (denounce != 0)
                    {

                        return RedirectToAction("UserDenounceControl", "Admin");
                    }
                    new MailController().SendUserAccountLock(form).Deliver();


                    return RedirectToAction("UserControl", "Admin");
                }
                //}
            }

            Error("Usuario:" + form.FirstName + " no se pudo eliminar");
            return RedirectToAction("UserControl", "Admin");
        }

        public ActionResult PendingChange()
        {
            var pendingChanges = saleRepository.GetMany(x => x.Activated &&
                                            x.SalePendingChange.Where(s => s.Activated == true).Count() > 0
                                       ).OrderBy(x => x.Modified);

            return View(pendingChanges);
        }

        [HttpGet]
        public ActionResult ApprobalChange(long id)
        {
            var sale = saleRepository.GetById(id);

            return View(sale);
        }

        [HttpGet]
        public ActionResult DenyChange(long id)
        {
            var sale = saleRepository.GetById(id);

            return View(sale);
        }

        [HttpPost]
        public ActionResult DenyChange(Sale sale)
        {
            sale = saleRepository.GetById(sale.SaleId);

            var command = new InactivaSalePendingChangeCommand();
            command.changes = sale.SalePendingChange.Where(x => x.Activated).ToList();

            var result = commandBus.Submit(command);
            if (result.Success)
            {
                new MailController().SendEmailDenyChange(sale).Deliver();
                Information("La publicación ha sido denegada.");
                return RedirectToAction("PendingChange", "Admin");                
            }

            Attention("Ha ocurrido un error desconocido.");
            return View(sale);
        }

        [HttpPost]
        public ActionResult ApprobalChange(Sale sale)
        {
            sale = saleRepository.GetById(sale.SaleId);

            var Changes = sale.SalePendingChange.Where(x => x.Activated).OrderByDescending(x => x.SalePendingChangeId).First();

            sale.Category = Changes.Category;
            sale.Cost = Changes.Cost;
            sale.Description = Changes.Description;
            sale.Modified = DateTime.Now;
            sale.PendingChange = false;
            sale.Picture = Changes.Picture;
            sale.Quantity = Changes.Quantity;
            sale.Title = Changes.Title;
            sale.YouTubeLink = Changes.YouTubeLink;

            var cmd = new CreateOrUpdateSaleCommand(sale, false);
            var resultCmd = commandBus.Submit(cmd);

            if (!resultCmd.Success)
            {
                Attention("Ha ocurrido un error desconocido.");
                return RedirectToAction("PendingChange", "Admin");
            }

            var command = new InactivaSalePendingChangeCommand();
            command.changes = sale.SalePendingChange.Where(x => x.Activated).ToList();

            var result = commandBus.Submit(command);
            if (result.Success)
            {
                Information("La publicación ha sido aprobada.");
                return RedirectToAction("PendingChange", "Admin");
            }

            else
                Attention("Ha ocurrido un error desconocido.");

            return View(sale);
        }

        public ActionResult UserControl()
        {
            var list = userRepository.GetMany(x => x.Activated);

            return View(list);
        }

        public ActionResult SaleDisable(long id, long denounce)
        {
            var sale = saleRepository.Get(x => x.SaleId == id);
            sale.Activated = false;
            sale.ActiveForSales = false;
            saleRepository.Update(sale);

            var denounceVar = denounceRepository.Get(x => x.DenounceId == denounce);
            denounceVar.TookAction = true;
            denounceRepository.Update(denounceVar);
            return RedirectToAction("SaleDenounceControl");
        }

        #endregion

        #region Denounces

        public ActionResult UserDenounceControl()
        {
            var list = denounceRepository.GetMany(x => x.TookAction == false && x.SaleToDenounce == null);

            return View(list);
        }

        public ActionResult SaleDenounceControl()
        {
            var list = denounceRepository.GetMany(x => x.TookAction == false && x.UserToDenounce == null);

            return View(list);
        }

        public ActionResult IgnoreDenounce(long id)
        {
            var denounce = denounceRepository.Get(x => x.DenounceId == id);
            denounce.TookAction = true;
            denounceRepository.Update(denounce);
            return RedirectToAction("UserDenounceControl");
        }
        
        #endregion

        #region SpotLigth

        public ActionResult SpotlightApprove()
        {
            var pendingChanges = saleRepository.GetMany(x => x.Activated &&
                                            x.Spotlight && !x.SpotlightApprove
                                       ).OrderBy(x => x.Modified);

            return View(pendingChanges);
        }

        [HttpGet]
        public ActionResult DenySpotlight(long id)
        {
            var sale = saleRepository.GetById(id);

            return View(sale);
        }

        [HttpPost]
        public ActionResult DenySpotlight(Sale sale)
        {
            sale = saleRepository.GetById(sale.SaleId);

            var command = new InactivaSalePendingChangeCommand();
            command.changes = sale.SalePendingChange.Where(x => x.Activated).ToList();

            var result = commandBus.Submit(command);
            if (result.Success)
            {
                new MailController().SendEmailDenyChange(sale).Deliver();
                Information("La solicitud ha sido denegada.");
                return RedirectToAction("PendingChange", "Admin");
            }

            Attention("Ha ocurrido un error desconocido.");
            return View(sale);
        }

        [HttpGet]
        public ActionResult ApprobalSpotlight(long id)
        {
            var sale = saleRepository.GetById(id);

            return View(sale);
        }

        [HttpPost]
        public ActionResult ApprobalSpotlight(Sale sale)
        {
            sale = saleRepository.GetById(sale.SaleId);

            var command = new DeleteSaleCommand(sale.SaleId, sale.Activated, sale.ActiveForSales, sale.PendingChange, true, true);

            var result = commandBus.Submit(command);

            Success("La solicitud ha sido aprobada .");

            return RedirectToAction("PendingChange");
        }
        #endregion
        
        public ActionResult Index()
        {
            return View();
        }

    }
}
