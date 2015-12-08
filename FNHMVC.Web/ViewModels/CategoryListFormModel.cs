using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FNHMVC.Model;

namespace FNHMVC.Web.ViewModels
{
    public class CategoryListFormModel
    {
        public Category category { get; set; }
        public List<Category> childs { get; set; }

        public CategoryListFormModel(Category category, List<Category> categories)
        {
            this.category = category;
            this.childs = categories;
        }
    }
}