namespace Olive.Desktop.WPF.ViewModels
{
    using System;
    using System.Linq;
    using System.Collections.ObjectModel;
    using Olive.Data;
    using System.Windows.Input;
    using Olive.Desktop.WPF.Behavior;
    using System.Windows;
    using System.Reflection;

    public enum IngredientsAdminMode
    {
        View,
        Edit,
        Insert,
        Update
    }

    public enum UnitsAdminMode
    {
        View,
        Edit,
        Insert,
        Update
    }

    public enum SourcesAdminMode
    {
        View,
        Edit,
        Insert,
        Update
    }

    public class AdministrationViewModel : ViewModelBase, IPageViewModel
    {
        private string testValidation;

        public string TestValidation
        {
            get { return testValidation; }
            set { testValidation = value; this.OnPropertyChanged("TestValidation"); }
        }

        public string Name
        {
            get { return "Администрация"; }
        }

        public AdministrationViewModel()
        {
            this.NewCategory = new Category();
            this.EditUpdateDeleteCategory = new Category();
            this.hasParentCategory = false;
            this.IsEditMode = false;

            SwitchIngredientsToMode(IngredientsAdminMode.View);
            SwitchUnitsToMode(UnitsAdminMode.View);
            SwitchSourcesToMode(SourcesAdminMode.View);
        }

        #region Categories adminstration

        #region Categories fields
        private ObservableCollection<Category> categories;
        private ObservableCollection<Category> parentCategories;
        private bool hasParentCategory;
        private Category newCategory;
        private Category editUpdateDeleteCategory;
        private bool isEditMode;
        private bool isNormalMode;

        private ICommand addUpdateCategoryCommand;
        private ICommand editCategoryCommand;
        private ICommand deleteCategoryCommand;
        private ICommand cancelCategoryCommand;
        #endregion

        public Category NewCategory
        {
            get
            {
                return this.newCategory;
            }
            set
            {
                this.newCategory = value;
                this.OnPropertyChanged("NewCategory");
            }
        }

        public Category EditUpdateDeleteCategory
        {
            get
            {
                var viewParentsCategories = this.GetDefaultView(this.ParentCategories);

                if (this.editUpdateDeleteCategory == null)
                {
                    this.editUpdateDeleteCategory = (viewParentsCategories.CurrentItem as Category);
                }
                return this.editUpdateDeleteCategory;
            }
            set
            {
                this.editUpdateDeleteCategory = value;
                this.OnPropertyChanged("EditUpdateDeleteCategory");
            }
        }

        public bool HasParentCategory
        {
            get
            {
                return this.hasParentCategory;
            }
            set
            {
                this.hasParentCategory = value;
                this.OnPropertyChanged("HasParentCategory");
            }
        }

        public bool IsEditMode
        {
            get
            {
                return this.isEditMode;
            }
            set
            {
                this.isEditMode = value;
                this.IsNormalMode = !value;
                this.OnPropertyChanged("IsEditMode");
            }
        }

        public bool IsNormalMode
        {
            get
            {
                return this.isNormalMode;
            }
            set
            {
                this.isNormalMode = value;
                this.OnPropertyChanged("IsNormalMode");
            }
        }

        public ObservableCollection<Category> Categories
        {
            get
            {
                if (this.categories == null)
                {
                    this.categories = new ObservableCollection<Category>(this.Db.Categories.All().ToList());
                }

                return this.categories;
            }
            set
            {
                this.categories = value;
                this.OnPropertyChanged("Categories");
            }
        }

        public ObservableCollection<Category> ParentCategories
        {
            get
            {
                if (this.parentCategories == null)
                {
                    this.parentCategories = new ObservableCollection<Category>(this.Db.Categories.AllParentCategories().ToList());
                }

                return this.parentCategories;
            }
            set
            {
                this.parentCategories = value;
                this.OnPropertyChanged("ParentCategories");
            }
        }

        public ICommand АddUpdateCategoryCommand
        {
            get
            {
                if (this.addUpdateCategoryCommand == null)
                {
                    this.addUpdateCategoryCommand = new RelayCommand(this.HandleAddUpdateCategory);
                }
                return this.addUpdateCategoryCommand;
            }
        }

        public ICommand EditCategoryCommand
        {
            get
            {
                if (this.editCategoryCommand == null)
                {
                    this.editCategoryCommand = new RelayCommand(this.HandleEditCategoryCommand);
                }
                return this.editCategoryCommand;
            }
        }

        public ICommand DeleteCategoryCommand
        {
            get
            {
                if (this.deleteCategoryCommand == null)
                {
                    this.deleteCategoryCommand = new RelayCommand(this.HandleDeleteCategoryCommand);
                }
                return this.deleteCategoryCommand;
            }
        }

        public ICommand CancelCategoryCommand
        {
            get
            {
                if (this.cancelCategoryCommand == null)
                {
                    this.cancelCategoryCommand = new RelayCommand(this.HandleCancelCategoryCommand);
                }
                return this.cancelCategoryCommand;
            }
        }

        private void HandleAddUpdateCategory(object parameter)
        {
            if (this.IsNormalMode)
            {
                int result = 0;
                try
                {
                    this.Db.Categories.Add(this.NewCategory);
                    result = this.Db.SaveChanges();
                }
                catch (Exception)
                {
                    MessageBox.Show("Неможеш да добавиш категория с празно име." + "\n" + "     Първо попълни празното поле!!!");
                }
                //this.Db.Categories.Add(this.NewCategory);
                //int result = this.Db.SaveChanges();

                if (result != 0)
                {
                    RaiseCategoriesChanged();
                    HandleCancelCategoryCommand(null);
                    this.ParentCategories = new ObservableCollection<Category>(this.Db.Categories.AllParentCategories().ToList());
                    MessageBox.Show("Усщешно добавихте нова категория");
                }
            }

            if (this.IsEditMode)
            {

                if (this.EditUpdateDeleteCategory.ParentCategoryID == null)
                {
                    this.EditUpdateDeleteCategory.ParentCategoryID = null;
                }

                int result = 0;
                if (!String.IsNullOrEmpty(this.EditUpdateDeleteCategory.Name))
                {
                    this.Db.Categories.Update(this.EditUpdateDeleteCategory);
                    result = this.Db.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Неможеш да промениш категория на празно име." + "\n" + "     Първо попълни празното поле!!!");
                    this.ParentCategories = new ObservableCollection<Category>(this.GetUowDataInstance().Categories.AllParentCategories().ToList());
                    HandleCancelCategoryCommand(null);
                }

                //this.Db.Categories.Update(this.EditUpdateDeleteCategory);
                //int result = this.Db.SaveChanges();

                if (result != 0)
                {
                    RaiseCategoriesChanged();
                    HandleCancelCategoryCommand(null);
                    this.ParentCategories = new ObservableCollection<Category>(this.Db.Categories.AllParentCategories().ToList());
                    MessageBox.Show("Успешно променихте категория");
                }
            }
            this.IsEditMode = false;
        }

        private void HandleEditCategoryCommand(object parameter)
        {
            this.IsEditMode = true;

            if (this.EditUpdateDeleteCategory.ParentCategoryID != null)
            {
                this.HasParentCategory = true;
            }
        }

        private void HandleDeleteCategoryCommand(object parameter)
        {

            this.Db.Categories.Delete(this.EditUpdateDeleteCategory);

            int result = this.Db.SaveChanges();

            if (result != 0)
            {
                RaiseCategoriesChanged();
                HandleCancelCategoryCommand(null);
                this.ParentCategories = new ObservableCollection<Category>(this.Db.Categories.AllParentCategories().ToList());
                MessageBox.Show("Усщешно изтрихте категория");
            }
        }

        private void HandleCancelCategoryCommand(object parameter)
        {
            this.NewCategory = new Category();
            this.HasParentCategory = false;
            this.IsEditMode = false;
        }

        public event EventHandler<EventArgs> CategoriesChanged;

        private void RaiseCategoriesChanged()
        {

            if (this.CategoriesChanged != null)
            {
                this.CategoriesChanged(this, new EventArgs());
            }
        }
        #endregion

        #region Ingredients administaration
        #region Ingredients fields
        private ObservableCollection<Ingredient> ingredients;
        private Ingredient ingredientItem;
        private bool isViewModeIngredeients;
        private bool isInsertModeIngredeients;
        private bool isEditModeIngredeients;
        private bool isUpdateModeIngredeients;

        private ICommand addIngredientCommand;
        private ICommand editIngredientCommand;
        private ICommand updateIngredientCommand;
        private ICommand deleteIngredientCommand;
        private ICommand cancelIngredientCommand;
        #endregion

        public bool IsViewModeIngredeients
        {
            get
            {
                return this.IsViewModeIngredeients;
            }
            set
            {
                this.isViewModeIngredeients = value;
                this.OnPropertyChanged("IsViewModeIngredeients");
            }
        }

        public bool IsInsertModeIngredeients
        {
            get
            {
                return this.isInsertModeIngredeients;
            }
            set
            {
                this.isInsertModeIngredeients = value;
                this.OnPropertyChanged("IsInsertModeIngredeients");
            }
        }

        public bool IsEditModeIngredeients
        {
            get
            {
                return this.isEditModeIngredeients;
            }
            set
            {
                this.isEditModeIngredeients = value;
                this.OnPropertyChanged("IsEditModeIngredeients");
            }
        }

        public bool IsUpdateModeIngredeients
        {
            get
            {
                return this.isUpdateModeIngredeients;
            }
            set
            {
                this.isUpdateModeIngredeients = value;
                this.OnPropertyChanged("IsUpdateModeIngredeients");
            }
        }

        public Ingredient IngredientItem
        {
            get
            {
                if (this.ingredientItem == null)
                {
                    this.ingredientItem = new Ingredient();
                }
                return this.ingredientItem;
            }
            set
            {
                this.ingredientItem = value;
                this.OnPropertyChanged("IngredientItem");

            }
        }

        public ObservableCollection<Ingredient> Ingredients
        {
            get
            {
                if (this.ingredients == null)
                {
                    this.ingredients = new ObservableCollection<Ingredient>(this.Db.Ingredients.All().OrderBy(x => x.Name).ToList());
                }
                return this.ingredients;
            }
            set
            {
                this.ingredients = value;
                this.OnPropertyChanged("Ingredients");
            }
        }

        public ICommand AddIngredientCommand
        {
            get
            {
                if (this.addIngredientCommand == null)
                {
                    this.addIngredientCommand = new RelayCommand(this.HandleAddIngredientCommand);
                }
                return this.addIngredientCommand;
            }
        }

        public ICommand EditIngredientCommand
        {
            get
            {
                if (this.editIngredientCommand == null)
                {
                    this.editIngredientCommand = new RelayCommand(this.HandleEditIngredientCommand);
                }
                return this.editIngredientCommand;
            }
        }

        public ICommand UpdateIngredientCommand
        {
            get
            {
                if (this.updateIngredientCommand == null)
                {
                    this.updateIngredientCommand = new RelayCommand(this.HandleUpdateIngredientCommand);
                }
                return this.updateIngredientCommand;
            }
        }

        public ICommand DeleteIngredientCommand
        {
            get
            {
                if (this.deleteIngredientCommand == null)
                {
                    this.deleteIngredientCommand = new RelayCommand(this.HandleDeleteIngredientCommand);
                }
                return this.deleteIngredientCommand;
            }
        }

        public ICommand CancelIngredientCommand
        {
            get
            {
                if (this.cancelIngredientCommand == null)
                {
                    this.cancelIngredientCommand = new RelayCommand(this.HandleCancelIngredientCommand);
                }
                return this.cancelIngredientCommand;
            }
        }

        private void HandleAddIngredientCommand(object parameter)
        {
            //MessageBox.Show("HandleAddIngredientCommand");
            int result = 0;
            try
            {
                this.Db.Ingredients.Add(this.IngredientItem);
                result = this.Db.SaveChanges();
            }
            catch (Exception)
            {
                MessageBox.Show("Неможеш да добавиш съставка с празно име." + "\n" + "     Първо попълни празното поле!!!");
            }



            if (result != 0)
            {
                HandleCancelIngredientCommand(null);
                this.Ingredients = new ObservableCollection<Ingredient>(this.Db.Ingredients.All().OrderBy(x => x.Name).ToList());
                MessageBox.Show("Усщешно добавихте нова съставка");
            }
        }

        private void HandleEditIngredientCommand(object parameter)
        {
            this.SwitchIngredientsToMode(IngredientsAdminMode.Update);

            var view = this.GetDefaultView(this.Ingredients);
            Ingredient selectedIngredient = view.CurrentItem as Ingredient;
            this.IngredientItem = selectedIngredient;
        }

        private void HandleUpdateIngredientCommand(object parameter)
        {
            //MessageBox.Show("HandleUpdateIngredientCommand");
            int result = 0;
            if (!String.IsNullOrEmpty(this.IngredientItem.Name))
            {
                this.Db.Ingredients.Update(this.IngredientItem);
                result = this.Db.SaveChanges();
            }
            else
            {
                MessageBox.Show("Неможеш да промениш съставка на празно име." + "\n" + "     Първо попълни празното поле!!!");
                this.Ingredients = new ObservableCollection<Ingredient>(this.GetUowDataInstance().Ingredients.All().OrderBy(x => x.Name).ToList());
                HandleCancelIngredientCommand(null);
            }
            //this.Db.Ingredients.Update(this.IngredientItem);
            //int result = this.Db.SaveChanges();

            if (result != 0)
            {
                HandleCancelIngredientCommand(null);
                this.Ingredients = new ObservableCollection<Ingredient>(this.Db.Ingredients.All().OrderBy(x => x.Name).ToList());
                MessageBox.Show("Успешно променихте съставка");
            }

        }

        private void HandleDeleteIngredientCommand(object parameter)
        {
            var view = this.GetDefaultView(this.Ingredients);
            Ingredient selectedIngredient = view.CurrentItem as Ingredient;
            this.IngredientItem = selectedIngredient;

            this.Db.Ingredients.Delete(this.IngredientItem);

            int result = this.Db.SaveChanges();

            if (result != 0)
            {
                HandleCancelIngredientCommand(null);
                this.Ingredients = new ObservableCollection<Ingredient>(this.Db.Ingredients.All().OrderBy(x => x.Name).ToList());
                MessageBox.Show("Усщешно изтрихте съставка");
            }
        }

        private void HandleCancelIngredientCommand(object parameter)
        {
            SwitchIngredientsToMode(IngredientsAdminMode.View);
            this.IngredientItem = new Ingredient();
        }

        private void SwitchIngredientsToMode(IngredientsAdminMode selectIngredientMode)
        {
            switch (selectIngredientMode)
            {
                case IngredientsAdminMode.View:
                    {
                        this.IsViewModeIngredeients = true;
                        this.IsEditModeIngredeients = false;
                        this.IsInsertModeIngredeients = true;
                        this.IsUpdateModeIngredeients = false;
                        break;
                    }
                case IngredientsAdminMode.Edit:
                    {
                        this.IsEditModeIngredeients = true;
                        this.IsViewModeIngredeients = false;
                        break;
                    }
                case IngredientsAdminMode.Insert:
                    {
                        this.IsViewModeIngredeients = false;
                        this.IsEditModeIngredeients = true;
                        this.IsInsertModeIngredeients = true;
                        this.IsUpdateModeIngredeients = false;
                        break;
                    }
                case IngredientsAdminMode.Update:
                    {
                        this.IsViewModeIngredeients = false;
                        this.IsEditModeIngredeients = true;
                        this.IsInsertModeIngredeients = false;
                        this.IsUpdateModeIngredeients = true;
                        break;
                    }
            }
        }
        #endregion

        #region Units administaration
        #region Units fields
        private ObservableCollection<Unit> units;
        private Unit unitItem;

        private bool isViewModeUnits;
        private bool isInsertModeUnits;
        private bool isEditModeUnits;
        private bool isUpdateModeUnits;

        private ICommand addUnitCommand;
        private ICommand editUnitCommand;
        private ICommand updateUnitCommand;
        private ICommand deleteUnitCommand;
        private ICommand cancelUnitCommand;
        #endregion

        public bool IsViewModeUnits
        {
            get
            {
                return this.IsViewModeUnits;
            }
            set
            {
                this.isViewModeUnits = value;
                this.OnPropertyChanged("IsViewModeUnits");
            }
        }

        public bool IsInsertModeUnits
        {
            get
            {
                return this.isInsertModeUnits;
            }
            set
            {
                this.isInsertModeUnits = value;
                this.OnPropertyChanged("IsInsertModeUnits");
            }
        }

        public bool IsEditModeUnits
        {
            get
            {
                return this.isEditModeUnits;
            }
            set
            {
                this.isEditModeUnits = value;
                this.OnPropertyChanged("IsEditModeUnits");
            }
        }

        public bool IsUpdateModeUnits
        {
            get
            {
                return this.isUpdateModeUnits;
            }
            set
            {
                this.isUpdateModeUnits = value;
                this.OnPropertyChanged("IsUpdateModeUnits");
            }
        }

        public Unit UnitItem
        {
            get
            {
                //if (this.ingredient == null)
                //{
                //    this.ingredient = this.Db.Recipes.GetById(this.RecipeID);
                //    this.ingredient.ImageURL = Properties.Resources.RecipesImagesFolder + this.recipe.ImageURL;
                //}
                if (this.unitItem == null)
                {
                    this.unitItem = new Unit();
                }
                return this.unitItem;
            }
            set
            {
                this.unitItem = value;
                this.OnPropertyChanged("UnitItem");

            }
        }

        public ObservableCollection<Unit> Units
        {
            get
            {
                if (this.units == null)
                {
                    this.units = new ObservableCollection<Unit>(this.Db.Units.All().OrderBy(x => x.UnitName).ToList());
                }
                //this.ingredients = new ObservableCollection<Ingredient>(this.Db.Ingredients.All().OrderBy(x => x.IngredientID).ToList());
                return this.units;
            }
            set
            {
                this.units = value;
                this.OnPropertyChanged("Units");
            }
        }

        public ICommand AddUnitCommand
        {
            get
            {
                if (this.addUnitCommand == null)
                {
                    this.addUnitCommand = new RelayCommand(this.HandleAddUnitCommand);
                }
                return this.addUnitCommand;
            }
        }

        public ICommand EditUnitCommand
        {
            get
            {
                if (this.editUnitCommand == null)
                {
                    this.editUnitCommand = new RelayCommand(this.HandleEditUnitCommand);
                }
                return this.editUnitCommand;
            }
        }

        public ICommand UpdateUnitCommand
        {
            get
            {
                if (this.updateUnitCommand == null)
                {
                    this.updateUnitCommand = new RelayCommand(this.HandleUpdateUnitCommand);
                }
                return this.updateUnitCommand;
            }
        }

        public ICommand DeleteUnitCommand
        {
            get
            {
                if (this.deleteUnitCommand == null)
                {
                    this.deleteUnitCommand = new RelayCommand(this.HandleDeleteUnitCommand);
                }
                return this.deleteUnitCommand;
            }
        }

        public ICommand CancelUnitCommand
        {
            get
            {
                if (this.cancelUnitCommand == null)
                {
                    this.cancelUnitCommand = new RelayCommand(this.HandleCancelUnitCommand);
                }
                return this.cancelUnitCommand;
            }
        }

        private void HandleAddUnitCommand(object parameter)
        {
            //MessageBox.Show("HandleAddUnitCommand");
            int result = 0;
            try
            {
                this.Db.Units.Add(this.UnitItem);
                result = this.Db.SaveChanges();

            }
            catch (Exception)
            {
                MessageBox.Show("Неможеш да добавиш мерна единица с празно име." + "\n" + "     Първо попълни празното поле!!!");
            }
            //this.Db.Units.Add(this.UnitItem);
            //int result = this.Db.SaveChanges();

            if (result != 0)
            {
                HandleCancelUnitCommand(null);
                this.Units = new ObservableCollection<Unit>(this.Db.Units.All().OrderBy(x => x.UnitName).ToList());
                MessageBox.Show("Усщешно добавихте нова мерна единица");

            }
        }

        private void HandleEditUnitCommand(object parameter)
        {
            //MessageBox.Show("HandleEditUnitCommand");
            this.SwitchUnitsToMode(UnitsAdminMode.Update);

            var view = this.GetDefaultView(this.Units);
            Unit selectedUnit = view.CurrentItem as Unit;
            this.UnitItem = selectedUnit;
        }

        private void HandleUpdateUnitCommand(object parameter)
        {
            //MessageBox.Show("HandleUpdateUnitCommand");

            int result = 0;
            //MessageBox.Show("HandleUpdateUnitCommand");
            if (!String.IsNullOrEmpty(this.UnitItem.UnitName) || !String.IsNullOrEmpty(this.UnitItem.UnitName))
            {
                this.Db.Units.Update(this.UnitItem);
                result = this.Db.SaveChanges();
            }
            else
            {
                MessageBox.Show("Неможеш да промениш мерна единица на празно име." + "\n" + "     Първо попълни празното поле!!!");
                this.Units = new ObservableCollection<Unit>(this.GetUowDataInstance().Units.All().OrderBy(x => x.UnitName).ToList());
                HandleCancelUnitCommand(null);
            }

            //this.Db.Units.Update(this.UnitItem);
            //int result = this.Db.SaveChanges();

            if (result != 0)
            {
                HandleCancelUnitCommand(null);
                this.Units = new ObservableCollection<Unit>(this.Db.Units.All().OrderBy(x => x.UnitName).ToList());
                MessageBox.Show("Успешно променихте мерна единица");
            }

        }

        private void HandleDeleteUnitCommand(object parameter)
        {
            //MessageBox.Show("HandleDeleteUnitCommand");

            var view = this.GetDefaultView(this.Units);
            Unit selectedUnit = view.CurrentItem as Unit;
            this.UnitItem = selectedUnit;

            this.Db.Units.Delete(this.UnitItem);

            int result = this.Db.SaveChanges();

            if (result != 0)
            {
                HandleCancelUnitCommand(null);
                this.Units = new ObservableCollection<Unit>(this.Db.Units.All().OrderBy(x => x.UnitName).ToList());
                MessageBox.Show("Усщешно изтрихте мерна единица");
            }
        }

        private void HandleCancelUnitCommand(object parameter)
        {
            //MessageBox.Show("HandleCancelUnitCommand");
            SwitchUnitsToMode(UnitsAdminMode.View);
            this.UnitItem = new Unit();
        }

        private void SwitchUnitsToMode(UnitsAdminMode selectUnitsMode)
        {
            switch (selectUnitsMode)
            {
                case UnitsAdminMode.View:
                    {
                        this.IsViewModeUnits = true;
                        this.IsEditModeUnits = false;
                        this.IsInsertModeUnits = true;
                        this.IsUpdateModeUnits = false;
                        break;
                    }
                case UnitsAdminMode.Edit:
                    {
                        this.IsEditModeUnits = true;
                        this.IsViewModeUnits = false;
                        break;
                    }
                case UnitsAdminMode.Insert:
                    {
                        this.IsViewModeUnits = false;
                        this.IsEditModeUnits = true;
                        this.IsInsertModeUnits = true;
                        this.IsUpdateModeUnits = false;
                        break;
                    }
                case UnitsAdminMode.Update:
                    {
                        this.IsViewModeUnits = false;
                        this.IsEditModeUnits = true;
                        this.IsInsertModeUnits = false;
                        this.IsUpdateModeUnits = true;
                        break;
                    }
            }
        }
        #endregion

        #region Sources administration
        #region Sources fields
        private ObservableCollection<Source> sources;
        private Source sourceItem;

        private bool isViewModeSources;
        private bool isInsertModeSources;
        private bool isEditModeSources;
        private bool isUpdateModeSources;

        private ICommand addSourceCommand;
        private ICommand editSourceCommand;
        private ICommand updateSourceCommand;
        private ICommand deleteSourceCommand;
        private ICommand cancelSourceCommand;
        #endregion

        public bool IsViewModeSources
        {
            get
            {
                return this.IsViewModeSources;
            }
            set
            {
                this.isViewModeSources = value;
                this.OnPropertyChanged("IsViewModeSources");
            }
        }

        public bool IsInsertModeSources
        {
            get
            {
                return this.isInsertModeSources;
            }
            set
            {
                this.isInsertModeSources = value;
                this.OnPropertyChanged("IsInsertModeSources");
            }
        }

        public bool IsEditModeSources
        {
            get
            {
                return this.isEditModeSources;
            }
            set
            {
                this.isEditModeSources = value;
                this.OnPropertyChanged("IsEditModeSources");
            }
        }

        public bool IsUpdateModeSources
        {
            get
            {
                return this.isUpdateModeSources;
            }
            set
            {
                this.isUpdateModeSources = value;
                this.OnPropertyChanged("IsUpdateModeSources");
            }
        }

        public Source SourceItem
        {
            get
            {
                //if (this.ingredient == null)
                //{
                //    this.ingredient = this.Db.Recipes.GetById(this.RecipeID);
                //    this.ingredient.ImageURL = Properties.Resources.RecipesImagesFolder + this.recipe.ImageURL;
                //}
                if (this.sourceItem == null)
                {
                    this.sourceItem = new Source();
                }
                return this.sourceItem;
            }
            set
            {
                this.sourceItem = value;
                this.OnPropertyChanged("SourceItem");

            }
        }

        public ObservableCollection<Source> Sources
        {
            get
            {
                if (this.sources == null)
                {
                    this.sources = new ObservableCollection<Source>(this.Db.Sources.All().OrderBy(x => x.Name).ToList());
                }
                //this.ingredients = new ObservableCollection<Ingredient>(this.Db.Ingredients.All().OrderBy(x => x.IngredientID).ToList());
                return this.sources;
            }
            set
            {
                this.sources = value;
                this.OnPropertyChanged("Sources");
            }
        }

        public ICommand AddSourceCommand
        {
            get
            {
                if (this.addSourceCommand == null)
                {
                    this.addSourceCommand = new RelayCommand(this.HandleAddSourceCommand);
                    //this.addSourceCommand = new RelayCommand(this.HandleAddSourceCommand, this.HandleStopAddSourceCommand);
                }
                //else
                //{
                //    this.addSourceCommand = new RelayCommand(this.HandleAddSourceCommand);
                //    //this.addSourceCommand.CanExecuteChanged += this.OnCanExecuteChanged;
                //}

                return this.addSourceCommand;
            }
        }

        public ICommand EditSourceCommand
        {
            get
            {
                if (this.editSourceCommand == null)
                {
                    this.editSourceCommand = new RelayCommand(this.HandleEditSourceCommand);
                }
                return this.editSourceCommand;
            }
        }

        public ICommand UpdateSourceCommand
        {
            get
            {
                if (this.updateSourceCommand == null)
                {
                    this.updateSourceCommand = new RelayCommand(this.HandleUpdateSourceCommand);
                }
                return this.updateSourceCommand;
            }
        }

        public ICommand DeleteSourceCommand
        {
            get
            {
                if (this.deleteSourceCommand == null)
                {
                    this.deleteSourceCommand = new RelayCommand(this.HandleDeleteSourceCommand);
                }
                return this.deleteSourceCommand;
            }
        }

        public ICommand CancelSourceCommand
        {
            get
            {
                if (this.cancelSourceCommand == null)
                {
                    this.cancelSourceCommand = new RelayCommand(this.HandleCancelSourceCommand);
                }
                return this.cancelSourceCommand;
            }
        }

        //private Validation sourceError;

        //public Validation SourceError
        //{
        //    get { return sourceError; }
        //    set { sourceError = value; this.OnPropertyChanged("SourceError"); }
        //}

        private string sourceError;

        public string SourceError
        {
            get { return sourceError; }
            set { sourceError = value; this.OnPropertyChanged("SourceError"); }
        }

        private void HandleAddSourceCommand(object parameter)
        {
            //MessageBox.Show("HandleAddUnitCommand");

            //if (Validation.GetHasError(this.textBoxName))
            //{
            //    var errors = Validation.GetErrors(this.textBoxName);
            //    string errorMsg = (string)errors[0].ErrorContent;
            //    MessageBox.Show(errorMsg, "Error");
            //    return;
            //}

            //MessageBox.Show("SourceError: " + this.SourceError.ToString());

            int result = 0;
            try
            {
                this.Db.Sources.Add(this.SourceItem);
                result = this.Db.SaveChanges();
            }
            catch (Exception)
            {

                MessageBox.Show("Неможеш да добавиш източник с празно име." + "\n" + "     Първо попълни празното поле!!!");
            }

            //this.Db.Sources.Add(this.SourceItem);
            //int result = this.Db.SaveChanges();

            if (result != 0)
            {
                HandleCancelSourceCommand(null);
                this.Sources = new ObservableCollection<Source>(this.Db.Sources.All().OrderBy(x => x.Name).ToList());
                MessageBox.Show("Усщешно добавихте нов източник");

            }
        }

        public void OnCanExecuteChanged(object sender, EventArgs e)
        {

        }

        private bool HandleStopAddSourceCommand(object parameter)
        {
            bool validateSuccess = true;

            if (String.IsNullOrEmpty(this.SourceItem.Name))
            {
                validateSuccess = false;
            }
            return validateSuccess;
        }

        private void HandleEditSourceCommand(object parameter)
        {
            //MessageBox.Show("HandleEditUnitCommand");
            this.SwitchSourcesToMode(SourcesAdminMode.Update);

            var view = this.GetDefaultView(this.Sources);
            Source selectedSource = view.CurrentItem as Source;
            this.SourceItem = selectedSource;
        }

        private void HandleUpdateSourceCommand(object parameter)
        {
            int result = 0;
            //MessageBox.Show("HandleUpdateUnitCommand");
            if (!String.IsNullOrEmpty(this.SourceItem.Name))
            {
                this.Db.Sources.Update(this.SourceItem);
                result = this.Db.SaveChanges();
            }
            else
            {
                MessageBox.Show("Неможеш да промениш източник с на празно име." + "\n" + "     Първо попълни празното поле!!!");
                this.Sources = new ObservableCollection<Source>(this.GetUowDataInstance().Sources.All().OrderBy(x => x.Name).ToList());
                HandleCancelSourceCommand(null);
            }
            //this.Db.Sources.Update(this.SourceItem);
            //int result = this.Db.SaveChanges();

            if (result != 0)
            {
                HandleCancelSourceCommand(null);
                this.Sources = new ObservableCollection<Source>(this.Db.Sources.All().OrderBy(x => x.Name).ToList());
                MessageBox.Show("Успешно променихте източник");
            }

        }

        private void HandleDeleteSourceCommand(object parameter)
        {
            //MessageBox.Show("HandleDeleteUnitCommand");

            var view = this.GetDefaultView(this.Sources);
            Source selectedSource = view.CurrentItem as Source;
            this.SourceItem = selectedSource;

            this.Db.Sources.Delete(this.SourceItem);

            int result = this.Db.SaveChanges();

            if (result != 0)
            {
                HandleCancelSourceCommand(null);
                this.Sources = new ObservableCollection<Source>(this.Db.Sources.All().OrderBy(x => x.Name).ToList());
                MessageBox.Show("Усщешно изтрихте източник");
            }
        }

        private void HandleCancelSourceCommand(object parameter)
        {
            //MessageBox.Show("HandleCancelUnitCommand");
            SwitchSourcesToMode(SourcesAdminMode.View);
            this.SourceItem = new Source();

            //Application.StartupPath
            //MessageBox.Show(this.RecipesImagesFolderPath);
        }

        private void SwitchSourcesToMode(SourcesAdminMode selectSourcesMode)
        {
            switch (selectSourcesMode)
            {
                case SourcesAdminMode.View:
                    {
                        this.IsViewModeSources = true;
                        this.IsEditModeSources = false;
                        this.IsInsertModeSources = true;
                        this.IsUpdateModeSources = false;
                        break;
                    }
                case SourcesAdminMode.Edit:
                    {
                        this.IsEditModeSources = true;
                        this.IsViewModeSources = false;
                        break;
                    }
                case SourcesAdminMode.Insert:
                    {
                        this.IsViewModeSources = false;
                        this.IsEditModeSources = true;
                        this.IsInsertModeSources = true;
                        this.IsUpdateModeSources = false;
                        break;
                    }
                case SourcesAdminMode.Update:
                    {
                        this.IsViewModeSources = false;
                        this.IsEditModeSources = true;
                        this.IsInsertModeSources = false;
                        this.IsUpdateModeSources = true;
                        break;
                    }
            }
        }
        #endregion

        #region Settings
        private string assemblyPath = Assembly.GetExecutingAssembly().Location;
        private string recipesImagesFolderPath;
        private OliveAppSetting recipesImagesFolderName;

        public string RecipesImagesFolderPath
        {
            get
            {
                int isDebug = assemblyPath.IndexOf("Debug");
                if (isDebug != -1)
                {
                    this.recipesImagesFolderPath = assemblyPath.Replace(@"bin\Debug\Olive.Desktop.WPF.exe", @"Images\");
                }
                else
                {
                    this.recipesImagesFolderPath = assemblyPath.Replace(@"Application\Olive.Desktop.WPF.exe", this.RecipesImagesFolderName.Value);
                }

                return this.recipesImagesFolderPath;
            }
            set
            {
                this.recipesImagesFolderPath = value;
                this.OnPropertyChanged("RecipesImagesFolderPath");
            }
        }

        public OliveAppSetting RecipesImagesFolderName
        {
            get
            {

                this.recipesImagesFolderName = this.Db.OliveAppSettings.All().Where(s => s.Name == "RecipesImagesFolderName").FirstOrDefault();
                return this.recipesImagesFolderName;
            }
            set
            {
                this.recipesImagesFolderName = value;
                this.OnPropertyChanged("RecipesImagesFolderName");
            }
        }
        #endregion
    }
}
