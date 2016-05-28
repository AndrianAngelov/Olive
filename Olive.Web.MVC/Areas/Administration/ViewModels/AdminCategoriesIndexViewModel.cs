using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Olive.Data;
using Olive.Web.MVC.Areas.Administration.Models;

namespace Olive.Web.MVC.Areas.Administration.ViewModels
{
    //public enum CategoryType
    //{
    //    Parent = 1,
    //    Child = 2
    //}

    public class AdminCategoriesIndexViewModel
    {
        public AdminCategoriesIndexViewModel()
        {

        }

        public int CategoryID { get; set; }
        public string Name { get; set; }
        public CategoryType NewCategoryType { get; set; }

        public IEnumerable<Category> ParentCategories { get; set; }
        //public ICollection<Category> ParentCategories { get; set; }
        public int? SelectedParentCategoryID { get; set; }

        public IEnumerable<Category> ChildrenCategories { get; set; }
        public int? SelectedChildrenCategoryID { get; set; }

        //public Category NewCategory { get; set; }
    }
}