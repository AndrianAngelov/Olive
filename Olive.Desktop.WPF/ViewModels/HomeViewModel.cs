namespace Olive.Desktop.WPF.ViewModels
{
    using System.Linq;
    using System.Collections.ObjectModel;
    using Olive.Data;
    using Olive.Data.Uow;

    public class HomeViewModel : ViewModelBase, IPageViewModel
    {
        private ObservableCollection<Recipe> recipes;

        public string Name
        {
            get { return "HOME VIEW"; }
        }

        public ObservableCollection<Recipe> Recipes
        {
            get
            {
                if (this.recipes == null)
                {
                    this.recipes = new ObservableCollection<Recipe>(new UowData().Recipes.All().ToList());
                }
                return this.recipes;
            }
        }
    }
}
