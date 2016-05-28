using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Olive.Web.MVC.ViewModels
{
    public class CategoryViewModel
    {
        public int CategoryID { get; set; }
        public int ParentCategoryID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<CategoryViewModel> ChildCategories { get; set; }
        public virtual CategoryViewModel ParentCategory { get; set; }
    }
}