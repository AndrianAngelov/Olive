namespace Olive.Desktop.WPF.ViewModels
{
    using System;
    using System.Linq;
    using Olive.Data;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Olive.Desktop.WPF.Behavior;
    using Olive.Desktop.WPF.Helpers;

    public class CategoriesListViewModel : ViewModelBase,IPageViewModel
    {
        ObservableCollection<Category> categories;
        private ICommand getRecipesByCategoryCommand;
        
        public CategoriesListViewModel()
        {

        }

        public string Name
        {
            get
            {
                return "Categories List View";
            }
        }

        public ObservableCollection<Category> Categories
        {
            get
            {
                if (this.categories == null)
                {
                    this.categories = new ObservableCollection<Category>(this.Db.Categories.AllParentCategories().ToList());
                }
                this.categories = new ObservableCollection<Category>(this.Db.Categories.AllParentCategories().ToList());
                return this.categories;
            }
        }

        public ICommand GetRecipesByCategoryCommand
        {
            get
            {
                if (this.getRecipesByCategoryCommand == null)
                {
                    this.getRecipesByCategoryCommand = new RelayCommand(this.HandleGetRecipesByCategoryCommand);
                }
                return this.getRecipesByCategoryCommand;
            }
        }

        private void HandleGetRecipesByCategoryCommand(object parameter)
        {
            long selectedCategoryID = (parameter as Category).CategoryID;
            this.RaiseRecipesByCategoryChanged(selectedCategoryID);
        }

        public event EventHandler<RecipesByCategoryChangedArgs> RecipesByCategoryChanged;

        private void RaiseRecipesByCategoryChanged(long categoryID)
        {

            if (this.RecipesByCategoryChanged != null)
            {
                this.RecipesByCategoryChanged(this, new RecipesByCategoryChangedArgs(categoryID));
            }
        }

    }
}
