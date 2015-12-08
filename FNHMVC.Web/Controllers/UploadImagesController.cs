using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FancyImageUploader.Models;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Net;
using BootstrapMvcSample.Controllers;
using FNHMVC.Web.Core.ActionFilters;
using FNHMVC.Web.Core.Models;
using FNHMVC.CommandProcessor.Dispatcher;
using FNHMVC.Data.Repositories;
using FNHMVC.Model;
using FNHMVC.Web.Helpers;
using FNHMVC.Domain.Commands;

namespace FancyImageUploader.Controllers
{
    [FNHMVCAuthorize(Roles.User, Roles.Admin)]
    public class UploadImagesController : BootstrapBaseController
    {

        private readonly ICommandBus commandBus;
        private readonly ISaleImagesRepository saleImagesRepository;
        private readonly ISaleRepository saleRepository;

        public UploadImagesController(ICommandBus commandBus, ISaleImagesRepository saleImagesRepository, ISaleRepository saleRepository)
        {
            this.commandBus = commandBus;
            this.saleImagesRepository = saleImagesRepository;
            this.saleRepository = saleRepository;
        }

        [HttpPost]
        public ActionResult PreviewImage()
        {
            var bytes = new byte[0];
            ViewBag.Mime = "image/png";

            if (Request.Files.Count == 1)
            {
                bytes = new byte[Request.Files[0].ContentLength];
                Request.Files[0].InputStream.Read(bytes, 0, bytes.Length);
                ViewBag.Mime = Request.Files[0].ContentType;
            }

            ViewBag.Message = Convert.ToBase64String(bytes, Base64FormattingOptions.InsertLineBreaks);
            return PartialView();
        }


        public ActionResult UploadImageModal(long id)
        {
            var imagesUrl = new ImagesModel();

            var SaleBanners = new List<SaleImages>();
            var SaleImgs = new List<SaleImages>();

            var saleImages = saleImagesRepository.GetMany(x =>
                // (x.Activated || (!x.Activated && x.Type == (int)SaleImageType.Banner)) && 
                                                                x.Sale.SaleId == id
                                                         );

            foreach (var item in saleImages)
            {
                if (item.Type == (int)SaleImageType.Sale)
                {
                    SaleImgs.Add(item);
                }

                if (item.Type == (int)SaleImageType.Banner)
                {
                    SaleBanners.Add(item);
                }

            }

            imagesUrl.SaleImages = SaleImgs;
            imagesUrl.SaleImagesBanner = SaleBanners;
            imagesUrl.Sale = saleRepository.GetById(id);

            return View(imagesUrl);
        }

        public ActionResult UploadImage(long id, int type)
        {
            var _UploadImageModel = new UploadImageModel();
            _UploadImageModel.SaleId = id;
            _UploadImageModel.SaleImageType = type;

            ViewBag.SaleId = id;
            //Just to distinguish between ajax request (for: modal dialog) and normal request
            if (Request.IsAjaxRequest())
            {
                return PartialView(_UploadImageModel);
            }

            return View(_UploadImageModel);
        }

        //
        // POST: /Home/UploadImage

        [HttpPost]
        public ActionResult UploadImage(UploadImageModel model)
        {
            try
            {
                //Check if all simple data annotations are valid
                if (ModelState.IsValid)
                {
                    //Prepare the needed variables
                    Bitmap original = null;
                    var name = "newimagefile";
                    var errorField = string.Empty;

                    if (model.IsUrl)
                    {
                        errorField = "Url";
                        name = GetUrlFileName(model.Url);
                        original = GetImageFromUrl(model.Url);
                    }
                    else if (model.IsFlickr)
                    {
                        errorField = "Flickr";
                        name = GetUrlFileName(model.Flickr);
                        original = GetImageFromUrl(model.Flickr);
                    }
                    else if (model.File != null) // model.IsFile
                    {
                        errorField = "File";
                        name = Path.GetFileNameWithoutExtension(model.File.FileName);
                        original = Bitmap.FromStream(model.File.InputStream) as Bitmap;
                    }

                    //If we had success so far
                    if (original != null)
                    {
                        var img = CreateImage(original, model.X, model.Y, model.Width, model.Height);

                        //Demo purposes only - save image in the file system
                        var fn = Server.MapPath("~/Content/img/upload/" + name + ".png");
                        img.Save(fn, System.Drawing.Imaging.ImageFormat.Png);


                        FileUploadManager images = new FileUploadManager();
                        var sale = saleRepository.GetById(model.SaleId);

                        var list = images.UploadImages(sale, (SaleImageType)model.SaleImageType, fn);
                        int itemCount = list.Count();
                        bool updateSale = false;

                        for (int i = 0; i < itemCount; i++)
                        {
                            var item = list[i];
                            var cmdImage = new CreateOrUpdateSaleImagesCommand(item, i == (itemCount - 1));
                            cmdImage.Activated = model.SaleImageType == (int)SaleImageType.Sale;
                            commandBus.Submit(cmdImage);

                            if ((sale.Picture == null || sale.Picture.Trim().Length <= 0) && !updateSale)
                            {
                                sale.Picture = item.Url;
                                updateSale = true;
                            }
                        }

                        try
                        {
                            if (updateSale)
                            {

                                var cmdSale = new CreateOrUpdateSaleCommand(sale, true);
                                var result = commandBus.Submit(cmdSale);
                                if (result.Success)
                                {
                                    Information("Se ha establecido esta imagen como predeterminada.");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            FNHMVC.Web.Helpers.LoggerManager.WriteError(ex);
                        }
                        //Redirect to index
                        return RedirectToAction("UploadImageModal", new { id = model.SaleId });
                    }
                    else //Otherwise we add an error and return to the (previous) view with the model data
                        ModelState.AddModelError(errorField, "Su archivo no parece válido. Por favor, inténtelo nuevamente utilizando sólo imágenes correctas!");
                }
            }
            catch (Exception ex)
            {
                Error("Error al subir la imagen.");
                FNHMVC.Web.Helpers.LoggerManager.WriteError(ex);
            }
            return View(model);
        }

        /// <summary>
        /// Gets an image from the specified URL.
        /// </summary>
        /// <param name="url">The URL containing an image.</param>
        /// <returns>The image as a bitmap.</returns>
        Bitmap GetImageFromUrl(string url)
        {
            var buffer = 1024;
            Bitmap image = null;

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                return image;

            using (var ms = new MemoryStream())
            {
                var req = WebRequest.Create(url);

                using (var resp = req.GetResponse())
                {
                    using (var stream = resp.GetResponseStream())
                    {
                        var bytes = new byte[buffer];
                        var n = 0;

                        while ((n = stream.Read(bytes, 0, buffer)) != 0)
                            ms.Write(bytes, 0, n);
                    }
                }

                image = Bitmap.FromStream(ms) as Bitmap;
            }

            return image;
        }

        /// <summary>
        /// Gets the filename that is placed under a certain URL.
        /// </summary>
        /// <param name="url">The URL which should be investigated for a file name.</param>
        /// <returns>The file name.</returns>
        string GetUrlFileName(string url)
        {
            var parts = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var last = parts[parts.Length - 1];
            return Path.GetFileNameWithoutExtension(last);
        }

        /// <summary>
        /// Creates a small image out of a larger image.
        /// </summary>
        /// <param name="original">The original image which should be cropped (will remain untouched).</param>
        /// <param name="x">The value where to start on the x axis.</param>
        /// <param name="y">The value where to start on the y axis.</param>
        /// <param name="width">The width of the final image.</param>
        /// <param name="height">The height of the final image.</param>
        /// <returns>The cropped image.</returns>
        Bitmap CreateImage(Bitmap original, int x, int y, int width, int height)
        {
            var img = new Bitmap(width, height);

            using (var g = Graphics.FromImage(img))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(original, new Rectangle(0, 0, width, height), x, y, width, height, GraphicsUnit.Pixel);
            }

            return img;
        }
    }
}
