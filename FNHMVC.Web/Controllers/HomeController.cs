using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BootstrapMvcSample.Controllers;
using FNHMVC.Model;
using FNHMVC.Web.ViewModels;
using FNHMVC.Domain.Commands;
using FNHMVC.Core.Common;
using FNHMVC.Web.Core.Extensions;
using FNHMVC.CommandProcessor.Dispatcher;
using FNHMVC.Data.Repositories;
using FNHMVC.Web.Core.ActionFilters;
using System;

namespace FNHMVC.Web.Controllers
{
    public class HomeController : BootstrapBaseController
    {
        private readonly ICommandBus commandBus;
        private readonly ISaleRepository saleRepository;
        private readonly IUserRepository userRepository;
        private readonly IFollowRepository followRepository;
        private readonly ICategoryRepository categoryRepository;

        public HomeController(ICommandBus commandBus, ICategoryRepository categoryRepository, ISaleRepository saleRepository, IUserRepository userRepository, IFollowRepository followRepository)
        {
            this.commandBus = commandBus;
            this.saleRepository = saleRepository;
            this.userRepository = userRepository;
            this.followRepository = followRepository;
            this.categoryRepository = categoryRepository;
        }

        public ActionResult AsyncCall()
        {
            //Async Runner Example
            //commandBus.AsyncRun<FNHMVC.Web.Core.Common.WCComunicator>(c => c.DoSomething());
            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            try
            {
                Session["LoadedSales"] = 0;
                //Obtengo el use
                var user = userRepository.GetById(HttpContext.User.GetFNHMVCUser().UserId);
                //Obtengo las personas que estoy siguiendo
                var follows = followRepository.GetMany(x => x.Follower == user);
                var salesList = new List<Sale>();
                if (follows.Any())
                {

                    foreach (var follow in follows)
                    {
                        var saleOfUser = follow.User.Sales.Where(x => x.Activated == true &&
                                                      x.ActiveForSales &&
                            // x.SalePendingChange.Where(s => s.Activated == true).Count() <= 0 &&
                                                      x.PendingChange == false).OrderByDescending(x => x.GoodDeals.Count()).ToList();
                        salesList.AddRange(saleOfUser);
                    }

                }
                else
                {
                    salesList = saleRepository.GetMany(x => x.Activated == true &&
                                                      x.ActiveForSales &&
                                                      x.PendingChange == false).ToList();
                }
                var sales = salesList.OrderByDescending(x => x.Created).Take(20);
                if (!sales.Any())
                {
                    ViewBag.sales = new List<Sale>();
                }
                else
                {
                    ViewBag.sales = sales;
                }
                Session["LoadedSales"] = sales.ToList().Count();
                var listOfCategories = new List<CategoryListFormModel>();
                var categories = categoryRepository.GetMany(x => x.Activated == true && x.Parent == null).Take(10);
                foreach (var category in categories)
                {
                    Category parent = category;
                    var childs = categoryRepository.GetMany(x => x.Parent == parent && x.Activated == true).Take(15).ToList();
                    var item = new CategoryListFormModel(category, childs);
                    listOfCategories.Add(item);
                }
                return View(listOfCategories);
            }
            catch (Exception ex)
            {

                FNHMVC.Web.Helpers.LoggerManager.WriteError(ex);
                return View(new List<CategoryListFormModel>());
            }
        }

        public ActionResult Search()
        {
            try
            {
                Session["LoadedSales"] = 0;

                var sales = saleRepository.GetMany(x => x.Activated == true).Take(20);
                if (sales.Any())
                {
                    ViewBag.sales = sales;
                }
                else
                {
                    Information("No existen ventas actualmente");
                    ViewBag.sales = new List<Sale>();
                }
                Sale sale = saleRepository.GetMany(x => x.Activated && x.ActiveForSales && x.PendingChange == false).OrderByDescending(x => x.Cost).First();
                var search = new SearchFormModel();
                search.AdvanceSearch = true;
                search.EndPrice = sale.Cost;
                sale = saleRepository.GetMany(x => x.Activated && x.ActiveForSales && x.PendingChange == false).OrderByDescending(x => x.GoodDeals.Count).First();
                search.EndGoodDeal = sale.GoodDeals.Count;
                sale = saleRepository.GetMany(x => x.Activated && x.ActiveForSales && x.PendingChange == false).OrderByDescending(x => x.Created).First();
                search.EndDate = sale.Created.Date;
                sale = saleRepository.GetMany(x => x.Activated && x.ActiveForSales && x.PendingChange == false).OrderBy(x => x.Created).First();
                search.StartDate = sale.Created.Date;
                return View(search);
            }
            catch (Exception ex)
            {
                FNHMVC.Web.Helpers.LoggerManager.WriteError(ex);
                return View(new SearchFormModel());
            }
        }

        [HttpPost]
        public ActionResult Search(SearchFormModel search)
        {
            try
            {
                List<Sale> searchOfSales;
                if (search.AdvanceSearch)
                {
                    searchOfSales = saleRepository.GetMany(x => x.Activated == true && (x.GoodDeals.Count() >= search.StartGoodDeal && x.GoodDeals.Count <= search.EndGoodDeal) && (x.Cost >= (decimal)search.StartPrice && x.Cost <= (decimal)search.EndPrice) && (x.Created >= search.StartDate && x.Created <= search.EndDate)).Skip(0).Take(20).ToList();
                    if (search.GeneralFilter != null)
                    {
                        searchOfSales = searchOfSales.Where(x => x.Title.Contains(search.GeneralFilter) || x.Description.Contains(search.GeneralFilter) || x.User.FirstName.Contains(search.GeneralFilter) || x.User.LastName.Contains(search.GeneralFilter) || x.Category.Name.Contains(search.GeneralFilter) || x.Category.Description.Contains(search.GeneralFilter)).ToList();
                    }
                }
                else
                {
                    searchOfSales = saleRepository.GetMany(x => x.Title.Contains(search.GeneralFilter) || x.Description.Contains(search.GeneralFilter) || x.User.FirstName.Contains(search.GeneralFilter) || x.User.LastName.Contains(search.GeneralFilter) || x.Category.Name.Contains(search.GeneralFilter) || x.Category.Description.Contains(search.GeneralFilter)).ToList();
                }

                ViewBag.sales = searchOfSales;
                if (!searchOfSales.Any())
                {
                    ViewBag.sales = new List<Sale>();
                }
                //if(searchOfSales!=null)
                //{
                //    ViewBag.sales = searchOfSales;    
                //}
                //else
                //{
                //    Information("No existen ventas actualmente");
                //    ViewBag.sales = new List<Sale>();
                //}

                Sale sale = saleRepository.GetMany(x => x.Activated && x.ActiveForSales && x.PendingChange == false).OrderByDescending(x => x.Cost).First();
                var s = new SearchFormModel();
                s.AdvanceSearch = true;
                s.EndPrice = sale.Cost;
                sale = saleRepository.GetMany(x => x.Activated && x.ActiveForSales && x.PendingChange == false).OrderByDescending(x => x.GoodDeals.Count).First();
                s.EndGoodDeal = sale.GoodDeals.Count;
                sale = saleRepository.GetMany(x => x.Activated && x.ActiveForSales && x.PendingChange == false).OrderByDescending(x => x.Created).First();
                search.EndDate = sale.Created.Date;
                sale = saleRepository.GetMany(x => x.Activated && x.ActiveForSales && x.PendingChange == false).OrderBy(x => x.Created).First();
                search.StartDate = sale.Created.Date;
                return View(s);
            }
            catch (Exception ex)
            {
                FNHMVC.Web.Helpers.LoggerManager.WriteError(ex);
                return View(new SearchFormModel());
            }

        }



        public ActionResult GetSales()
        {
            try
            {
                var user = userRepository.GetById(HttpContext.User.GetFNHMVCUser().UserId);
                //Obtengo las personas que estoy siguiendo
                var follows = followRepository.GetMany(x => x.Follower == user);
                var salesList = new List<Sale>();
                if (follows.Any())
                {

                    foreach (var follow in follows)
                    {
                        var saleOfUser = follow.User.Sales.Where(x => x.Activated == true).OrderByDescending(x => x.GoodDeals.Count()).ToList();
                        salesList.AddRange(saleOfUser);
                    }

                }
                else
                {
                    salesList = saleRepository.GetMany(x => x.Activated == true &&
                                                      x.ActiveForSales &&
                                                      x.PendingChange == false).ToList();
                }

                if (Session["LoadedSales"] == null)
                    Session["LoadedSales"] = 0;

                var sales = salesList.OrderByDescending(x => x.Created).Skip((int)Session["LoadedSales"]).Take(20);
                ViewBag.sales = sales;
                int cant = (int)Session["LoadedSales"];
                Session["LoadedSales"] = sales.ToList().Count() + cant;
                return View();
            }
            catch (Exception ex)
            {
                FNHMVC.Web.Helpers.LoggerManager.WriteError(ex);
                return View();
            }
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult UserProfile(long id = 0)
        {
            if (id <= 0)
                id = HttpContext.User.GetFNHMVCUser().UserId;

            var user = userRepository.GetById(id);
            var isFollow =
                followRepository.Get(
                    x => x.Follower.UserId == HttpContext.User.GetFNHMVCUser().UserId && x.User.UserId == id);
            if (user != null)
            {
                ViewBag.user = user;
                ViewBag.UserID = HttpContext.User.GetFNHMVCUser().UserId;
                ViewBag.AccountID = user.UserId;
                ViewBag.ClientsCount = user.Followers == null ? 0 : user.Followers.Count();
                ViewBag.Followers = user.Followers;
                ViewBag.LastName = user.LastName;
                ViewBag.FirstName = user.FirstName;
                ViewBag.ShowButton = false;
                ViewBag.IFollowing = "0";

                var vone = (HttpContext.User.GetFNHMVCUser().UserId != id);
                var vtow = HttpContext.User.GetFNHMVCUser().IsAuthenticated;
                if (vone && vtow)
                {
                    ViewBag.ShowButton = true;
                    if (isFollow != null)
                    {
                        ViewBag.IFollowing = "1";
                    }
                    else
                    {
                        ViewBag.IFollowing = "0";
                    }
                }
                else
                {
                    ViewBag.ShowButton = false;
                }

                return View(id);
            }

            Error("El perfil no esta disponible.");
            return RedirectToAction("Index");
        }

    }
}
