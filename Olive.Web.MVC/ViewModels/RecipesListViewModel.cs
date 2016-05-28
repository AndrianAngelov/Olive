namespace Olive.Web.MVC.ViewModels
{
    using System.Collections.Generic;

    public class RecipesListViewModel : ViewModelBase
    {

        public string ListTitle { get; set; }

        public IEnumerable<RecipeViewModel> Recipes { get; set; }

        public int PageSize { get; set; }

        public int CurrentCategoryID { get; set; }

        public int RecipesListNumberOfPages { get; set; }

        public string PagingBtnsClass { get; set; }
    }
}
