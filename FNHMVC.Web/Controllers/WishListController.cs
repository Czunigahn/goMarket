using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BootstrapMvcSample.Controllers;
using FNHMVC.CommandProcessor.Dispatcher;
using FNHMVC.Data.Repositories;
using FNHMVC.Model;
using FNHMVC.Web.Core.ActionFilters;
using FNHMVC.Web.Core.Models;

using FNHMVC.Domain.Commands;

using FNHMVC.Web.Core.Extensions;
using FNHMVC.Web.Core.Models;
using FNHMVC.Web.Helpers;
using FNHMVC.Web.ViewModels;

namespace FNHMVC.Web.Controllers
{
    [CompressResponse]
    [FNHMVCAuthorize(Roles.User, Roles.Admin)]
    public class WishListController : BootstrapBaseController
    {
        //
        // GET: /WishList/
        private readonly ICommandBus commandBus;
        private readonly IUserWishListRepository userWishListRepository;
        private readonly IUserRepository userRepository;
        private readonly ISaleRepository saleRepository;

        public WishListController(ICommandBus commandBus, IUserRepository userRepository, IUserWishListRepository userWishListRepository, ISaleRepository saleRepository)
        {
            this.commandBus = commandBus;
            this.userRepository = userRepository;
            this.userWishListRepository = userWishListRepository;
            this.saleRepository = saleRepository;
        }

        public ActionResult Index()
        {
            User user = userRepository.GetById(HttpContext.User.GetFNHMVCUser().UserId);

            if (user == null)
            {
                Error("Tu sesión ha caducado, vuelve a iniciar sesión.");
                return RedirectToAction("Login", "Account");
                //return Json(new { error = "Tu sesión ha caducado, vuelve a iniciar sesión.", success = false, id = SaleId }, JsonRequestBehavior.AllowGet);
            }

            var wishlist = userWishListRepository.GetMany(x => x.Activated && x.User.UserId == user.UserId);

            return View(wishlist);

        }

        public ActionResult Delete(long id)
        {
            var wishList = userWishListRepository.GetById(id);
            if (wishList == null)
            {
                Attention("El registro no existe, o esta temporalmente deshabilitado");
                return RedirectToAction("Index");
            }

            wishList.Description = wishList.Sale.Description;
            wishList.Name = wishList.Sale.Title;
            
            return View(wishList);
        }

        [HttpPost]
        public ActionResult Delete(UserWishList wishlist)
        {
            wishlist = userWishListRepository.GetById(wishlist.UserWishListId);

            var command = new CreateOrUpdateUserWishListCommand(wishlist);
            command.Activated = false;

            var result = commandBus.Submit(command);

            if (result.Success)
                Information("La publicación ha sido eliminada de tus deseos.");
            else
                Error("No se pude eliminar la publicación de tus deseos");

            return RedirectToAction("Index");
        }

        public ActionResult AddToWishList(long id)
        {
            var SaleId = id;
            bool success = false;
            string error = "";

            try
            {
                User user = userRepository.GetById(HttpContext.User.GetFNHMVCUser().UserId);

                if (user == null)
                {
                    Error("Tu sesión ha caducado, vuelve a iniciar sesión.");
                    return RedirectToAction("Login", "Account");
                    //return Json(new { error = "Tu sesión ha caducado, vuelve a iniciar sesión.", success = false, id = SaleId }, JsonRequestBehavior.AllowGet);
                }

                var sale = saleRepository.GetById(id);
                if (sale == null)
                {
                    Error("La venta que desea ver no existe ó esta deshabilitada temporalmente.");
                    return RedirectToAction("ViewSale", "Sale", new { id = id });
                    //return Json(new { error = "La venta que desea ver no existe ó esta deshabilitada temporalmente.", success = false, id = SaleId }, JsonRequestBehavior.AllowGet);
                }

                UserWishList MyWishList = null;

                var wishlist = userWishListRepository.GetMany(x => x.Activated && x.User.UserId == user.UserId && x.Sale.SaleId == sale.SaleId);

                if (wishlist != null && wishlist.Count() > 0)
                {
                    MyWishList = wishlist.FirstOrDefault();
                }

                if (MyWishList != null)
                {
                    Information("Esta publicación ya esta en tus deseos.");
                    return RedirectToAction("ViewSale", "Sale", new { id = id });
                }

                var command = new CreateOrUpdateUserWishListCommand(0, DateTime.Now, true, "-", "-", user, sale);

                var result = commandBus.Submit(command);

                success = result.Success;

                if (!success)
                {
                    //   error = "No se pudo agregar la publicación  a tus deseos.";
                    Error("No se pudo agregar la publicación  a tus deseos.");
                    return RedirectToAction("ViewSale", "Sale", new { id = id });
                }

                Success("En tus deseos.");

            }
            catch (Exception ex)
            {
                success = false;
                error = ex.Message;
                Error("Ha ocurrido un error");
                FNHMVC.Web.Helpers.LoggerManager.WriteError(ex);
            }

            return RedirectToAction("ViewSale", "Sale", new { id = id });
            //return Json(new { error = error, success = success, id = SaleId }, JsonRequestBehavior.AllowGet);
        }

    }
}
