namespace Olive.Web.MVC.ViewModels.Administration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Collections.ObjectModel;
    using Olive.Data;
    using Olive.Data.Uow;
   
    using System.Windows;
    
    using System.ComponentModel;
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
                if (this.editUpdateDeleteCategory == null)
                {
                    this.editUpdateDeleteCategory = new Category();
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
                }

                if (result != 0)
                {
                    RaiseCategoriesChanged();
                    HandleCancelCategoryCommand(null);
                    this.ParentCategories = new ObservableCollection<Category>(this.Db.Categories.AllParentCategories().ToList());
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
                    this.ParentCategories = new ObservableCollection<Category>(this.GetUowDataInstance().Categories.AllParentCategories().ToList());
                    HandleCancelCategoryCommand(null);
                }

                if (result != 0)
                {
                    RaiseCategoriesChanged();
                    HandleCancelCategoryCommand(null);
                    this.ParentCategories = new ObservableCollection<Category>(this.Db.Categories.AllParentCategories().ToList());
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

        private void HandleAddIngredientCommand(object parameter)
        {
            int result = 0;
            try
            {
                this.Db.Ingredients.Add(this.IngredientItem);
                result = this.Db.SaveChanges();
            }
            catch (Exception)
            {
            }



            if (result != 0)
            {
                HandleCancelIngredientCommand(null);
                this.Ingredients = new ObservableCollection<Ingredient>(this.Db.Ingredients.All().OrderBy(x => x.Name).ToList());
            }
        }

        private void HandleEditIngredientCommand(object parameter)
        {
            this.SwitchIngredientsToMode(IngredientsAdminMode.Update);
        }

        private void HandleUpdateIngredientCommand(object parameter)
        {
            int result = 0;
            if (!String.IsNullOrEmpty(this.IngredientItem.Name))
            {
                this.Db.Ingredients.Update(this.IngredientItem);
                result = this.Db.SaveChanges();
            }
            else
            {
                this.Ingredients = new ObservableCollection<Ingredient>(this.GetUowDataInstance().Ingredients.All().OrderBy(x => x.Name).ToList());
                HandleCancelIngredientCommand(null);
            }

            if (result != 0)
            {
                HandleCancelIngredientCommand(null);
                this.Ingredients = new ObservableCollection<Ingredient>(this.Db.Ingredients.All().OrderBy(x => x.Name).ToList());
            }

        }

        private void HandleDeleteIngredientCommand(object parameter)
        {
            this.Db.Ingredients.Delete(this.IngredientItem);

            int result = this.Db.SaveChanges();

            if (result != 0)
            {
                HandleCancelIngredientCommand(null);
                this.Ingredients = new ObservableCollection<Ingredient>(this.Db.Ingredients.All().OrderBy(x => x.Name).ToList());
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
                return this.units;
            }
            set
            {
                this.units = value;
                this.OnPropertyChanged("Units");
            }
        }

        private void HandleAddUnitCommand(object parameter)
        {
            int result = 0;
            try
            {
                this.Db.Units.Add(this.UnitItem);
                result = this.Db.SaveChanges();

            }
            catch (Exception)
            {
            }

            if (result != 0)
            {
                HandleCancelUnitCommand(null);
                this.Units = new ObservableCollection<Unit>(this.Db.Units.All().OrderBy(x => x.UnitName).ToList());

            }
        }

        private void HandleEditUnitCommand(object parameter)
        {
            this.SwitchUnitsToMode(UnitsAdminMode.Update);
        }

        private void HandleUpdateUnitCommand(object parameter)
        {
            int result = 0;
            if (!String.IsNullOrEmpty(this.UnitItem.UnitName) || !String.IsNullOrEmpty(this.UnitItem.UnitName))
            {
                this.Db.Units.Update(this.UnitItem);
                result = this.Db.SaveChanges();
            }
            else
            {
                this.Units = new ObservableCollection<Unit>(this.GetUowDataInstance().Units.All().OrderBy(x => x.UnitName).ToList());
                HandleCancelUnitCommand(null);
            }

            if (result != 0)
            {
                HandleCancelUnitCommand(null);
                this.Units = new ObservableCollection<Unit>(this.Db.Units.All().OrderBy(x => x.UnitName).ToList());
            }

        }

        private void HandleDeleteUnitCommand(object parameter)
        {
            this.Db.Units.Delete(this.UnitItem);

            int result = this.Db.SaveChanges();

            if (result != 0)
            {
                HandleCancelUnitCommand(null);
                this.Units = new ObservableCollection<Unit>(this.Db.Units.All().OrderBy(x => x.UnitName).ToList());
            }
        }

        private void HandleCancelUnitCommand(object parameter)
        {
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
                return this.sources;
            }
            set
            {
                this.sources = value;
                this.OnPropertyChanged("Sources");
            }
        }

        private string sourceError;

        public string SourceError
        {
            get { return sourceError; }
            set { sourceError = value; this.OnPropertyChanged("SourceError"); }
        }

        private void HandleAddSourceCommand(object parameter)
        {
            int result = 0;
            try
            {
                this.Db.Sources.Add(this.SourceItem);
                result = this.Db.SaveChanges();
            }
            catch (Exception)
            {

            }

            if (result != 0)
            {
                HandleCancelSourceCommand(null);
                this.Sources = new ObservableCollection<Source>(this.Db.Sources.All().OrderBy(x => x.Name).ToList());

            }
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
            this.SwitchSourcesToMode(SourcesAdminMode.Update);
        }

        private void HandleUpdateSourceCommand(object parameter)
        {
            int result = 0;
            if (!String.IsNullOrEmpty(this.SourceItem.Name))
            {
                this.Db.Sources.Update(this.SourceItem);
                result = this.Db.SaveChanges();
            }
            else
            {
                this.Sources = new ObservableCollection<Source>(this.GetUowDataInstance().Sources.All().OrderBy(x => x.Name).ToList());
                HandleCancelSourceCommand(null);
            }

            if (result != 0)
            {
                HandleCancelSourceCommand(null);
                this.Sources = new ObservableCollection<Source>(this.Db.Sources.All().OrderBy(x => x.Name).ToList());
            }

        }

        private void HandleDeleteSourceCommand(object parameter)
        {
            this.Db.Sources.Delete(this.SourceItem);

            int result = this.Db.SaveChanges();

            if (result != 0)
            {
                HandleCancelSourceCommand(null);
                this.Sources = new ObservableCollection<Source>(this.Db.Sources.All().OrderBy(x => x.Name).ToList());
            }
        }

        private void HandleCancelSourceCommand(object parameter)
        {
            SwitchSourcesToMode(SourcesAdminMode.View);
            this.SourceItem = new Source();
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

    }
}
