namespace Olive.Desktop.WPF.ViewModels
{
    using System;
    using System.Linq;
    using System.Collections.ObjectModel;
    using Olive.Data;
    using System.Windows.Input;
    using Olive.Desktop.WPF.Behavior;
    using Olive.Desktop.WPF.Helpers;
    using System.ComponentModel;
    using System.Windows.Controls;

    public class RecipesListViewModel:ViewModelBase,IPageViewModel
    {
        private ObservableCollection<Recipe> recipes;
        private ICommand selectedRecipeCommand;
        private ICommand searchByTitleCommand;
        private string searchText;

        public RecipesListViewModel()
        {
        }

        public RecipesListViewModel(long categoryID)
        {
            this.Recipes = new ObservableCollection<Recipe>(this.Db.Recipes.AllRecipesByCategoryID(categoryID).ToList());
        }

        public string Name
        {
            get { return "Рецепти"; }
        }


        public string SearchText
        {
            get 
            { 
                return this.searchText; 
            }
            set 
            { 
                this.searchText = value;
                this.OnPropertyChanged("SearchText");
            }
        }
        

        public ObservableCollection<Recipe> Recipes
        {
            get
            {
                if (this.recipes == null)
                {
                    this.recipes = new ObservableCollection<Recipe>(this.Db.Recipes.All().ToList());
                }

                return this.recipes;
            }

            set
            {
                this.recipes = value;
                this.OnPropertyChanged("Recipes"); 
            }
            
        }

        public ICommand GetSelectedRecipeCommand
        {
            get
            {
                if (this.selectedRecipeCommand == null)
                {
                    this.selectedRecipeCommand = new RelayCommand(this.HandleSelectedRecipe);
                }
                return this.selectedRecipeCommand;
            }
        }

        public ICommand SearchByTitleCommand
        {
            get
            {
                if (this.searchByTitleCommand == null)
                {
                    this.searchByTitleCommand = new RelayCommand(this.HandleSearchByTitleCommand);
                }
                return this.searchByTitleCommand;
            }
        }

        private void HandleSelectedRecipe(object parameter)
        {
            ListViewItem selectedItem = parameter as ListViewItem;
            Recipe recipe = (selectedItem.Content as Recipe);
            this.RaiseSelectedRecipeChanged(recipe.RecipeID);
        }

        private void HandleSearchByTitleCommand(object parameter)
        {
            Filter(parameter.ToString());
        }

        public event EventHandler<SelectedRecipeChangedArgs> SelectedRecipeChanged;

        private void RaiseSelectedRecipeChanged(long recipeID)
        {

            if (this.SelectedRecipeChanged != null)
            {
                this.SelectedRecipeChanged(this, new SelectedRecipeChangedArgs(recipeID));
            }
        }

        void OnViewSourceCurrentChanged(object sender, EventArgs e)
        {
            var selectedRecipe = (sender as ICollectionView).CurrentItem as Recipe;
            this.RaiseSelectedRecipeChanged(selectedRecipe.RecipeID);
        }

        private void Filter(string filterText)
        {
            var recipesCollectionView = this.GetDefaultView(this.Recipes);
            if (filterText == string.Empty)
            {
                recipesCollectionView.Filter = null;
            }
            else
            {
                var filterTextToLower = filterText.ToLower();
                recipesCollectionView.Filter = (item) =>
                {
                    var recipe = item as Recipe;
                    if (recipe == null)
                    {
                        return false;
                    }
                    return recipe.Title.ToLower().Contains(filterTextToLower);
                           //phoneItem.Model.ToLower().Contains(filterTextToLower) ||
                           //phoneItem.Year.ToString().Contains(filterTextToLower) ||
                           //phoneItem.OS.Name.ToLower().Contains(filterTextToLower) ||
                           //phoneItem.OS.Version.ToLower().Contains(filterTextToLower) ||
                           //phoneItem.OS.Manufacturer.ToLower().Contains(filterTextToLower);
                };
            }
        }
    }
}
