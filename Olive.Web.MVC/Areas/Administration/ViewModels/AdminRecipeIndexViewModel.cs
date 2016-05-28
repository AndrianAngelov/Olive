using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using Olive.Data;
using Olive.Web.MVC.ViewModels;

namespace Olive.Web.MVC.Areas.Administration.ViewModels
{
    public class AdminRecipeIndexViewModel
    {
        public int RecipeID { get; set; }
        public string Title { get; set; }
        public string ImageURL { get; set; }
        public string ImageVersion { get; set; }
        public int Rating { get; set; }
        public int PreparationTime { get; set; }
        public int CookingTime { get; set; }
        public int Serves { get; set; }
        public string SourceName { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int NumberOfHits { get; set; }
        public int NumberOfLikes { get; set; }

        //public IEnumerable<Category> Categories { get; set; }
        public string Category { get; set; }
        public string ParentCategory { get; set; }

    }
}