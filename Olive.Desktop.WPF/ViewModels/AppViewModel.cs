namespace Olive.Desktop.WPF.ViewModels
{
    using System;
    using System.Windows.Input;
    using Olive.Desktop.WPF.Behavior;
    using Olive.Desktop.WPF.Helpers;
    using System.Windows;
    using System.Reflection;
    using System.Xml;
    using IWshRuntimeLibrary;
    using System.IO;
    using System.Diagnostics;

    public class AppViewModel : ViewModelBase
    {
        private IPageViewModel currentViewModel;
        private CategoriesListViewModel categoriesListVM;
        private RecipesListViewModel recipesListVM;
        private AdministrationViewModel administrationVM;
        private RecipeViewModel recipeVM;

        private ICommand changeViewModelCommand;
        private ICommand goHomeCommand;
        private ICommand newRecipeCommand;

        public AppViewModel()
        {
            this.CurrentViewModel = RecipesListVM;
        }

        public CategoriesListViewModel CategoriesListVM
        {
            get
            {
                if (this.categoriesListVM == null)
                {
                    this.categoriesListVM = new CategoriesListViewModel();
                }
                this.categoriesListVM.RecipesByCategoryChanged += this.OnRecipesByCategoryChanged;
                return this.categoriesListVM;
            }
            set
            {
                this.categoriesListVM = value;
                this.OnPropertyChanged("CategoriesListVM");
            }
        }

        public RecipesListViewModel RecipesListVM
        {
            get
            {
                if (this.recipesListVM == null)
                {
                    this.recipesListVM = new RecipesListViewModel();
                    this.recipesListVM.SelectedRecipeChanged += this.OnSelectedRecipeChanged;
                }

                return this.recipesListVM;
            }
            set
            {
                this.recipesListVM = value;
                if (this.recipesListVM != null)
                {
                    this.recipesListVM.SelectedRecipeChanged += this.OnSelectedRecipeChanged;
                }
                this.OnPropertyChanged("RecipesListVM");
            }
        }

        public AdministrationViewModel AdministrationVM
        {
            get
            {
                if (this.administrationVM == null)
                {
                    this.administrationVM = new AdministrationViewModel();
                    this.administrationVM.CategoriesChanged += this.OnCategoriesChanged;
                }
                return this.administrationVM;
            }
            set
            {
                this.administrationVM = value;
                this.administrationVM.CategoriesChanged += this.OnCategoriesChanged;
                this.OnPropertyChanged("AdministrationVM");
            }
        }

        public RecipeViewModel RecipeVM
        {
            get
            {
                if (this.recipeVM == null)
                {
                    this.recipeVM = new RecipeViewModel();
                }
                return this.recipeVM;
            }
            set
            {
                this.recipeVM = value;
                this.recipeVM.NewRecipeChanged += this.OnNewRecipeChanged;
                this.recipeVM.UpdateRecipeChanged += this.OnUpdateRecipeChanged;
                this.recipeVM.DeleteRecipeChanged += this.OnDeleteRecipeChanged;
                this.recipeVM.CancelInsertModeChanged += this.OnCancelInsertModeChanged;
                this.OnPropertyChanged("RecipeVM");

            }
        }

        public IPageViewModel CurrentViewModel
        {
            get
            {
                return this.currentViewModel;
            }
            set
            {
                this.currentViewModel = value;
                this.OnPropertyChanged("CurrentViewModel");
            }
        }

        public ICommand ChangeViewModel
        {
            get
            {
                if (this.changeViewModelCommand == null)
                {
                    this.changeViewModelCommand = new RelayCommand(this.HandleChangeViewModelCommand);
                }
                return this.changeViewModelCommand;
            }
        }

        public ICommand GoHomeCommand
        {
            get
            {
                if (this.goHomeCommand == null)
                {
                    this.goHomeCommand = new RelayCommand(this.HandleGoHomeCommand);
                }
                return this.goHomeCommand;
            }
        }

        public ICommand NewRecipeCommand
        {
            get
            {
                if (this.newRecipeCommand == null)
                {
                    this.newRecipeCommand = new RelayCommand(this.HandleNewRecipeCommand);
                }
                return this.newRecipeCommand;
            }
        }

        private void HandleChangeViewModelCommand(object parameter)
        {
            var newCurrentViewModel = parameter as IPageViewModel;
            this.CurrentViewModel = newCurrentViewModel;
        }

        private void HandleGoHomeCommand(object parameter)
        {
            this.RecipesListVM = new RecipesListViewModel();
            this.HandleChangeViewModelCommand(this.RecipesListVM);
        }

        private void HandleNewRecipeCommand(object parameter)
        {
            this.RecipeVM = new RecipeViewModel(0, RecipeMode.Insert);
            this.HandleChangeViewModelCommand(this.RecipeVM);
        }

        public void OnRecipesByCategoryChanged(object sender, RecipesByCategoryChangedArgs e)
        {
            this.RecipesListVM = new RecipesListViewModel(e.CategoryID);
            this.HandleChangeViewModelCommand(this.RecipesListVM);
        }

        public void OnSelectedRecipeChanged(object sender, SelectedRecipeChangedArgs e)
        {
            this.RecipeVM = new RecipeViewModel(e.RecipeID, RecipeMode.View);
            this.HandleChangeViewModelCommand(this.RecipeVM);
        }

        public void OnCategoriesChanged(object sender, EventArgs e)
        {
            this.CategoriesListVM = new CategoriesListViewModel();
        }

        public void OnNewRecipeChanged(object sender, SelectedRecipeChangedArgs e)
        {
            this.RecipeVM = new RecipeViewModel(e.RecipeID, RecipeMode.View);
            this.HandleChangeViewModelCommand(this.RecipeVM);
        }

        public void OnUpdateRecipeChanged(object sender, SelectedRecipeChangedArgs e)
        {
            this.RecipeVM = new RecipeViewModel(e.RecipeID, RecipeMode.View);
            this.HandleChangeViewModelCommand(this.RecipeVM);
        }

        public void OnDeleteRecipeChanged(object sender, EventArgs e)
        {
            this.HandleGoHomeCommand(null);
        }

        public void OnCancelInsertModeChanged(object sender, EventArgs e)
        {
            this.HandleGoHomeCommand(null);
        }

    }
}
