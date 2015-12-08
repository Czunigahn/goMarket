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
    public class CartController : BootstrapBaseController
    {
        private readonly ICommandBus commandBus;
        private readonly ICategoryRepository categoryRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly ISaleRepository saleRepository;
        private readonly IUserRepository userRepository;
        private readonly IUserAddressRepository userAddressRepository;
        private readonly ICartRepository cartRepository;
        private readonly IExpenseRepository expenseRepository;
        private readonly ICuponRepository cuponRepository;
        public CartController(ICommandBus commandBus, ICuponRepository cuponRepository, ITransactionRepository transactionRepository, IExpenseRepository expenseRepository, ICategoryRepository categoryRepository, ISaleRepository saleRepository, IUserRepository userRepository, IUserAddressRepository userAddressRepository, ICartRepository cartRepository)
        {
            this.cuponRepository = cuponRepository;
            this.expenseRepository = expenseRepository;
            this.commandBus = commandBus;
            this.categoryRepository = categoryRepository;
            this.transactionRepository = transactionRepository;
            this.saleRepository = saleRepository;
            this.userRepository = userRepository;
            this.userAddressRepository = userAddressRepository;
            this.cartRepository = cartRepository;
        }
        //
        // GET: /Sale/

        public ActionResult Index()
        {
            var cart = cartRepository.GetMany(x => x.User.UserId == HttpContext.User.GetFNHMVCUser().UserId);

            ViewBag.hasCoupon = false;
            foreach (var cart1 in cart)
            {
                if (cart1.Cupon != null)
                {
                    ViewBag.hasCoupon = true;
                    break;
                }
            }
            return View(cart);
        }

        [HttpPost]
        public ActionResult Index(CuponAddFormModel model)
        {
            var user = userRepository.GetById(HttpContext.User.GetFNHMVCUser().UserId);

            var cupon = cuponRepository.Get(x => x.CuponName.Equals(model.Name));

            var carts = cartRepository.GetMany(x => x.User == user);

            ViewBag.hasCoupon = false;
            foreach (var cart1 in carts)
            {
                if (cart1.Sale.User.Equals(cupon.User))
                {
                    cart1.Cupon = cupon;
                    cartRepository.Update(cart1, false);

                    cupon.TimesUsed += 1;
                    cuponRepository.Update(cupon);
                    break;
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult DeleteCart()
        {
            var cart = cartRepository.GetMany(x => x.User.UserId == HttpContext.User.GetFNHMVCUser().UserId);
            cartRepository.Delete(cart);

            Success("Cart eliminada!");

            return RedirectToAction("Index");
        }

        public ActionResult PayPayPal()
        {
            var cart = cartRepository.GetMany(x => x.User.UserId == HttpContext.User.GetFNHMVCUser().UserId);
            if (cart != null)
            {
                if (cart.Count() < 10)
                {
                    var request = WebRequest.Create("https://api-3t.sandbox.paypal.com/nvp");
                    request.Method = "POST";

                    double totalAmmount = 0;

                    string headerVars =
                        String.Format(
                            "USER={0}&PWD={1}&SIGNATURE={2}&METHOD={3}&VERSION={4}&RETURNURL={5}&CANCELURL={6}",
                            ConfigurationManager.AppSettings["BusinessAccountKey"], ConfigurationManager.AppSettings["BusinessPassword"],
                            ConfigurationManager.AppSettings["BusinessSignature"], "SetExpressCheckout", "93",
                            ConfigurationManager.AppSettings["ReturnURL"], ConfigurationManager.AppSettings["CancelURL"]);
                    int contador = 0;

                    ;
                    foreach (var singleCart in cart)
                    {
                        if (singleCart.Quantity <= singleCart.Sale.Quantity)
                        {

                            var itemAmmount = Math.Round((double)(singleCart.Sale.Cost * singleCart.Quantity), 2);
                            var taxPercentage = Math.Round(Convert.ToDouble(ConfigurationManager.AppSettings["BusinessTax"]), 2);
                            var tax = Math.Round((itemAmmount * taxPercentage), 2);
                            var ammount = Math.Round(itemAmmount + tax, 2);

                            var desc = singleCart.Sale.Title;
                            var detailDesc = singleCart.Sale.Description;
                            var paypalAccount = singleCart.Sale.User.PaypalAccount;
                            var requestId = singleCart.CartId;
                            var quantity = singleCart.Quantity;
                            var priceSingleitem = Math.Round((double)singleCart.Sale.Cost, 2);
                            if (singleCart.Cupon != null)
                            {
                                itemAmmount -= Math.Round((((double)singleCart.Sale.Cost) * singleCart.Quantity) * singleCart.Cupon.Discount, 2);
                                tax = Math.Round((itemAmmount * taxPercentage), 2); ;
                                ammount = Math.Round(itemAmmount + tax, 2);
                                desc = desc + " minus discount";
                                detailDesc = detailDesc + " minus discount";
                                priceSingleitem -= Math.Round(((double)singleCart.Sale.Cost * singleCart.Cupon.Discount), 2);

                            }
                            totalAmmount += ammount;


                            headerVars = headerVars.Insert(headerVars.Length,
                                                           String.Format(
                                                               "&PAYMENTREQUEST_{0}_CURRENCYCODE={1}&PAYMENTREQUEST_{0}_AMT={2}&PAYMENTREQUEST_{0}_ITEMAMT={3}&PAYMENTREQUEST_{0}_TAXAMT={4}&PAYMENTREQUEST_{0}_PAYMENTACTION={5}&PAYMENTREQUEST_{0}_DESC={6}&PAYMENTREQUEST_{0}_SELLERPAYPALACCOUNTID={7}&PAYMENTREQUEST_{0}_PAYMENTREQUESTID={8}&L_PAYMENTREQUEST_{0}_DESC0={9}&L_PAYMENTREQUEST_{0}_AMT0={10}&L_PAYMENTREQUEST_{0}_QTY0={11}&PAYMENTREQUEST_{0}_SHIPPINGAMT={12}",
                                                               contador, "USD", ammount, itemAmmount, tax, "Sale", desc,
                                                               paypalAccount,
                                                               requestId, detailDesc, priceSingleitem, quantity, 0));
                            ++contador;
                        }
                    }

                    request.ContentLength = headerVars.Length;

                    //SEND INFORMATION
                    using (var streamWriter = new StreamWriter(request.GetRequestStream(), ASCIIEncoding.ASCII)
                        )
                    {
                        streamWriter.Write(headerVars);
                        streamWriter.Close();
                    }

                    //RETRIEVE RESPONSE
                    string responseText;
                    using (var sr = new StreamReader(request.GetResponse().GetResponseStream()))
                    {
                        responseText = sr.ReadToEnd();
                    }
                    var values = responseText.Split("&".ToCharArray());
                    if (values.First(x => x.Contains("ACK")).Contains("Success"))
                    {
                        var token = HttpUtility.UrlDecode(values.First(x => x.Contains("TOKEN")).Substring(6));

                        var user = userRepository.GetById(HttpContext.User.GetFNHMVCUser().UserId);
                        // Crear comando
                        var expense = new CreateOrUpdateExpenseCommand();
                        expense.Created = DateTime.Now;
                        expense.CheckoutCompleted = false;
                        expense.Amount = totalAmmount;
                        expense.PayerID = "";
                        expense.Token = token;
                        expense.User = user;

                        var result = commandBus.Submit(expense);
                        if (result.Success)
                            return Redirect(String.Format("https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_express-checkout&token={0}", token));

                    }
                    Error("An error ocurred doing your transaction");
                }
                else
                {
                    Error("There cant be more than 10 Objects in a transaction");
                }
            }
            else
            {
                Error("You have zero items in your cart");
            }
            return RedirectToAction("Index");
        }

        public ActionResult SuccessPaypal(string token, string payerID)
        {
            var cart = cartRepository.GetMany(x => x.User.UserId == HttpContext.User.GetFNHMVCUser().UserId);
            var expense = expenseRepository.Get(x => x.User.UserId == HttpContext.User.GetFNHMVCUser().UserId && x.Token.Equals(token));
            var shippings = userAddressRepository.GetMany(x => x.User.UserId == HttpContext.User.GetFNHMVCUser().UserId);

            var shippinModel = new ShippingSelectionFormModel();
            shippinModel.Shippings = shippings.ToSelectListItems(-1);
            var checkoutmodel = new CompleteCheckout();
            checkoutmodel.Cart = cart;
            checkoutmodel.FormModel = shippinModel;
            ViewBag.TotalSale = expense.Amount;
            return View(checkoutmodel);
        }

        [HttpPost]
        public ActionResult SuccessPaypal(CompleteCheckout shipping, string token, string payerID)
        {
            var cart = cartRepository.GetMany(x => x.User.UserId == HttpContext.User.GetFNHMVCUser().UserId);
            var user = userRepository.GetById(HttpContext.User.GetFNHMVCUser().UserId);

            var expense = expenseRepository.Get(x => x.User.UserId == HttpContext.User.GetFNHMVCUser().UserId && x.Token.Equals(token) && x.CheckoutCompleted == false);
            if (expense != null)
            {
                var request = WebRequest.Create("https://api-3t.sandbox.paypal.com/nvp");
                request.Method = "POST";

                string headerVars = String.Format("USER={0}&PWD={1}&SIGNATURE={2}&METHOD={3}&VERSION={4}&TOKEN={5}&PAYERID={6}",
                            ConfigurationManager.AppSettings["BusinessAccountKey"], ConfigurationManager.AppSettings["BusinessPassword"],
                            ConfigurationManager.AppSettings["BusinessSignature"], "DoExpressCheckoutPayment ", "93", token, payerID);
                int contador = 0;
                foreach (var singleCart in cart)
                {
                    var ammount = Math.Round((double)(singleCart.Sale.Cost * singleCart.Quantity), 2);
                    ammount = Math.Round(ammount + (ammount * Convert.ToDouble(ConfigurationManager.AppSettings["BusinessTax"])), 2);
                    var paypalAccount = singleCart.Sale.User.PaypalAccount;
                    var requestId = singleCart.CartId;

                    if (singleCart.Cupon != null)
                    {

                        ammount -= Math.Round((((double)singleCart.Sale.Cost) * singleCart.Quantity) * singleCart.Cupon.Discount, 2);
                    }

                    headerVars = headerVars.Insert(headerVars.Length,
                                                   String.Format(
                                                       "&PAYMENTREQUEST_{0}_CURRENCYCODE={1}&PAYMENTREQUEST_{0}_AMT={2}&PAYMENTREQUEST_{0}_SELLERPAYPALACCOUNTID={3}&PAYMENTREQUEST_{0}_PAYMENTREQUESTID={4}",
                                                       contador, "USD", ammount, paypalAccount, requestId));
                    ++contador;
                }


                request.ContentLength = headerVars.Length;

                //SEND INFORMATION
                using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream(), ASCIIEncoding.ASCII))
                {
                    streamWriter.Write(headerVars);
                    streamWriter.Close();
                }

                //RETRIEVE RESPONSE
                string responseText = String.Empty;
                using (StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream()))
                {
                    responseText = sr.ReadToEnd();
                }
                var values = responseText.Split("&".ToCharArray());
                if (values.First(x => x.Contains("ACK")).Contains("Success"))
                {


                    expense.PayerID = payerID;
                    expense.CheckoutCompleted = true;
                    expense.Shipping = shipping.FormModel.ShippingAdress;
                    expenseRepository.Update(expense);


                    foreach (var singleCart in cart)
                    {
                        var sale = singleCart.Sale;
                        sale.Quantity = sale.Quantity - singleCart.Quantity;
                        saleRepository.Update(sale, false);


                        var ammount = (double)(singleCart.Sale.Cost * singleCart.Quantity);

                        var transaction = new Transaction
                        {
                            Buyer = user,
                            Created = DateTime.Now,
                            Expense = expense,
                            Sale = sale,
                            Quantity = singleCart.Quantity,
                            Seller = sale.User,
                            TotalPrice = ammount,
                            TotalTax =
                                (ammount *
                                 Convert.ToDouble(ConfigurationManager.AppSettings["BusinessTax"]))
                        };
                        transactionRepository.Add(transaction);

                    }



                    cartRepository.Delete(cart);

                    //TODO Mandar Email de comprobación
                    new MailController().SendUserPurchaseCompleted(user).Deliver();
                    Success("Se completo la compra, gracias por preferirnos!");


                }
                else
                {
                    Error("A ocurrido un error por favor volver a intentarlo!");
                }
            }

            return RedirectToAction("Index");
        }



        [HttpPost]
        public ActionResult AddToCart(CartFormModel id)
        {
            var alreadyOnCart = cartRepository.Get(x => x.Sale.SaleId == id.SaleId && x.User.UserId == HttpContext.User.GetFNHMVCUser().UserId);

            if (alreadyOnCart == null)
            {
                var sale = saleRepository.GetById(id.SaleId);
                if (sale.Quantity > id.Quantity)
                {
                    var user = userRepository.GetById(HttpContext.User.GetFNHMVCUser().UserId);
                    // Crear comando
                    var cart = new CreateOrUpdateCartCommand(0, user, sale, id.Quantity, DateTime.Now, null);
                    var result = commandBus.Submit(cart);
                    if (result.Success)
                    {
                        Success("You added this sale to your cart!");
                        return RedirectToAction("ViewSale", "Sale", new { id = id.SaleId });
                    }
                    else
                    {
                        Error("An error occurred");
                        return RedirectToAction("ViewSale", "Sale", new { id = id.SaleId });
                    }
                }
                else
                {
                    Error("That amount of product does not exist in the inventory");
                    return RedirectToAction("ViewSale", "Sale", new { id = id.SaleId });
                }
            }
            else
                Information("Already on Cart!");

            return RedirectToAction("ViewSale", "Sale", new { id = id.SaleId });
        }

        public ActionResult Delete(long id)
        {
            var cart = cartRepository.Get(x => x.CartId == id && x.User.UserId == HttpContext.User.GetFNHMVCUser().UserId);
            if (cart == null)
            {
                Attention("La venta no existe, o esta temporalmente deshabilitada");
            }
            else
            {
                var command = new DeleteCartCommand();
                command.CartId = cart.CartId;
                var result = commandBus.Submit(command);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(Cart cart, long id)
        {
            var command = new CreateOrUpdateCartCommand(cart);
            var result = commandBus.Submit(command);

            return RedirectToAction("Index");
        }
    }
}