using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Olive.Data;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Olive.Web.MVC.Areas.Administration.ViewModels
{
    public class AdminRecipeViewModel
    {
        public Recipe Recipe { get; set; }

        public int RecipeID { get; set; }
        [Required]
        public string Title { get; set; }

        public HttpPostedFileBase Image { get; set; }
        public string ImageURL { get; set; }
        public string ImageVersion { get; set; }
        
        public IEnumerable<Category> ParentCategories { get; set; }
        [Required]
        public int SelectedParentCategory { get; set; }
        public IEnumerable<Category> ChildrenCategories { get; set; }
        public int? SelectedChildrenCategory { get; set; }
        public IEnumerable<int> PrepTimeHours { get; set; }
        public int SelectedPrepTimeHours { get; set; }
        public IEnumerable<int> PrepTimeMinutes { get; set; }
        public int SelectedPrepTimeMinutes { get; set; }
        public IEnumerable<int> CookTimeHours { get; set; }
        public int SelectedCookTimeHours { get; set; }
        public IEnumerable<int> CookTimeMinutes { get; set; }
        public int SelectedCookTimeMinutes { get; set; }
        public IEnumerable<int> Serves { get; set; }
        public int SelectedServes { get; set; }
        public IEnumerable<int> Rating { get; set; }
        public int SelectedRating { get; set; }
        public IEnumerable<Source> Sources { get; set; }
        public int SelectedSource { get; set; }
        public IEnumerable<string> Recommendation { get; set; }
        public string SelectedRecommendation { get; set; }

        public DateTime PublishedDate { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Description { get; set; }
        public int NumberOfHits { get; set; }
        public int NumberOfLikes { get; set; }

        //public List<AdminStepViewModel> Steps { get; set; }
        public List<RecipeItem> RecipeItems { get; set; }

        public AdminRecipeViewModel() 
        {
            Recipe = new Recipe();
            RecipeItems = new List<RecipeItem>();
        }
    }
}