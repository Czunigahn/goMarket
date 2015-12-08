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
﻿using FNHMVC.Web.Core.Authentication;
﻿using FNHMVC.Web.Core.Extensions;
using FNHMVC.Web.Core.Models;
using FNHMVC.Web.ViewModels;

namespace FNHMVC.Web.Controllers
{
    [CompressResponse]
    [FNHMVCAuthorize(Roles.User, Roles.Admin)]
    public class UserAddressController : BootstrapBaseController
    {
        private readonly ICommandBus commandBus;
        private readonly IUserRepository userRepository;
        private readonly IUserAddressRepository userAddressRepository;
       

        public UserAddressController(ICommandBus commandBus,  IUserRepository userRepository, IUserAddressRepository userAddressRepository)
        {
            this.commandBus = commandBus;
            this.userRepository = userRepository;
            this.userAddressRepository = userAddressRepository;
        }

        public ActionResult Index()
        {
            User user = userRepository.GetById(HttpContext.User.GetFNHMVCUser().UserId);
            if (user == null)
            {
                Attention("Tu sesión ha caducado, vuelve a iniciar sesión.");
                return RedirectToAction("Login", "Account");
            }
            var userAddress = userAddressRepository.GetMany(x => x.User.UserId == HttpContext.User.GetFNHMVCUser().UserId);

            return View(userAddress);
        }

        public ActionResult Create()
        {
            User user = userRepository.GetById(HttpContext.User.GetFNHMVCUser().UserId);
            if (user == null)
            {
                Attention("Tu sesión ha caducado, vuelve a iniciar sesión.");
                return RedirectToAction("Login", "Account");
            }
            var viewModel = new UserAddressFormModel();
            viewModel.Activated = true;
            viewModel.FullName = user.FullName;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserAddressFormModel model)
        {
            if (ModelState.IsValid)
            {

                User user = userRepository.GetById(HttpContext.User.GetFNHMVCUser().UserId);

                var commandOriginal = new CreateOrUpdateUserAddressCommand
                {
                    UserAddressId = 0,
                    User = user,
                    FullName = model.FullName,
                    AddressLine1 = model.AddressLine1,
                    AddressLine2 = model.AddressLine2,
                    City = model.City,
                    State = model.State,
                    ZipCode = model.ZipCode,
                    Country = model.Country,
                    PhoneNumber = model.PhoneNumber,
                    Activated = true
                };

                var result = commandBus.Submit(commandOriginal);

                if (result.Success)
                {
                    Information("La direccion ha sido creada");
                    return RedirectToAction("Index", "UserAddress");
                }
                Attention("La direccion no se pudo crear");
            }
            return RedirectToAction("Index", "UserAddress");
        }

        public ActionResult Edit(long id)
        {
            var userAddress = userAddressRepository.GetById(id);

            var viewModel = new UserAddressFormModel(userAddress);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserAddressFormModel model)
        {
            if (ModelState.IsValid)
            {
                var originalUserAddress = userAddressRepository.GetById(model.UserAddressId);
                if(originalUserAddress == null)
                {
                    Attention("La direccion de usuario que intenta editar  ya no esta disponible.");
                    return RedirectToAction("Index", "UserAddress");
                }

                originalUserAddress.User = userRepository.GetById(HttpContext.User.GetFNHMVCUser().UserId);

                var commandOriginal = new CreateOrUpdateUserAddressCommand
                {
                    UserAddressId = model.UserAddressId,
                    User = userRepository.GetById(HttpContext.User.GetFNHMVCUser().UserId),
                    FullName = model.FullName,
                    AddressLine1 = model.AddressLine1,
                    AddressLine2 = model.AddressLine2,
                    City = model.City,
                    State = model.State,
                    ZipCode = model.ZipCode,
                    Country = model.Country,
                    PhoneNumber = model.PhoneNumber,
                    Activated = model.Activated
                };
                
                //var commandOriginal = new CreateOrUpdateUserAddressCommand(originalUserAddress);
                var result = commandBus.Submit(commandOriginal);

                if (result.Success)
                {
                    Success("La direccion de usuario ha sido editada");
                    return RedirectToAction("Index", "UserAddress");
                }
                Attention("La direccion de usuario no se pudo editar");

            }
            return RedirectToAction("Index", "UserAddress");
        }
    }
}
