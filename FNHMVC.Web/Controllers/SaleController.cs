 ﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
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
using FNHMVC.Web.ViewModels;
using Facebook;
using FlickrNet;
using System.Configuration;



namespace FNHMVC.Web.Controllers
{
    [CompressResponse]
    [FNHMVCAuthorize(Roles.User, Roles.Admin)]
    public class SaleController : BootstrapBaseController
    {
        private readonly ICommandBus commandBus;
        private readonly ICategoryRepository categoryRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly ISaleRepository saleRepository;
        private readonly IUserRepository userRepository;
        private readonly IGoodDealRepository goodDealRepository;
        private readonly ISalePendingChangeRepository salePendingChangeRepository;
        private readonly ICartRepository cartRepository;
        private readonly IFollowRepository followRepository;
        private readonly IUserReviewsRepository userReviewsRepository;

        public SaleController(ICommandBus commandBus, ICategoryRepository categoryRepository, IFollowRepository followRepository, ISalePendingChangeRepository salePendingChangeRepository, ISaleRepository saleRepository, IUserRepository userRepository, IGoodDealRepository goodDealRepository, ICartRepository cartRepository, IUserReviewsRepository userReviewsRepository, ITransactionRepository transactionRepository)
        {
            this.commandBus = commandBus;
            this.categoryRepository = categoryRepository;
            this.saleRepository = saleRepository;
            this.userRepository = userRepository;
            this.goodDealRepository = goodDealRepository;
            this.salePendingChangeRepository = salePendingChangeRepository;
            this.cartRepository = cartRepository;
            this.followRepository = followRepository;
            this.userReviewsRepository = userReviewsRepository;
            this.transactionRepository = transactionRepository;
        }

        public ActionResult Index(DateTime? startDate, DateTime? endDate)
        {
            User user = userRepository.GetById(HttpContext.User.GetFNHMVCUser().UserId);
            if (user == null)
            {
                Attention("Tu sesión ha caducado, vuelve a iniciar sesión.");
                return RedirectToAction("Login", "Account");
            }


            //If date is not passed, take current month's first and last dte 
            DateTime dtNow;
            dtNow = DateTime.Today;
            if (!startDate.HasValue)
            {
                startDate = new DateTime(dtNow.Year, dtNow.Month, 1);
                endDate = startDate.Value.AddMonths(1).AddDays(-1);
            }
            //take last date of start date's month, if end date is not passed 
            if (startDate.HasValue && !endDate.HasValue)
            {
                endDate = (new DateTime(startDate.Value.Year, startDate.Value.Month, 1)).AddMonths(1).AddDays(-1);
            }

            var sales = saleRepository.GetMany(x => x.Created >= startDate && x.Created <= endDate &&
                                               x.Activated &&
                                               x.SalePendingChange.Where(s => s.Activated == true).Count() <= 0 &&
                                               x.User.UserId == HttpContext.User.GetFNHMVCUser().UserId
                                               );

            //if request is Ajax will return partial view
            if (Request.IsAjaxRequest())
            {
                return PartialView("_SaleList", sales);
            }
            //set start date and end date to ViewBag dictionary
            ViewBag.StartDate = startDate.Value.ToShortDateString();
            ViewBag.EndDate = endDate.Value.ToShortDateString();
            //if request is not ajax
            return View(sales);
        }

        public ActionResult PendingChange()
        {
            var sales = saleRepository.GetMany(x =>
                                     x.Activated &&
                                     x.SalePendingChange.Where(s => s.Activated == true).Count() > 0 &&
                                     x.User.UserId == HttpContext.User.GetFNHMVCUser().UserId
                                     );

            return View(sales);

        }


        public ActionResult CreateFromExisting(long id = 0)
        {
            User user = userRepository.GetById(HttpContext.User.GetFNHMVCUser().UserId);
            if (user == null)
            {
                Attention("Tu sesión ha caducado, vuelve a iniciar sesión.");
                return RedirectToAction("Login", "Account");
            }

            var viewModel = new SaleFormModel();

            var sale = saleRepository.GetById(id);
            if (sale != null)
            {
                viewModel.Title = sale.Title;
                viewModel.Cost = sale.Cost;
                viewModel.Picture = sale.Picture;
                viewModel.YouTubeLink = sale.YouTubeLink;
                viewModel.Description = sale.Description;
                viewModel.Created = DateTime.Today;

                viewModel.SendToSuscriptors = user.Followers.Count > 0;
                ViewBag.HasSuscriptors = user.Followers.Count > 0 ? "1" : "0";

                var categories = categoryRepository.GetMany(x => x.Activated == true);
                viewModel.Categories = categories.ToSelectListItems(-1);

                return View(viewModel);
            }

            viewModel.SendToSuscriptors = user.Followers.Count > 0;
            ViewBag.HasSuscriptors = user.Followers.Count > 0 ? "1" : "0";

            var categories2 = categoryRepository.GetMany(x => x.Activated == true);
            viewModel.Categories = categories2.ToSelectListItems(-1);
            viewModel.Created = DateTime.Today;
            return View(viewModel);
        }

        public ActionResult Create()
        {
            User user = userRepository.GetById(HttpContext.User.GetFNHMVCUser().UserId);
            if (user == null)
            {
                Attention("Tu sesión ha caducado, vuelve a iniciar sesión.");
                return RedirectToAction("Login", "Account");
            }

            if (user.PaypalAccount.Equals(""))
            {
                Attention("Necesitas haber ingresado tú cuenta de paypal para postear una venta.");
                return RedirectToAction("MyAccount", "Account");
            }


            var viewModel = new SaleFormModel();

            viewModel.SendToSuscriptors = user.Followers.Count > 0;
            ViewBag.HasSuscriptors = user.Followers.Count > 0 ? "1" : "0";

            var categories = categoryRepository.GetMany(x => x.Activated == true);
            viewModel.Categories = categories.ToSelectListItems(-1);
            viewModel.Created = DateTime.Today;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(SaleFormModel form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Category category = categoryRepository.GetById(form.CategoryId);
                    User user = userRepository.GetById(HttpContext.User.GetFNHMVCUser().UserId);

                    if (user == null)
                    {
                        Attention("Tu sesión ha caducado, vuelve a iniciar sesión.");
                        return RedirectToAction("Login", "Account");
                    }

                    var date = form.SaleId == 0 ? DateTime.Now : form.Created;

                    //var originalSale = saleRepository.GetById(form.SaleId);

                    var command = new CreateOrUpdateSaleCommand
                    {
                        TookItHome = form.TookItHome,
                        Altitude = form.Altitude,
                        Latitude = form.Latitude,

                        Title = form.Title,
                        Activated = true,
                        Category = category,
                        User = user,
                        Cost = form.Cost,
                        Created = date,
                        Modified = DateTime.Now,
                        Description = form.Description,
                        Picture = form.Picture,
                        Quantity = form.Quantity,
                        SaleId = 0,
                        YouTubeLink = form.YouTubeLink,
                        ActiveForSales = true,//originalSale == null ? true : originalSale.Activated,
                        PendingChange = false,//originalSale == null ? false : originalSale.PendingChange,
                        DateFromDeal = DateTime.Now,
                        DateToDeal = DateTime.Now,
                        CostDeal = 0,
                        HasDeal = false,
                        DescriptionDeal = "--"


                    };




                    var result = commandBus.Submit(command);

                    ////Save pictures
                    //if (command.Picture != null && command.Picture.Trim().Length > 0 && result.Success)
                    //{
                    //    try
                    //    {
                    //        FileUploadManager images = new FileUploadManager();
                    //        var sale = saleRepository.GetById(command.SaleId);

                    //        var list = images.UploadImages(sale, command.Picture);

                    //        int itemCount = list.Count();
                    //        for (int i = 0; i < itemCount; i++)
                    //        {
                    //            var item = list[i];
                    //            var cmdImage = new CreateOrUpdateSaleImagesCommand(item, i == (itemCount - 1));
                    //            commandBus.Submit(cmdImage);
                    //        }


                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Error(ex.Message);
                    //    }
                    //}

                    //Post on Facebook
                    if (form.PostOnFacebook)
                    {
                        try
                        {
                            dynamic messagePost = new ExpandoObject();
                            messagePost.picture = form.Picture;
                            messagePost.link = "http://lexstore.apphb.com/";
                            messagePost.video = form.YouTubeLink;
                            messagePost.name = form.Title;

                            // "{*actor*} " + "posted news..."; //<---{*actor*} is the user (i.e.: Alex)
                            messagePost.caption = "Categoria: " + category.Name;
                            messagePost.description = form.Description;
                            messagePost.message = HttpContext.User.GetFNHMVCUser().Name + " ha realizado una publicación.";


                            if (HttpContext.Session["AccessToken"] != null)
                            {
                                string acccessToken = HttpContext.Session["AccessToken"].ToString();
                                var appp = new FacebookClient(acccessToken);
                                try
                                {
                                    var postId = appp.Post("me/feed?", messagePost);
                                }
                                catch (FacebookOAuthException ex)
                                {
                                    Error(ex.Message);
                                    //handle oauth exception
                                }
                                catch (FacebookApiException ex)
                                {
                                    Error(ex.Message);
                                    //handle facebook exception
                                }
                            }
                            else
                            {
                                Error("Necesitas Iniciar Session con facebook");
                            }
                        }
                        catch (Exception ex)
                        {
                            Error(ex.Message);
                            FNHMVC.Web.Helpers.LoggerManager.WriteError(ex);
                        }
                    }


                    //Send Email to followers
                    try
                    {
                        if (form.SendToSuscriptors)
                            if (user.Followers != null)
                                if (user.Followers.Count > 0)
                                    new MailController().SendEmailToClients(user, command).Deliver();
                    }
                    catch (Exception ex)
                    {
                        Error(ex.Message);
                        FNHMVC.Web.Helpers.LoggerManager.WriteError(ex);
                    }

                    if (result.Success)
                    {
                        return RedirectToAction("UploadImageModal", "UploadImages", new { id = command.SaleId });
                    }
                }

                Error("Error al crear la publicación");
                return View("Create", form);
            }
            catch (Exception ex)
            {
                FNHMVC.Web.Helpers.LoggerManager.WriteError(ex);
                return View("Create", form);
            }
        }

        public ActionResult SendMail(long id)
        {
            User user = userRepository.GetById(HttpContext.User.GetFNHMVCUser().UserId);

            if (user == null)
            {
                Attention("Tu sesión ha caducado, vuelve a iniciar sesión.");
                return RedirectToAction("Login", "Account");
            }

            var sale = saleRepository.GetById(id);

            if (sale.SalePendingChange.Where(x => x.Activated).Count() > 0)
            {
                Attention("Esta publicación tiene cambios pendientes.");
                return RedirectToAction("Index");
            }

            var command = new CreateOrUpdateSaleCommand(sale, false);


            try
            {
                if (user.Followers != null)
                    if (user.Followers.Count > 0)
                        new MailController().SendEmailToClients(user, command).Deliver();
            }
            catch (Exception ex)
            {
                Error("Ha ocurrido un error mientras se realizaba la petición.");
                FNHMVC.Web.Helpers.LoggerManager.WriteError(ex);
                return RedirectToAction("Index");
            }

            Information("Se ha reenviado la publicación a tus suscriptores.");

            return RedirectToAction("Index");
        }

        // GET: /Expense/Edit
        public ActionResult Edit(long id)
        {
            var sale = saleRepository.GetById(id);

            if (sale.SalePendingChange.Where(x => x.Activated).Count() > 0)
            {
                Attention("Esta publicación tiene cambios pendientes.");
                return RedirectToAction("Index");
            }

            var viewModel = new SaleFormUpdateDataModel(sale);
            var categories = categoryRepository.GetMany(x => x.Activated == true);
            viewModel.Categories = categories.ToSelectListItems(sale.Category.CategoryId);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SaleFormUpdateDataModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Category category = categoryRepository.GetById(model.CategoryId);


                    var originalSale = saleRepository.GetById(model.SaleId);

                    if (originalSale == null)
                    {
                        Attention("La publicación que intenta editar  ya no esta disponible.");
                        return RedirectToAction("Index", "Sale");
                    }

                    var date = model.SaleId == 0 ? DateTime.Now : originalSale.Created;



                    originalSale.Modified = DateTime.Now;
                    originalSale.PendingChange = true;

                    var commandOriginal = new CreateOrUpdateSaleCommand(originalSale, false);
                    var result = commandBus.Submit(commandOriginal);

                    if (!result.Success)
                    {
                        Attention("Ha ocurrido un error desconocido.");
                        return RedirectToAction("Index", "Sale");
                    }

                    var command = new CreateOrUpdateSalePendingChangeCommand
                    {
                        Title = model.Title,
                        Activated = true,
                        Category = category,
                        Cost = model.Cost,
                        Created = date,
                        Modified = DateTime.Now,
                        Description = model.Description,
                        Picture = model.Picture,
                        Quantity = model.Quantity,
                        YouTubeLink = model.YouTubeLink,
                        SalePendingChangeId = 0,
                        ReasonId = model.ReasonId,
                        ReasonDescription = model.ReasonDescription,
                        Sale = originalSale,
                        CommitAfterAccept = true,
                        Altitude = model.Altitude,
                        Latitude = model.Latitude,
                        TookItHome = model.TookItHome,

                    };


                    result = commandBus.Submit(command);

                    if (result.Success)
                    {
                        Information("La publicación ha sido modificada, cambios pendientes de aprobación.");
                        return RedirectToAction("Index", "Sale");
                    }

                    Attention("La edición no se pudo completar");
                }

                return RedirectToAction("Index", "Sale");
            }
            catch (Exception ex)
            {
                Error("Ha ocurrido un error.");
                FNHMVC.Web.Helpers.LoggerManager.WriteError(ex);
                return RedirectToAction("Index", "Sale");
            }
        }

        public ActionResult Delete(long id)
        {
            var sale = saleRepository.GetById(id);
            if (sale == null)
            {
                Attention("La venta no existe, o esta temporalmente deshabilitada");
                return RedirectToAction("Index");
            }
            return View(sale);
        }

        public ActionResult Disable(long id)
        {
            var sale = saleRepository.GetById(id);
            if (sale == null)
            {
                Attention("La venta no existe, o esta temporalmente deshabilitada");
                return RedirectToAction("Index");
            }

            bool ActiveForSales = sale.ActiveForSales;

            ViewBag.ActionToDo = ActiveForSales ? "deshabilitar" : "habilitar";

            return View(sale);
        }

        [HttpPost]
        public ActionResult Delete(Sale sale)
        {
            sale = saleRepository.GetById(sale.SaleId);
            var command = new DeleteSaleCommand(sale.SaleId, false, false, false);
            var result = commandBus.Submit(command);
            Information("La publicación ha sido eliminada.");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Disable(Sale sale)
        {
            sale = saleRepository.GetById(sale.SaleId);

            bool ActiveForSales = !sale.ActiveForSales;

            var command = new DeleteSaleCommand(sale.SaleId, sale.Activated, ActiveForSales, sale.PendingChange);

            var result = commandBus.Submit(command);

            if (ActiveForSales)
                Information("La publicación ha sido habilitada .");
            else
                Information("La publicación ha sido deshabilitada .");

            return RedirectToAction("Index");
        }


        public ActionResult ViewSale(long id = 0)
        {
            try
            {
                User user = userRepository.GetById(HttpContext.User.GetFNHMVCUser().UserId);

                if (user == null)
                {
                    Attention("Tu sesión ha caducado, vuelve a iniciar sesión.");
                    return RedirectToAction("Login", "Account");
                }

                var sale = saleRepository.GetById(id);
                if (sale == null)
                {
                    Error("La venta que desea ver no existe ó esta deshabilitada temporalmente.");
                    return RedirectToAction("Index", "Home");
                }




                #region Rate

                var rate = new RatingFormModel();

                //Obtener valoraciones de la venta
                var _UserReviews = sale.UserReviews.Where(x => x.Active && x.Sale.SaleId == sale.SaleId).ToList();

                rate.Active = true;
                rate.Comment = "";
                rate.Date = DateTime.Now;
                rate.ReviewId = 0;

                //estadisticas
                var totalReviewsCount = _UserReviews.Count(); //cuantos han comentado
                var totalRateValues = _UserReviews.Sum(x => x.Value); //que valor han dado

                var Start5Count = _UserReviews.Count(x => x.Value == 5);
                var Start4Count = _UserReviews.Count(x => x.Value == 4);
                var Start3Count = _UserReviews.Count(x => x.Value == 3);
                var Start2Count = _UserReviews.Count(x => x.Value == 2);
                var Start1Count = _UserReviews.Count(x => x.Value == 1);

                rate.SaleCount = totalRateValues;

                var div = Convert.ToDecimal((totalReviewsCount <= 0 ? 1 : totalReviewsCount));
                rate.SaleAvg = Decimal.Round(totalRateValues / div, 2, MidpointRounding.AwayFromZero); ;

                rate.Start5Count = Start5Count;
                rate.Start4Count = Start4Count;
                rate.Start3Count = Start3Count;
                rate.Start2Count = Start2Count;
                rate.Start1Count = Start1Count;

                rate.Start5Avg = Decimal.Round(((Start5Count / div) * 100), 2, MidpointRounding.AwayFromZero);//  totalRateValues / (Start5Count <= 0 ? 1 : Start5Count);
                rate.Start4Avg = Decimal.Round((Start4Count / div) * 100, 2, MidpointRounding.AwayFromZero); ;//  totalRateValues / (Start4Count <= 0 ? 1 : Start4Count);
                rate.Start3Avg = Decimal.Round((Start3Count / div) * 100, 2, MidpointRounding.AwayFromZero); ;// totalRateValues / (Start3Count <= 0 ? 1 : Start3Count);
                rate.Start2Avg = Decimal.Round((Start2Count / div) * 100, 2, MidpointRounding.AwayFromZero); ;//  totalRateValues / (Start2Count <= 0 ? 1 : Start2Count);
                rate.Start1Avg = Decimal.Round((Start1Count / div) * 100, 2, MidpointRounding.AwayFromZero); ;// totalRateValues / (Start1Count <= 0 ? 1 : Start1Count);

                rate.Title = "";
                rate.TitleSale = sale.Title;
                rate.Value = 0;


                var review = userReviewsRepository.GetMany(x => x.Sale.SaleId == sale.SaleId && x.User.UserId == user.UserId);

                UserReviews TheReview = null;
                if (review != null && review.Count() > 0)
                {
                    TheReview = review.FirstOrDefault();
                }

                rate.ReviewId = (TheReview == null ? 0 : TheReview.ReviewId);
                rate.SaleId = sale.SaleId;
                rate.UserReview = TheReview;

                #endregion

                var SimilarSales = new List<Sale>();

                var SimilarSalesFind = saleRepository.GetMany(
                                                              x => x.Category.CategoryId == sale.Category.CategoryId ||
                                                              x.Title.Contains(sale.Title) ||
                                                              x.Description.Contains(sale.Description)
                                                              && x.Activated
                                                              && x.ActiveForSales
                                                              && x.SaleId != sale.SaleId
                                                             ).Take(10);

                //if (SimilarSalesFind == null || SimilarSalesFind.Count() <= 0)
                //{
                //    SimilarSalesFind=saleRepository.GetMany(x=> x.Category.p
                //}

                if (SimilarSalesFind != null)
                    SimilarSales.AddRange(SimilarSalesFind);

                var model = new SaleFormModel(sale, rate, SimilarSales.ToList());

                ViewBag.Category = sale.Category.Name;
                ViewBag.UserReviews = sale.UserReviews;
                ViewBag.AccountID = sale.User.UserId;
                ViewBag.ClientsCount = sale.User.Followers == null ? 0 : sale.User.Followers.Count();
                ViewBag.Followers = sale.User.Followers;
                ViewBag.LastName = sale.User.LastName;
                ViewBag.FirstName = sale.User.FirstName;

                GoodDeal isAGoodDeal = null;
                var goodDeals = goodDealRepository.GetMany(x => x.Sale.SaleId == id && x.User.UserId == HttpContext.User.GetFNHMVCUser().UserId);

                if (goodDeals != null && goodDeals.Count() > 0)
                    isAGoodDeal = goodDeals.FirstOrDefault();



                ViewBag.GoodDealsCount = sale.GoodDeals.Count;
                var vone = (HttpContext.User.GetFNHMVCUser().UserId != id);
                var vtow = HttpContext.User.GetFNHMVCUser().IsAuthenticated;

                if (isAGoodDeal != null && (vone && vtow))
                {
                    ViewBag.ButtonText = "Buena oferta:" + sale.GoodDeals.Count;
                }
                else
                {
                    ViewBag.ButtonText = "Marcar como buena oferta: " + sale.GoodDeals.Count;
                }


                return View(model);
            }
            catch (Exception ex)
            {
                Error("Error al cargar la publicación");
                FNHMVC.Web.Helpers.LoggerManager.WriteError(ex);
                return View(new SaleFormModel());
            }
        }

        //[HttpGet]
        public ActionResult GoodDeal(long id)
        {
            try
            {
                var isAGoodDeal =
                    goodDealRepository.Get(
                        x => x.Sale.SaleId == id && x.User.UserId == HttpContext.User.GetFNHMVCUser().UserId);
                if (isAGoodDeal == null) //no ha marcado como buena oferta
                {
                    var sale = saleRepository.GetById(id);
                    var user = userRepository.GetById(HttpContext.User.GetFNHMVCUser().UserId);
                    // Crear comando
                    var goodDeal = new CreateGoodDealCommand(0, user, sale);
                    var result = commandBus.Submit(goodDeal);
                    if (result.Success)
                    {
                        Success("Has marcado esta publicación como buena oferta.");
                        return RedirectToAction("ViewSale", "Sale", new { id = id });
                    }
                    else
                    {
                        Error("Ha ocurrido un error desconocido.");
                        return RedirectToAction("ViewSale", "Sale", new { id = id });
                    }

                }

                Information("Anteriormente marcaste esta publiación como buena oferta.");
                return RedirectToAction("ViewSale", "Sale", new { id = id });
            }
            catch (Exception ex)
            {
                Error("Error al marcar como buena oferta");
                FNHMVC.Web.Helpers.LoggerManager.WriteError(ex);
                return RedirectToAction("ViewSale", "Sale", new { id = id });
            }
        }



        public JsonResult RateSale(long id, int rate, string title, string comment)
        {

            var SaleId = id;
            bool success = false;
            string error = "";

            try
            {

                var sale = saleRepository.GetById(id);
                if (sale == null)
                {
                    return Json(new { error = "La venta que desea ver no existe ó esta deshabilitada temporalmente.", success = false, id = SaleId }, JsonRequestBehavior.AllowGet);
                }

                User user = userRepository.GetById(HttpContext.User.GetFNHMVCUser().UserId);

                if (user == null)
                {
                    return Json(new { error = "Tu sesión ha caducado, vuelve a iniciar sesión.", success = false, id = SaleId }, JsonRequestBehavior.AllowGet);
                }

                var review = userReviewsRepository.GetMany(x => x.Sale.SaleId == sale.SaleId && x.User.UserId == user.UserId);

                UserReviews TheReview = null;
                if (review != null && review.Count() > 0)
                {
                    TheReview = review.FirstOrDefault();
                }


                var cmd = new CreateOrUpdateUserReviewsCommand
                {
                    Active = true,
                    Comment = comment,
                    Title = title,
                    Date = TheReview == null ? DateTime.Now : TheReview.Date,
                    ReviewId = TheReview == null ? 0 : TheReview.ReviewId,
                    Sale = sale,
                    User = user,
                    Value = rate,
                };

                var result = commandBus.Submit(cmd);

                error = "";
                success = result.Success;
            }

            catch (System.Exception ex)
            {
                if (ex.InnerException != null)
                    while (ex.InnerException != null)
                        ex = ex.InnerException;
                
                FNHMVC.Web.Helpers.LoggerManager.WriteError(ex);

                error = ex.Message;
            }

            return Json(new { error = error, success = success, id = SaleId }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Sold(DateTime? startDate, DateTime? endDate)
        {
            //If date is not passed, take current month's first and last dte 
            DateTime dtNow;
            dtNow = DateTime.Today;
            if (!startDate.HasValue)
            {
                startDate = new DateTime(dtNow.Year, dtNow.Month, 1);
                endDate = startDate.Value.AddMonths(1).AddDays(-1);
            }
            //take last date of start date's month, if end date is not passed 
            if (startDate.HasValue && !endDate.HasValue)
            {
                endDate = (new DateTime(startDate.Value.Year, startDate.Value.Month, 1)).AddMonths(1).AddDays(-1);
            }

            var transactions = transactionRepository.GetMany(x => x.Created >= startDate && x.Created <= endDate &&
                                               x.Seller.UserId == HttpContext.User.GetFNHMVCUser().UserId
                                               );

            //if request is Ajax will return partial view
            if (Request.IsAjaxRequest())
            {
                return PartialView("_SoldList", transactions);
            }
            //set start date and end date to ViewBag dictionary
            ViewBag.StartDate = startDate.Value.ToShortDateString();
            ViewBag.EndDate = endDate.Value.ToShortDateString();
            //if request is not ajax
            return View(transactions);
        }

        public ActionResult Purchased(DateTime? startDate, DateTime? endDate)
        {
            //If date is not passed, take current month's first and last dte 
            DateTime dtNow;
            dtNow = DateTime.Today;
            if (!startDate.HasValue)
            {
                startDate = new DateTime(dtNow.Year, dtNow.Month, 1);
                endDate = startDate.Value.AddMonths(1).AddDays(-1);
            }
            //take last date of start date's month, if end date is not passed 
            if (startDate.HasValue && !endDate.HasValue)
            {
                endDate = (new DateTime(startDate.Value.Year, startDate.Value.Month, 1)).AddMonths(1).AddDays(-1);
            }

            var transactions = transactionRepository.GetMany(x => x.Created >= startDate && x.Created <= endDate &&
                                               x.Buyer.UserId == HttpContext.User.GetFNHMVCUser().UserId
                                               );

            //if request is Ajax will return partial view
            if (Request.IsAjaxRequest())
            {
                return PartialView("_PurchasedList", transactions);
            }
            //set start date and end date to ViewBag dictionary
            ViewBag.StartDate = startDate.Value.ToShortDateString();
            ViewBag.EndDate = endDate.Value.ToShortDateString();
            //if request is not ajax
            return View(transactions);
        }

        #region  Deals

        public ActionResult Deals(DateTime? startDate, DateTime? endDate)
        {
            User user = userRepository.GetById(HttpContext.User.GetFNHMVCUser().UserId);
            if (user == null)
            {
                Attention("Tu sesión ha caducado, vuelve a iniciar sesión.");
                return RedirectToAction("Login", "Account");
            }

            var ImSuscriptTo = followRepository.GetMany(x => x.User.UserId == user.UserId);


            var ListSales = new List<Sale>();

            foreach (var follow in ImSuscriptTo)
            {
                var saleOfUser = follow.User.Sales.Where(x => x.Activated && x.ActiveForSales);
                ListSales.AddRange(saleOfUser);
            }

            //var sales = saleRepository.GetMany(x => x.CostDeal > 0 && x.DescriptionDeal.Trim() =="" &&
            //                                   x.Activated &&
            //                                   x.User.UserId == HttpContext.User.GetFNHMVCUser().UserId
            //                                   );

            //If date is not passed, take current month's first and last dte 
            DateTime dtNow;
            dtNow = DateTime.Today;
            if (!startDate.HasValue)
            {
                startDate = new DateTime(dtNow.Year, dtNow.Month, 1);
                endDate = startDate.Value.AddMonths(1).AddDays(-1);
            }
            //take last date of start date's month, if end date is not passed 
            if (startDate.HasValue && !endDate.HasValue)
            {
                endDate = (new DateTime(startDate.Value.Year, startDate.Value.Month, 1)).AddMonths(1).AddDays(-1);
            }

            var sales = saleRepository.GetMany(x => (x.DateFromDeal >= startDate || x.DateFromDeal <= endDate) &&
                                               (x.DateToDeal >= startDate || x.DateToDeal <= endDate) &&
                                               x.Activated &&
                                               x.CostDeal > 0 && x.DescriptionDeal.Trim() != "" &&
                                               x.SalePendingChange.Where(s => s.Activated == true).Count() <= 0 &&
                                               x.User.UserId == HttpContext.User.GetFNHMVCUser().UserId
                                               );

            //if request is Ajax will return partial view
            if (Request.IsAjaxRequest())
            {
                return PartialView("_SaleList", sales);
            }
            ViewBag.StartDate = startDate.Value.ToShortDateString();
            ViewBag.EndDate = endDate.Value.ToShortDateString();
            //if request is not ajax
            return View(sales);
        }

        public ActionResult CreateDeals(long id)
        {
            var sale = saleRepository.GetById(id);

            if (sale.SalePendingChange.Where(x => x.Activated).Count() > 0)
            {
                Attention("Esta publicación tiene cambios pendientes.");
                return RedirectToAction("Index");
            }

            var viewModel = new DealsFormModel(sale);
            viewModel.DateFromDeal = DateTime.Parse(viewModel.DateFromDeal.ToShortDateString());
            viewModel.DateToDeal = DateTime.Parse(viewModel.DateToDeal.ToShortDateString());
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDeals(DealsFormModel model)
        {
            if (ModelState.IsValid)
            {
                var originalSale = saleRepository.GetById(model.SaleId);

                if (originalSale == null)
                {
                    Attention("La publicación que intenta editar  ya no esta disponible.");
                    return RedirectToAction("Index", "Sale");
                }

                originalSale.DescriptionDeal = model.DescriptionDeal;
                originalSale.CostDeal = model.CostDeal;
                originalSale.DateFromDeal = model.DateFromDeal;
                originalSale.DateToDeal = model.DateToDeal;
                originalSale.HasDeal = true;

                var commandOriginal = new CreateOrUpdateSaleCommand(originalSale, true);
                var result = commandBus.Submit(commandOriginal);

                if (result.Success)
                {
                    Information("La oferta ha sido creada");
                    return RedirectToAction("Index", "Sale");
                }
                Attention("La oferta no se pudo crear");
            }
            return RedirectToAction("Index", "Sale");
        }


        public ActionResult EditDeals(long id)
        {
            var sale = saleRepository.GetById(id);

            if (sale.SalePendingChange.Where(x => x.Activated).Count() > 0)
            {
                Attention("Esta publicación tiene cambios pendientes.");
                return RedirectToAction("Index");
            }

            var viewModel = new DealsFormModel(sale);

            viewModel.CostDeal = sale.CostDeal;
            viewModel.DateFromDeal = sale.DateFromDeal;
            viewModel.DateToDeal = sale.DateToDeal;
            viewModel.DescriptionDeal = sale.DescriptionDeal;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDeals(DealsFormModel model)
        {
            if (ModelState.IsValid)
            {
                var originalSale = saleRepository.GetById(model.SaleId);

                if (originalSale == null)
                {
                    Attention("La publicación que intenta editar  ya no esta disponible.");
                    return RedirectToAction("Index", "Sale");
                }

                originalSale.DescriptionDeal = model.DescriptionDeal;
                originalSale.CostDeal = model.CostDeal;
                originalSale.DateFromDeal = model.DateFromDeal;
                originalSale.DateToDeal = model.DateToDeal;
                originalSale.HasDeal = model.HasDeal;

                var commandOriginal = new CreateOrUpdateSaleCommand(originalSale, true);
                var result = commandBus.Submit(commandOriginal);

                if (result.Success)
                {
                    Success("La oferta ha sido editada");
                    return RedirectToAction("Deals", "Sale");
                }
                Attention("La oferta no se pudo editar");
            }
            return RedirectToAction("Deals", "Sale");
        }



        public ActionResult DeleteDeals(long id)
        {
            var sale = saleRepository.GetById(id);
            if (sale == null)
            {
                Attention("La venta no existe, o esta temporalmente deshabilitada");
                return RedirectToAction("Index");
            }
            var viewModel = new DealsFormModel(sale);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult DeleteDeals(DealsFormModel model)
        {
            var originalSale = saleRepository.GetById(model.SaleId);

            if (originalSale == null)
            {
                Attention("La publicación que intenta editar ya no esta disponible.");
                return RedirectToAction("Index", "Sale");
            }

            originalSale.HasDeal = false;
            originalSale.DescriptionDeal = "--";
            originalSale.CostDeal = 0;

            var commandOriginal = new CreateOrUpdateSaleCommand(originalSale, true);
            var result = commandBus.Submit(commandOriginal);

            if (result.Success)
            {
                Information("La oferta ha sido eliminada");
                return RedirectToAction("Deals", "Sale");
            }

            return RedirectToAction("Index");
        }

        #endregion

        #region SpotLigth

        public ActionResult Spotlight(long id)
        {
            var sale = saleRepository.GetById(id);
            if (sale == null)
            {
                Attention("La venta no existe, o esta temporalmente deshabilitada");
                return RedirectToAction("Index");
            }

            bool Spotlight = sale.Spotlight;

            ViewBag.ActionToDo = Spotlight ? "no resaltar" : "resaltar";

            return View(sale);
        }

        [HttpPost]
        public ActionResult Spotlight(Sale sale)
        {
            if (ModelState.IsValid)
            {
                var originalSale = saleRepository.GetById(sale.SaleId);

                if (originalSale == null)
                {
                    Attention("La publicación que intenta editar  ya no esta disponible.");
                    return RedirectToAction("Index", "Sale");
                }

                originalSale.Spotlight = !originalSale.Spotlight;
                originalSale.SpotlightApprove = false;

                var commandOriginal = new CreateOrUpdateSaleCommand(originalSale, true);
                var result = commandBus.Submit(commandOriginal);

                if (!result.Success)
                {
                    Information("La publicación ha sido resaltada");
                    return RedirectToAction("Index", "Sale");
                }

                if (result.Success)
                {
                    Information("La publicación ha sido resaltada");
                    return RedirectToAction("Index", "Sale");
                }
                Attention("La publicación no se pudo resaltar");
            }
            return RedirectToAction("Index", "Sale");
        }

        #endregion


    }
}
