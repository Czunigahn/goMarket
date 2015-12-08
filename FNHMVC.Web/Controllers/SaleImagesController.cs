using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BootstrapMvcSample.Controllers;
using FNHMVC.CommandProcessor.Dispatcher;
using FNHMVC.Data.Repositories;
using FNHMVC.Domain.Commands;
using FNHMVC.Web.Helpers;

namespace FNHMVC.Web.Controllers
{
    public class SaleImagesController : BootstrapBaseController
    {
        private readonly ICommandBus commandBus;
        private readonly ISaleImagesRepository saleImagesRepository;
        private readonly ISaleRepository saleRepository;

        public SaleImagesController(ICommandBus commandBus, ISaleImagesRepository saleImagesRepository, ISaleRepository saleRepository)
        {
            this.commandBus = commandBus;
            this.saleImagesRepository = saleImagesRepository;
            this.saleRepository = saleRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Remove(long id)
        {
            var saleImage = saleImagesRepository.GetById(id);

            if (saleImage == null)
            {
                Error("La imagen ya no existe");
                return RedirectToAction("Index", "Sale");
            }

            DeleteSaleImagesCommand cmd = new DeleteSaleImagesCommand(id);

            var result = commandBus.Submit(cmd);

            if (!result.Success)
            {
                Error("La imagen no se pudo eliminar");
            }
            else
            {
                Success("La imagen ha sido eliminada.");
            }
            return RedirectToAction("UploadImageModal", "UploadImages", new { id = saleImage.Sale.SaleId });
        }

        public ActionResult ActivateBanner(long id)
        {

            var saleImage = saleImagesRepository.GetById(id);

            if (saleImage == null)
            {
                Error("La imagen ya no existe");
                return RedirectToAction("Index", "Sale");
            }

            var active = !saleImage.Activated;

            if (active)
            {
                var count = saleImagesRepository.GetMany(x => x.Activated &&
                                                         x.Sale == saleImage.Sale &&
                                                         x.Type == (int)SaleImageType.Banner
                                                        ).Count();

                if (count >= 5)
                {
                    Error("Solamente puede mantener 5 banners activos, desactive algunos si deseas cambiarlos.");
                    return RedirectToAction("UploadImageModal", "UploadImages", new { id = saleImage.Sale.SaleId });
                }
            }

            saleImage.Activated = active;

            var cmd = new CreateOrUpdateSaleImagesCommand(saleImage, true);

            var result = commandBus.Submit(cmd);

            if (!result.Success)
            {
                Error("El banner no ha podido ser desactivado.");
            }
            else
            {
                Success("El banner ha sido desactivado.");
            }

            return RedirectToAction("UploadImageModal", "UploadImages", new { id = saleImage.Sale.SaleId });
        }

        public ActionResult SetDefault(long id)
        {
            var saleImage = saleImagesRepository.GetById(id);

            if (saleImage == null)
            {
                Error("La imagen ya no existe");
                return RedirectToAction("Index", "Sale");
            }

            var filePath = "";

            if (saleImage.Url.Contains("http://") || saleImage.Url.Contains("https://"))
            {
                filePath = saleImage.Url;
            }
            else
            {
                filePath = "~/Content/img/upload/" + System.IO.Path.GetFileName(saleImage.Url);
            }

            var sale = saleImage.Sale;

            sale.Picture = filePath;

            var cmd = new CreateOrUpdateSaleCommand(sale, true);

            var result = commandBus.Submit(cmd);

            if (!result.Success)
            {
                Error("La imagen no se pudo establecer como predeterminada.");
            }
            else
            {
                Success("La imagen ha sido establecida como predeterminada para la publicación.");
            }
            return RedirectToAction("UploadImageModal", "UploadImages", new { id = saleImage.Sale.SaleId });
        }

    }
}
