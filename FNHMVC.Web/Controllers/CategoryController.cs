using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BootstrapMvcSample.Controllers;
using FNHMVC.Model;
using FNHMVC.Web.Helpers;
using FNHMVC.Web.ViewModels;
using FNHMVC.Domain.Commands;
using FNHMVC.Core.Common;
using FNHMVC.Web.Core.Extensions;
using FNHMVC.CommandProcessor.Dispatcher;
using FNHMVC.Data.Repositories;
using FNHMVC.Web.Core.ActionFilters;

namespace FNHMVC.Web.Controllers
{
    [CompressResponse]
    public class CategoryController : BootstrapBaseController
    {
        private readonly ICommandBus commandBus;
        private readonly ICategoryRepository categoryRepository;

        public CategoryController(ICommandBus commandBus, ICategoryRepository categoryRepository)
        {
            this.commandBus = commandBus;
            this.categoryRepository = categoryRepository;
        }

        public ActionResult Index()
        {
            var categories = categoryRepository.GetMany(x => x.Activated == true);
            return View(categories);
        }

        public ActionResult Details(long id)
        {
            return View();
        }

        public ActionResult Create()
        {
            var viewModel = new CategoryFormModel();
            var categories = categoryRepository.GetMany(x => x.Parent == null && x.Activated == true);
            var noparent = new Category();
            noparent.Name = "--No Parent--";
            noparent.CategoryId = 0;
            var parents = categories.ToList();
            parents.Add(noparent);
            viewModel.ParentCategories = parents.ToSelectListItems(-1);
            viewModel.Activated = true;
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var category = categoryRepository.GetById(id);
            var categories = categoryRepository.GetMany(x => x.Parent == null && x.Activated == true && x.CategoryId != id);
            var viewModel = new CategoryFormModel(category);
            var noparent = new Category();
            noparent.Name = "--No Parent--";
            noparent.CategoryId = 0;
            var parents = categories.ToList();
            parents.Add(noparent);
            viewModel.ParentCategories = parents.ToSelectListItems(-1);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(CategoryFormModel form)
        {
            if (ModelState.IsValid)
            {
                var parent = categoryRepository.GetById(form.ParentId);
                var command = new CreateOrUpdateCategoryCommand(form.CategoryId, form.Name, form.Description, form.Activated, parent);
                IEnumerable<ValidationResult> errors = commandBus.Validate(command);
                ModelState.AddModelErrors(errors);
                if (ModelState.IsValid)
                {
                    var result = commandBus.Submit(command);
                    if (result.Success) return RedirectToAction("Index");
                }
            }

            if (form.CategoryId == 0)
                return View("Create", form);
            else
                return View("Edit", form);
        }

        [AllowAnonymous]
        public ActionResult Delete(long id)
        {
            var category = categoryRepository.GetById(id);
            if (category == null)
            {
                Attention("La categoria no existe, o esta temporalmente deshabilitada");
                return RedirectToAction("Index");
            }

            if (category.Sales != null)
            {
                if (category.Sales.Count() > 0)
                {
                    Attention("La categoria no puede ser eliminada porque tiene publicaciones realizadas.");
                    return RedirectToAction("Index");
                }
            }


            return View(category);
        }

        [HttpPost]
        public ActionResult Delete(Category category)
        {
            //var command = new DeleteCategoryCommand { CategoryId = id };
            //var result = commandBus.Submit(command);


            if (ModelState.IsValid)
            {
                var command = new CreateOrUpdateCategoryCommand(category.CategoryId, category.Name, category.Description, false, category.Parent);
                IEnumerable<ValidationResult> errors = commandBus.Validate(command);
                ModelState.AddModelErrors(errors);
                if (ModelState.IsValid)
                {
                    var result = commandBus.Submit(command);
                    if (result.Success)
                    {
                        //var categories = categoryRepository.GetAll();
                        return RedirectToAction("Index");
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}
