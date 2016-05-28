using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Olive.Data;

namespace Olive.Web.MVC.Areas.Administration.ViewModels
{
    public class IngredientsIndexViewModel
    {
        public IEnumerable<Ingredient> AllIngredients { get; set; }
        public Ingredient NewIngredient{ get; set; }
    }
}