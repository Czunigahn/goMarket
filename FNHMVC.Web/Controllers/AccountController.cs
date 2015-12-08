using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using BootstrapMvcSample.Controllers;
using FNHMVC.Domain.Commands.User;
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
using Facebook;

namespace FNHMVC.Web.Controllers
{
    public class AccountController : BootstrapBaseController
    {
        private readonly ICommandBus commandBus;
        private readonly IUserRepository userRepository;
        private readonly ITokenRepository tokenRepository;
        private readonly IFollowRepository followRepository;
        private readonly IFormsAuthentication formAuthentication;
        private readonly IUserInboxRepository userInboxRepository;
        private readonly ICuponRepository cuponRepository;

        public AccountController(ICommandBus commandBus, ICuponRepository cuponRepository, IUserInboxRepository userInboxRepository, ITokenRepository tokenRepository, IUserRepository userRepository, IFormsAuthentication formAuthentication, IFollowRepository followRepository)
        {
            this.commandBus = commandBus;
            this.cuponRepository = cuponRepository;
            this.tokenRepository = tokenRepository;
            this.userRepository = userRepository;
            this.followRepository = followRepository;
            this.formAuthentication = formAuthentication;
            this.userInboxRepository = userInboxRepository;
        }

        #region Facebook login

        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        public ActionResult Facebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = "550175918354041",
                client_secret = "060e40aa8703ead6aea05849f42b55ee",
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email,publish_stream,offline_access" // Add other permissions as needed
            });

            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = "550175918354041",
                client_secret = "060e40aa8703ead6aea05849f42b55ee",
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });

            var accessToken = result.access_token;

            // Store the access token in the session
            HttpContext.Session["AccessToken"] = accessToken;

            // update the facebook client with the access token so 
            // we can make requests on behalf of the user
            fb.AccessToken = accessToken;
            // Get the user's information
            dynamic me = fb.Get("me?fields=first_name,last_name,id,email,picture");
            string email = me.email;
            User user = userRepository.Get(u => u.Email == email);
            if (user != null)
            {
                //inicio Session y guardo los datos del proveedor (facebook)
                //Registrar Proveedor
                var command = new UpdateUserCommand()
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Activated = user.Activated,
                    Age = user.Age,
                    Country = user.Country,
                    LastLoginTime = DateTime.Now,
                };
                var commandResult = commandBus.Submit(command);
                if (commandResult.Success)
                {
                    formAuthentication.SetAuthCookie(this.HttpContext, UserAuthenticationTicketBuilder.CreateAuthenticationTicket(user));
                }
                else
                {
                    Error("Ha ocurrido un error, por favor inténtalo de nuevo más tarde.");
                }

            }
            else
            {
                //Registrar usuario,proveedor, e iniciar sesion 
                //Registrar usuario con password automatica y activo
                //Enviar correo al usuario con su nueva password

                // Get the user's information
                string firstName = me.first_name;
                string lastName = me.last_name;
                string providerUserId = me.id;
                string password = CreatePassword(10);
                var command = new UserRegisterCommand
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Password = password,
                    Activated = true,
                    RoleId = (Int32)UserRoles.User,
                    Age = 0,
                    Genre = true,
                    Country = "",
                };
                var commandResult = commandBus.Submit(command);
                if (commandResult.Success)
                {
                    user = userRepository.Get(x => x.Email == command.Email && x.Activated);
                    var tokencommand = new UpdateUserCommand()
                    {
                        UserId = user.UserId,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Activated = user.Activated,
                        Age = user.Age,
                        Country = user.Country,
                        LastLoginTime = DateTime.Now,
                    };
                    var tokencommandResult = commandBus.Submit(tokencommand);
                    if (tokencommandResult.Success)
                    {
                        user.LastName = password;
                        new MailController().SendUserPassword(user).Deliver();
                        formAuthentication.SetAuthCookie(this.HttpContext,
                                                         UserAuthenticationTicketBuilder.CreateAuthenticationTicket(
                                                             user));
                        Success(" ¡Enhorabuena! Te has registrado correctamente en goMarket");
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region login/register/password

        private string CreatePassword(int length)
        {
            string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            string res = "";
            Random rnd = new Random();
            while (0 < length--)
                res += valid[rnd.Next(valid.Length)];
            return res;
        }

        //
        // GET: /Account/Logout

        public ActionResult Logout()
        {
            formAuthentication.Signout();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return ContextDependentView();
        }

        private bool ValidatePassword(User user, string password)
        {
            var encoded = Md5Encrypt.Md5EncryptPassword(password);
            return user.PasswordHash.Equals(encoded);
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return ContextDependentView();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LogOnFormModel form, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User user = userRepository.Get(u => u.Email == form.UserName && u.Activated == true);
                if (user != null)
                {
                    if (ValidatePassword(user, form.Password))
                    {
                        var command = new UpdateUserCommand()
                        {
                            UserId = user.UserId,
                            Email = user.Email,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Activated = user.Activated,
                            Age = user.Age,
                            PaypalAccount = user.PaypalAccount, //edit
                            Country = user.Country,
                            LastLoginTime = DateTime.Now,
                        };
                        var commandResult = commandBus.Submit(command);
                        if (commandResult.Success)
                        {
                            formAuthentication.SetAuthCookie(this.HttpContext, UserAuthenticationTicketBuilder.CreateAuthenticationTicket(user));
                            if (returnUrl != null)
                                return Redirect(returnUrl);
                            else
                                return RedirectToAction("Index", "Home");
                        }
                    }
                }
                Error("El nombre de usuario o la contraseña facilitada no es correcta.");
            }

            // If we got this far, something failed
            return View();
        }

        [HttpPost]
        public JsonResult JsonLogin(LogOnFormModel form, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User user = userRepository.Get(u => u.Email == form.UserName && u.Activated == true);
                if (user != null)
                {
                    if (ValidatePassword(user, form.Password))
                    {
                        var command = new UpdateUserCommand()
                        {
                            UserId = user.UserId,
                            Email = user.Email,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Activated = user.Activated,
                            Age = user.Age,
                            PaypalAccount = user.PaypalAccount, //edit
                            Country = user.Country,
                            LastLoginTime = DateTime.Now,
                        };
                        var commandResult = commandBus.Submit(command);
                        if (commandResult.Success)
                        {
                            formAuthentication.SetAuthCookie(this.HttpContext, UserAuthenticationTicketBuilder.CreateAuthenticationTicket(user));
                            return Json(new { success = true, redirect = returnUrl });
                        }
                        else
                        {
                            Error("Ha ocurrido un error desconocido.");
                        }
                    }
                }
            }
            Error("El nombre de usuario o la contraseña facilitada no es correcta.");
            // If we got this far, something failed
            return Json(new { errors = GetErrorsFromModelState() });
        }
        //
        // POST: /Account/JsonRegister

        [AllowAnonymous]
        [HttpPost]
        public ActionResult JsonRegister(UserFormModel form)
        {
            if (ModelState.IsValid)
            {
                var builder = form.Email + Guid.NewGuid().ToString() + form.LastName;
                var command = new UserRegisterCommand
                {
                    FirstName = form.FirstName,
                    LastName = form.LastName,
                    Email = form.Email,
                    Password = form.Password,
                    Activated = true,
                    PaypalAccount = form.PaypalAccount,
                    RoleId = (Int32)UserRoles.User,
                    Age = form.Age,
                    Genre = form.Genre,
                    Country = form.Country
                };
                IEnumerable<ValidationResult> errors = commandBus.Validate(command);
                ModelState.AddModelErrors(errors);
                if (ModelState.IsValid)
                {
                    var result = commandBus.Submit(command);
                    if (result.Success)
                    {
                        User user = userRepository.Get(u => u.Email == form.Email);
                        if (user != null)
                        {
                            var token = user.Email + Guid.NewGuid().ToString() + DateTime.Now;
                            var tokenCommand = new CreateOrUpdateTokenCommand()
                            {
                                TokenId = 0,
                                UserId = user.UserId,
                                ConfirmationToken = token,
                                Action = (int)MailOperationType.RegisterAccount,
                                Activated = true
                            };
                            var tokenResult = commandBus.Submit(tokenCommand);
                            if (tokenResult.Success)
                            {
                                var tokenMail = new TokenVerificationMail
                                {
                                    Action = tokenCommand.Action,
                                    Activated = tokenCommand.Activated,
                                    ConfirmationToken = tokenCommand.ConfirmationToken,
                                    Email = user.Email,
                                    UserId = user.UserId
                                };
                                new MailController().VerificationEmail(tokenMail).Deliver();
                                Information("Fue enviado un email a su cuenta de correo con los pasos para restablecer su Password.");
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        formAuthentication.SetAuthCookie(this.HttpContext,
                                                          UserAuthenticationTicketBuilder.CreateAuthenticationTicket(
                                                              user));
                        return Json(new { success = true });
                    }
                    else
                    {
                        Error("Ha ocurrido un error desconocido.");
                    }
                }
                // If we got this far, something failed
                return Json(new { errors = GetErrorsFromModelState() });
            }

            // If we got this far, something failed
            return Json(new { errors = GetErrorsFromModelState() });
        }

        // POST: /Account/Register

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(UserFormModel model)
        {
            if (ModelState.IsValid)
            {

                var builder = model.Email + Guid.NewGuid().ToString() + model.LastName;

                var command = new UserRegisterCommand
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password,
                    Activated = false,
                    PaypalAccount = model.PaypalAccount,
                    RoleId = (Int32)UserRoles.User,
                    Age = model.Age,
                    Genre = model.Genre,
                    Country = model.Country
                };
                IEnumerable<ValidationResult> errors = commandBus.Validate(command);
                ModelState.AddModelErrors(errors);
                if (ModelState.IsValid)
                {
                    var result = commandBus.Submit(command);
                    if (result.Success)
                    {
                        User user = userRepository.Get(u => u.Email == model.Email);
                        if (user != null)
                        {
                            var token = user.Email + Guid.NewGuid().ToString() + DateTime.Now;
                            var tokenCommand = new CreateOrUpdateTokenCommand()
                            {
                                TokenId = 0,
                                UserId = user.UserId,
                                ConfirmationToken = token,
                                Action = (int)MailOperationType.RegisterAccount,
                                Activated = true
                            };
                            errors = commandBus.Validate(tokenCommand);
                            ModelState.AddModelErrors(errors);
                            if (ModelState.IsValid)
                            {
                                var tokenResult = commandBus.Submit(tokenCommand);
                                if (tokenResult.Success)
                                {

                                    //SendEmailVerificationCommand
                                    var tokenMail = new TokenVerificationMail
                                    {
                                        Action = tokenCommand.Action,
                                        Activated = tokenCommand.Activated,
                                        ConfirmationToken = tokenCommand.ConfirmationToken,
                                        Email = user.Email,
                                        UserId = user.UserId
                                    };
                                    new MailController().VerificationEmail(tokenMail).Deliver();
                                    Success("¡Te has registrado correctamente en goMarket!");
                                    Information("Un mensaje ha sido enviado a tu correo electronico para confirmar tu cuenta.");
                                    return RedirectToAction("Index", "Home");
                                }
                            }
                        }
                    }
                    Error("Ha ocurrido un error desconocido.");
                }
                // If we got this far, something failed, redisplay form
                return View(model);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult Confirm(string confirmationToken = null, string userId = null)
        {
            int _userId = Convert.ToInt32(userId);
            if (_userId != 0 && confirmationToken != null)
            {

                Token token = tokenRepository.Get(x => x.ConfirmationToken == confirmationToken && x.UserId == _userId);
                if (token != null)
                {
                    if (token.Action == (int)MailOperationType.ResetPassword)
                    {
                        ResetPasswordModel reset = new ResetPasswordModel();
                        reset.UserId = token.UserId;
                        reset.ConfirmationToken = token.ConfirmationToken;
                        Information("Resert Password");
                        return View("ResetPassword", reset);
                    }
                    else if (token.Action == (int)MailOperationType.RegisterAccount)
                    {
                        User user = userRepository.Get(u => u.UserId == _userId);
                        if (user != null)
                        {
                            var command = new UpdateUserCommand()
                            {
                                UserId = user.UserId,
                                Email = user.Email,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                Activated = true,
                                Age = user.Age,
                                PaypalAccount = user.PaypalAccount, //edit
                                Country = user.Country,
                                LastLoginTime = DateTime.Now,
                            };
                            var commandResult = commandBus.Submit(command);
                            if (commandResult.Success)
                            {
                                formAuthentication.SetAuthCookie(this.HttpContext, UserAuthenticationTicketBuilder.CreateAuthenticationTicket(user));
                                return RedirectToAction("Index", "Home");
                            }

                        }

                    }
                }
            }
            Error("Su solicitud no es valida o ha expirado.");
            return RedirectToAction("Index", "Home");
        }

        private IEnumerable<string> GetErrorsFromModelState()
        {
            return ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
        }

        private ActionResult ContextDependentView()
        {
            string actionName = ControllerContext.RouteData.GetRequiredString("action");
            if (Request.QueryString["content"] != null)
            {
                ViewBag.FormAction = "Json" + actionName;
                return PartialView();
            }
            else
            {
                ViewBag.FormAction = actionName;
                return View();
            }
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordFormModel form)
        {
            if (ModelState.IsValid)
            {
                FNHMVCUser user = HttpContext.User.GetFNHMVCUser();
                var command = new ChangePasswordCommand
                {
                    UserId = user.UserId,
                    OldPassword = form.OldPassword,
                    NewPassword = form.NewPassword
                };
                IEnumerable<ValidationResult> errors = commandBus.Validate(command);
                ModelState.AddModelErrors(errors);
                if (ModelState.IsValid)
                {
                    var result = commandBus.Submit(command);
                    if (result.Success)
                    {
                        return RedirectToAction("ChangePasswordSuccess");
                    }
                    else
                    {
                        Error("La contraseña actual es incorrecta o la nueva contraseña no es válida.");
                    }
                }
            }
            // If we got this far, something failed, redisplay form
            return View(form);
        }

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View(new ForgotPasswordModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ForgotPassword(ForgotPasswordModel forgotPassword)
        {
            if (ModelState.IsValid)
            {
                var user =
                    userRepository.Get(x => x.Email == forgotPassword.Email && x.Activated == true);
                if (user != null)
                {
                    var token = user.Email + Guid.NewGuid().ToString() + DateTime.Now;
                    var command = new CreateOrUpdateTokenCommand()
                    {
                        TokenId = 0,
                        UserId = user.UserId,
                        ConfirmationToken = token,
                        Action = (int)MailOperationType.ResetPassword,
                        Activated = true
                    };
                    var result = commandBus.Submit(command);
                    if (result.Success)
                    {
                        var tokenMail = new TokenResetPasswordMail()
                        {
                            Action = command.Action,
                            Activated = command.Activated,
                            ConfirmationToken = command.ConfirmationToken,
                            Email = user.Email,
                            UserId = command.UserId
                        };
                        new MailController().ResetPasswordEmail(tokenMail).Deliver();
                        Information("Hemos enviado un mensaje a su cuenta de correo con los pasos para restablecer su contraseña.");
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            Error("Los datos especificados son incorrectos o su cuenta de usuario ha sido dehabilitada temporalmente.");
            return View(forgotPassword);
        }

        public ActionResult ValidateForgotPassword(string email)
        {
            var user = userRepository.Get(x => x.Email == email && x.Activated == true);
            if (user != null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ResetPassword(ResetPasswordModel reset)
        {
            return View(reset);
        }

        [HttpPost]
        public ActionResult SaveResetPassword(ResetPasswordModel reset)
        {
            if (ModelState.IsValid)
            {
                User user = userRepository.GetById(reset.UserId);
                var command = new ResetPasswordCommand()
                {
                    UserId = user.UserId,
                    NewPassword = reset.NewPassword
                };
                var result = commandBus.Submit(command);
                if (result.Success)
                {
                    Token token = tokenRepository.Get(x => x.ConfirmationToken == reset.ConfirmationToken);
                    var tokenCommand = new CreateOrUpdateTokenCommand()
                    {
                        Action = token.Action,
                        Activated = false,
                        ConfirmationToken = token.ConfirmationToken,
                        TokenId = token.TokenId,
                        UserId = token.UserId
                    };
                    // var tokenResult = commandBus.Submit(tokenCommand);
                    formAuthentication.SetAuthCookie(this.HttpContext, UserAuthenticationTicketBuilder.CreateAuthenticationTicket(user));
                    return RedirectToAction("Index", "Home");
                }
                Error("An error Occurred.");

            }
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Social/Follows

        public ActionResult Followers(long id = 0)
        {
            if (id <= 0)
                id = HttpContext.User.GetFNHMVCUser().UserId;

            var user = userRepository.GetById(id);

            if (user == null)
            {
                Error("El usuario ya no esta disponible.");
                return RedirectToAction("Index", "Home");
            }


            var list = new List<Follow>();
            var followers = user.Followers;

            if (followers != null)
                list = followers.ToList<Follow>();

            return View(list);
        }

        public ActionResult Follow(long id)
        {
            if (HttpContext.User.GetFNHMVCUser().UserId == id)
            {
                Attention("No puedes suscribirte a ti mismo.");
                return RedirectToAction("UserProfile", "Home", id);
            }

            var isFollow =
                followRepository.Get(
                    x => x.Follower.UserId == HttpContext.User.GetFNHMVCUser().UserId && x.User.UserId == id);
            if (isFollow == null) //no esta siendo seguido
            {
                var followingUser = userRepository.GetById(id);
                var followerUser = userRepository.GetById(HttpContext.User.GetFNHMVCUser().UserId);
                // Crear comando
                var follow = new CreateFollowCommand(0, followerUser, followingUser);
                var result = commandBus.Submit(follow);
                if (result.Success)
                    Success("Te has suscrito a las publicaciones de " + followingUser.FirstName);
                else
                    Error("Ocurrio un error al tratar de realizar la suscripción.");
                //Guardar follow
            }
            else
            {
                var follow = new DeleteFollowCommand() { FollowId = isFollow.FollowId };
                var result = commandBus.Submit(follow);
                if (result.Success)
                    Attention("Tu suscripción a " + isFollow.User.FirstName + " ha sido cancelada.");
                //Eliminar follow
            }

            return RedirectToAction("UserProfile", "Home", new { id = id });
        }

        #endregion

        #region Profile

        public ActionResult MyAccount()
        {
            ViewBag.AccountId = HttpContext.User.GetFNHMVCUser().UserId;
            return View();
        }

        public ActionResult MyProfile()
        {
            User user = userRepository.GetById(HttpContext.User.GetFNHMVCUser().UserId);

            if (user == null)
            {
                Attention("Tu sesión ha caducado, vuelve a iniciar sesión.");
                return RedirectToAction("Login", "Account");
            }

            var model = new UserEditFormModel(user);
            return View(model);
        }

        [HttpPost]
        public ActionResult MyProfile(UserEditFormModel model)
        {
            User user = userRepository.GetById(HttpContext.User.GetFNHMVCUser().UserId);

            var command = new CreateOrUpdateUserCommand()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = user.Email,
                Activated = user.Activated,
                PaypalAccount = model.PaypalAccount,
                RoleId = (Int32)UserRoles.User,
                Age = model.Age,
                Genre = model.Genre,
                Country = model.Country,
                Picture = model.Picture,
                Rate = user.Rate,
                About = model.About,
                UserId = HttpContext.User.GetFNHMVCUser().UserId,
                LastLoginTime = user.LastLoginTime,
                DateCreated = user.DateCreated,
                PasswordHash = user.PasswordHash
            };
            var result = commandBus.Submit(command);
            if (result.Success)
            {
                Success("Se edito correctamente su cuenta");
                if (model.ForgotPassword)
                {
                    return View("ForgotPassword", new ForgotPasswordModel { Email = user.Email });
                }
                return View(model);
            }
            //var model = new UserEditFormModel(user);
            return View(model);
        }

        #endregion

        #region Contact to Seller/Messages

        [HttpPost]
        public ActionResult Contact(UserContactModel model)
        {

            User user = userRepository.GetById(HttpContext.User.GetFNHMVCUser().UserId);

            if (user == null)
            {
                Attention("Tu sesión ha caducado, vuelve a iniciar sesión.");
                return RedirectToAction("UserProfile", "Home", new { id = model.UserIdToContact });
            }

            var seller = userRepository.GetById(model.UserIdToContact);

            var inbox = new CreateOrUpdateUserInboxCommand(0, DateTime.Now, DateTime.Now, false, true, model.SentEmail, model.Subject, model.Message, "", user, seller);
            inbox.LastMessage = "";

            var result = commandBus.Submit(inbox);

            Information("Tu mensaje ha sido enviado.");

            if (model.SentEmail)
                new MailController().SendEmailToSeller(inbox).Deliver();

            return RedirectToAction("UserProfile", "Home", new { id = model.UserIdToContact });

        }

        public ActionResult Inbox()
        {
            User user = userRepository.GetById(HttpContext.User.GetFNHMVCUser().UserId);

            if (user == null)
            {
                Attention("Tu sesión ha caducado, vuelve a iniciar sesión.");
                return RedirectToAction("Login", "Account");
            }

            var list = userInboxRepository.GetMany(x => x.Seller.UserId == user.UserId && x.Activated).ToList();
            //var list = user.UserInbox.Where(x => x.Seller.UserId == user.UserId).ToList();//.OrderBy(x => x.DateCreate).OrderByDescending(x => x.WasRead);

            return View(list.AsEnumerable());
        }

        public ActionResult ReadMessage(long id)
        {
            var inbox = userInboxRepository.GetById(id);
            inbox.WasRead = true;
            inbox.DateRead = DateTime.Now;

            var cmd = new CreateOrUpdateUserInboxCommand(inbox);

            commandBus.Submit(cmd);

            ViewBag.TheMessage = inbox.Message;
            ViewBag.TheSubject = inbox.Subject;

            inbox.Subject = inbox.Subject.Contains("Re:") ? inbox.Subject : "Re:" + inbox.Subject;
            inbox.Message = "";
            return View(inbox);
        }

        [HttpPost]
        public ActionResult ReplyMessage(UserInbox inbox)
        {


            var id = inbox.UserInboxId;

            var inboxRead = userInboxRepository.GetById(inbox.UserInboxId);

            //saved data
            inbox.Activated = inboxRead.Activated;
            inbox.DateCreate = inboxRead.DateCreate;
            inbox.DateRead = inboxRead.DateRead;
            inbox.LastMessage = inboxRead.LastMessage;

            //change user contact
            inbox.Seller = inboxRead.User;
            inbox.User = inboxRead.Seller;

            //user data
            inbox.Subject = inbox.Subject;
            inbox.Message = inbox.Message;
            inbox.LastMessage = inbox.Message + Environment.NewLine + "..." + inboxRead.LastMessage;
            inbox.SentEmail = inbox.SentEmail;

            //new record
            inbox.UserInboxId = 0;

            //no read
            inbox.WasRead = false;


            var cmd = new CreateOrUpdateUserInboxCommand(inbox);
            cmd.UserInboxId = 0;

            var result = commandBus.Submit(cmd);

            if (inbox.SentEmail)
                new MailController().SendEmailToSeller(cmd).Deliver();

            if (result.Success)
                Information("El mensaje ha sido enviado");
            else
            {
                Error("Ha ocurrido un error.");
                return ReadMessage(id);
            }

            return RedirectToAction("Inbox", "Account");
        }

        public ActionResult DeleteMessage(long id)
        {
            var inbox = userInboxRepository.GetById(id);

            return View(inbox);
        }

        [HttpPost]
        public ActionResult DeleteMessage(UserInbox inbox)
        {

            var cmd = new DeleteUserInboxCommand();

            cmd.UserInboxId = inbox.UserInboxId;

            var result = commandBus.Submit(cmd);

            if (result.Success)
                Information("El mensaje ha sido eliminado");
            else
            {
                Error("El mensaje no pudo ser eliminado.");
            }

            return RedirectToAction("Inbox", "Account");

        }

        #endregion

        #region Cupon

        public ActionResult Cupons()
        {
            var cupons = cuponRepository.GetMany(x => x.User.UserId == HttpContext.User.GetFNHMVCUser().UserId);

            return View(cupons);
        }

        public ActionResult ActivateCupon(long id)
        {
            var cupon = cuponRepository.GetById(id);

            if (cupon == null)
            {
                Attention("No de encontro el cupón con ese id.");

            }
            else
            {
                cupon.IsActive = !cupon.IsActive;
                cuponRepository.Update(cupon);
                Success("Se activo/desactivo correctamente");
            }

            return RedirectToAction("Cupons");
        }

        public ActionResult CreateCupon()
        {
            var model = new CuponCreateFormModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateCupon(CuponCreateFormModel model)
        {
            User user = userRepository.GetById(HttpContext.User.GetFNHMVCUser().UserId);

            var command = new CreateOrUpdateCuponCommand()
            {
                CuponId = 0,
                Created = DateTime.Now,
                CuponName = model.Name,
                Discount = model.Discount,
                IsActive = model.Activado,
                TimesUsed = 0,
                User = user
            };
            var result = commandBus.Submit(command);
            if (result.Success)
            {
                Success("Se creo el cupón");
            }

            return RedirectToAction("Cupons");
        }

        #endregion Cupon
    }
}


