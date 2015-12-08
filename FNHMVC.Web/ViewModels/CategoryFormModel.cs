using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FNHMVC.Model;

namespace FNHMVC.Web.ViewModels
{
    public class CategoryFormModel
    {
        public long CategoryId { get; set; }
        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "SubCategoria")]
        public long ParentId { get; set; }
        public IEnumerable<SelectListItem> ParentCategories { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Activa")]
        public bool Activated { get; set; }

        public CategoryFormModel() { Activated = true; }

        public CategoryFormModel(Category category)
        {
            this.CategoryId = category.CategoryId;
            this.Name = category.Name;
            this.Description = category.Description;
            this.Activated = category.Activated;
            if (category.Parent != null)
                this.ParentId = category.Parent.CategoryId;
        }
    }
}