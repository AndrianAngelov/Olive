using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Olive.Web.MVC.ViewModels
{
    public class HomeViewModel:ViewModelBase
    {
        public IEnumerable<RecipeViewModel> RecipesByRosiRecommendation { get; set; }

        public IEnumerable<RecipeViewModel> RecipesByCreationDate { get; set; }

        public IEnumerable<RecipeViewModel> RecipesByRating{ get; set; }

        public IEnumerable<RecipeViewModel> RecipesByNubmerOfHits{ get; set; }
    }
}