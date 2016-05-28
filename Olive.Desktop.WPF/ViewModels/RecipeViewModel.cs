namespace Olive.Desktop.WPF.ViewModels
{
    using System;
    using System.Linq;
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Windows;
    using System.Windows.Input;
    using Olive.Desktop.WPF.Behavior;
    using Microsoft.Win32;
    using Olive.Desktop.WPF.Helpers;
    using System.IO;
    using System.Windows.Media.Imaging;
    using System.Windows.Media;
    using System.Windows.Controls;
    using Olive.Data;

    public enum RecipeMode
    {
        View,
        Edit,
        Insert,
        Update
    }

    public class RecipeViewModel : ViewModelBase, IPageViewModel
    {
        #region TestBox
        private string testInfoMessage;

        public string TestInfoMessage
        {
            get { return this.testInfoMessage; }
            set { testInfoMessage = value; this.OnPropertyChanged("TestInfoMessage"); }
        }
        #endregion

        #region Fields
        private string pageName;
        private bool isEditMode;
        private bool isViewMode;
        private bool isInsertMode;
        private bool isUpdateMode;

        private int[] minutes;
        private int[] hours;
        private int[] servesArray;
        private string[] recommendationArray;

        int oldHoursPreparation = 0;
        int newHoursPreparation = 0;
        int oldMinutesPreparation = 0;
        int newMinutesPreparation = 0;

        int oldHoursCooking = 0;
        int newHoursCooking = 0;
        int oldMinutesCooking = 0;
        int newMinutesCooking = 0;

        private int preparationTime;
        private int cookingTime;

        private long recipeID;
        private string newImageName;
        private string oldImageName;
        private string newImageVersion;
        private string oldImageVersion;

        private Recipe originalRecipe;
        private Recipe recipe;
        private Category recipeCategory;

        private ObservableCollection<Category> parentCategories;
        private ObservableCollection<Source> sources;
        private ObservableCollection<Ingredient> ingredients;
        private ObservableCollection<Unit> units;

        private ICommand addImageCommand;
        private ICommand addNewRecipeItemCommand;
        private ICommand deleteRecipeItemCommand;
        private ICommand addNewIngredientCommand;
        private ICommand deleteIngredientCommand;
        private ICommand addNewStepCommand;
        private ICommand deleteStepCommand;
        private ICommand addNewRecipeCommand;
        private ICommand editRecipeCommand;
        private ICommand updateRecipeCommand;
        private ICommand deleteRecipeCommand;
        private ICommand cancelRecipeCommand;
        #endregion

        #region Constructors
        public RecipeViewModel()
        {
            this.IsViewMode = true;
            this.IsEditMode = false;
        }

        public RecipeViewModel(long recipeID, RecipeMode selectRecipeMode)
        {
            this.Hours = new int[] 
            {
                0,1,2,3,4,5,6,7,8,9,10,11,12
            };
            this.Minutes = new int[] 
            {
                0,5,10,15,20,25,30,35,40,45,50,55
            };
            this.ServesArray = new int[] 
            {
                1,2,3,4,5,6,7,8,9,10,11,12,13,14,15
            };
           
            this.RecommendationArray = new string[] 
            { 
                "Да","Не"
            };

            switch (selectRecipeMode)
            {
                case RecipeMode.View:
                    {
                        this.RecipeID = recipeID;
                        this.Name = this.Recipe.Title;

                        this.SwitchToMode(RecipeMode.View);

                        this.RecipeCategory = this.Recipe.Categories.ToList()[0];
                        this.OldImageName = this.Recipe.ImageURL;
                        this.OldImageVersion = this.Recipe.ImageVersion;

                        break;
                    }
                case RecipeMode.Edit:
                    {
                        this.RecipeID = recipeID;
                        this.Name = this.Recipe.Title;
                        this.SwitchToMode(RecipeMode.Edit);

                        this.RecipeCategory = this.Recipe.Categories.ToList()[0];

                        break;
                    }
                case RecipeMode.Insert:
                    {
                        this.SwitchToMode(RecipeMode.Insert);

                        this.recipe = new Recipe();
                        this.NewImageName = "null_image.png";
                        this.Recipe.ImageURL = this.RecipesImagesFolderPath + "null_image.png";
                        this.Recipe.RecipeItems.Add(new RecipeItem());

                        foreach (var item in this.Recipe.RecipeItems)
                        {
                            item.RecipeItems_Ingredients.Add(new RecipeItems_Ingredients());
                            item.Steps.Add(new Step());
                        }
                        break;
                    }
            }
        }
        #endregion

        #region Properties
        public string Name
        {
            get
            {
                return this.pageName;
            }
            set
            {
                this.pageName = value;
            }
        }

        public bool IsViewMode
        {
            get
            {
                return this.isViewMode;
            }
            set
            {
                this.isViewMode = value;
                this.OnPropertyChanged("IsViewMode");
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
                this.OnPropertyChanged("IsEditMode");
            }
        }

        public bool IsInsertMode
        {
            get
            {
                return this.isInsertMode;
            }
            set
            {
                this.isInsertMode = value;
                this.OnPropertyChanged("IsInsertMode");
            }
        }

        public bool IsUpdateMode
        {
            get
            {
                return this.isUpdateMode;
            }
            set
            {
                this.isUpdateMode = value;
                this.OnPropertyChanged("IsUpdateMode");
            }
        }

        public int[] Minutes
        {
            get
            {
                return this.minutes;
            }
            private set
            {
                this.minutes = value;
            }
        }

        public int[] Hours
        {
            get
            {
                return this.hours;
            }
            private set
            {
                this.hours = value;
            }
        }

        public int[] ServesArray
        {
            get { return servesArray; }
            set { servesArray = value; }
        }

        //public bool[] RecommendationArray
        //{
        //    get { return recommendationArray; }
        //    set { recommendationArray = value; }
        //}

        public string[] RecommendationArray
        {
            get { return recommendationArray; }
            set { recommendationArray = value; }
        }

        public long RecipeID
        {
            get
            {
                return this.recipeID;
            }
            set
            {
                this.recipeID = value;
                this.OnPropertyChanged("RecipeID");
            }
        }

        public string NewImageName
        {
            get
            {
                return this.newImageName;
            }
            set
            {
                this.newImageName = value;
                //this.OnPropertyChanged("NewImageName");
            }
        }

        public string OldImageName
        {
            get
            {
                return this.oldImageName;
            }
            set
            {
                this.oldImageName = value;
                //this.OnPropertyChanged("OldImageName");
            }
        }

        public string NewImageVersion
        {
            get { return newImageVersion; }
            set { newImageVersion = value; }
        }

        public string OldImageVersion
        {
            get { return oldImageVersion; }
            set { oldImageVersion = value; }
        }

        public Recipe OriginalRecipe
        {
            get
            {
                this.originalRecipe = this.Db.Recipes.GetById(this.RecipeID);
                //this.originalRecipe.ImageURL = Properties.Resources.RecipesImagesFolder + this.originalRecipe.ImageURL;
                this.originalRecipe.ImageURL = this.RecipesImagesFolderPath + this.originalRecipe.ImageURL;

                return this.originalRecipe;
            }
        }

        public Recipe Recipe
        {
            get
            {
                if (this.recipe == null)
                {
                    this.recipe = this.Db.Recipes.GetById(this.RecipeID);
                    try
                    {
                        ////this.recipe.ImageURL = Properties.Resources.RecipesImagesFolder + this.recipe.ImageURL;
                        //this.recipe.ImageURL = this.RecipesImagesFolderPath + this.recipe.ImageURL;
                    }
                    catch (Exception)
                    {
                        //TODO when press cancel in insert new recipe
                        MessageBox.Show("to fix");
                    }
                }

                return this.recipe;
            }
            set
            {
                this.recipe = value;
                this.OnPropertyChanged("Recipe");

            }
        }

        public Category RecipeCategory
        {
            get
            {
                var viewParentsCategories = this.GetDefaultView(this.ParentCategories);

                if (this.recipeCategory == null)
                {
                    this.recipeCategory = (viewParentsCategories.CurrentItem as Category);
                }

                return this.recipeCategory;
            }
            set
            {
                this.recipeCategory = value;

                this.OnPropertyChanged("RecipeCategory");
            }
        }

        public int PreparationTime
        {
            get
            {
                //this.preparationTime = -1;
                return this.preparationTime;
            }
            set
            {
                if (value > 55 || value == -1)
                {
                    if (value == -1)
                    {
                        oldHoursPreparation = 0;
                    }
                    else
                    {
                        oldHoursPreparation = value;
                    }

                    if (newHoursPreparation != oldHoursPreparation)
                    {
                        newHoursPreparation = oldHoursPreparation;
                    }
                }
                else
                {
                    oldMinutesPreparation = value;
                    if (newMinutesPreparation != oldMinutesPreparation)
                    {
                        newMinutesPreparation = oldMinutesPreparation;
                    }
                }
                this.preparationTime = newHoursPreparation + newMinutesPreparation;
                this.OnPropertyChanged("PreparationTime");
            }
        }

        public int CookingTime
        {
            get
            {
                return this.cookingTime;
            }
            set
            {
                if (value > 55 || value == -1)
                {
                    if (value == -1)
                    {
                        oldHoursCooking = 0;
                    }
                    else
                    {
                        oldHoursCooking = value;
                    }

                    if (newHoursCooking != oldHoursCooking)
                    {
                        newHoursCooking = oldHoursCooking;
                    }
                }
                else
                {
                    oldMinutesCooking = value;
                    if (newMinutesCooking != oldMinutesCooking)
                    {
                        newMinutesCooking = oldMinutesCooking;
                    }
                }
                this.cookingTime = newHoursCooking + newMinutesCooking;
                this.OnPropertyChanged("CookingTime");
            }
        }
        #endregion

        #region Properties_Observable
        public ObservableCollection<Category> ParentCategories
        {
            get
            {
                if (this.parentCategories == null)
                {
                    this.parentCategories = new ObservableCollection<Category>(this.Db.Categories.AllParentCategories().OrderBy(cat=>cat.Name).ToList());
                }
                return this.parentCategories;
            }
        }

        public ObservableCollection<Source> Sources
        {
            get
            {
                if (this.sources == null)
                {
                    this.sources = new ObservableCollection<Source>(this.Db.Sources.All().OrderBy(s=>s.Name).ToList());
                }
                return this.sources;
            }
        }

        public ObservableCollection<Ingredient> Ingredients
        {
            get
            {
                //if (this.ingredients == null)
                //{
                //    this.ingredients = new ObservableCollection<Ingredient>(this.Db.Ingredients.All().OrderBy(x => x.IngredientID).ToList());
                //}
                this.ingredients = new ObservableCollection<Ingredient>(this.Db.Ingredients.All().OrderBy(x => x.Name).ToList());
                return this.ingredients;
            }
        }

        public ObservableCollection<Unit> Units
        {
            get
            {
                //if (this.units == null)
                //{
                //    this.units = new ObservableCollection<Unit>(this.Db.Units.All().OrderBy(x => x.UnitShortName).ToList());
                //}
                this.units = new ObservableCollection<Unit>(this.Db.Units.All().OrderBy(x => x.UnitShortName).ToList());
                return this.units;
            }
        }
        #endregion

        #region Properties_Commands
        public ICommand AddImageCommand
        {
            get
            {
                if (this.addImageCommand == null)
                {
                    this.addImageCommand = new RelayCommand(this.HandleAddImageCommand);
                }
                return this.addImageCommand;
            }
        }

        public ICommand AddNewRecipeItemCommand
        {
            get
            {
                if (this.addNewRecipeItemCommand == null)
                {
                    this.addNewRecipeItemCommand = new RelayCommand(this.HandleAddNewRecipeItemCommand);
                }
                return this.addNewRecipeItemCommand;
            }
        }

        public ICommand DeleteRecipeItemCommand
        {
            get
            {
                if (this.deleteRecipeItemCommand == null)
                {
                    this.deleteRecipeItemCommand = new RelayCommand(this.HandleDeleteRecipeItemCommand);
                }
                return this.deleteRecipeItemCommand;
            }
        }

        public ICommand AddNewIngredientCommand
        {
            get
            {
                if (this.addNewIngredientCommand == null)
                {
                    this.addNewIngredientCommand = new RelayCommand(this.HandleAddNewIngredientCommand);
                }
                return this.addNewIngredientCommand;
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

        public ICommand AddNewStepCommand
        {
            get
            {
                if (this.addNewStepCommand == null)
                {
                    this.addNewStepCommand = new RelayCommand(this.HandleAddNewStepCommand);
                }
                return this.addNewStepCommand;
            }
        }

        public ICommand DeleteStepCommand
        {
            get
            {
                if (this.deleteStepCommand == null)
                {
                    this.deleteStepCommand = new RelayCommand(this.HandleDeleteStepCommand);
                }
                return this.deleteStepCommand;
            }
        }

        public ICommand AddNewRecipeCommand
        {
            get
            {
                if (this.addNewRecipeCommand == null)
                {
                    this.addNewRecipeCommand = new RelayCommand(this.HandleAddNewRecipeCommand);
                }
                return this.addNewRecipeCommand;
            }
        }

        public ICommand EditRecipeCommand
        {
            get
            {
                if (this.editRecipeCommand == null)
                {
                    this.editRecipeCommand = new RelayCommand(this.HandleEditRecipeCommand);
                }
                return this.editRecipeCommand;
            }
        }

        public ICommand UpdateRecipeCommand
        {
            get
            {
                if (this.updateRecipeCommand == null)
                {
                    this.updateRecipeCommand = new RelayCommand(this.HandleUpdateRecipeCommand);
                }
                return this.updateRecipeCommand;
            }
        }

        public ICommand DeleteRecipeCommand
        {
            get
            {
                if (this.deleteRecipeCommand == null)
                {
                    this.deleteRecipeCommand = new RelayCommand(this.HandleDeleteRecipeCommand);
                }
                return this.deleteRecipeCommand;
            }
        }

        public ICommand CancelRecipeCommand
        {
            get
            {
                if (this.cancelRecipeCommand == null)
                {
                    this.cancelRecipeCommand = new RelayCommand(this.HandleCancelRecipeCommand);
                }
                return this.cancelRecipeCommand;
            }
        }
        #endregion

        #region Methods_HandlersCommands
        private void HandleAddImageCommand(object parameter)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.Multiselect = false;
            fileDialog.ShowDialog();

            if (!fileDialog.FileName.Any())
            {
                return;
            }
            else
            {
                //Get picture name without path
                FileInfo fileInfo = new FileInfo(fileDialog.FileName);
                string nameWithoutPath = fileInfo.Name;

                //Get pictrue with path
                this.NewImageName = nameWithoutPath;
                this.Recipe.ImageURL = fileDialog.FileName;
                Image img = parameter as Image;
                img.Source = this.GetImageFromPath(fileDialog.FileName);
            }

            
        }

        private void HandleAddNewRecipeItemCommand(object parameter)
        {
            RecipeItem newRecipeItem = new RecipeItem();

            newRecipeItem.RecipeItems_Ingredients.Add(new RecipeItems_Ingredients());
            newRecipeItem.Steps.Add(new Step());

            this.Recipe.RecipeItems.Add(newRecipeItem);
        }

        private void HandleDeleteRecipeItemCommand(object parameter)
        {
            RecipeItem selectedRecipeItem = parameter as RecipeItem;
            this.Recipe.RecipeItems.Remove(selectedRecipeItem);
        }

        private void HandleAddNewIngredientCommand(object parameter)
        {
            RecipeItem selectedRecipeItem = parameter as RecipeItem;

            selectedRecipeItem.RecipeItems_Ingredients.Add(new RecipeItems_Ingredients());
            this.OnPropertyChanged("Recipe");
        }

        private void HandleDeleteIngredientCommand(object parameter)
        {
            RecipeItems_Ingredients selectedIngredient = parameter as RecipeItems_Ingredients;

            selectedIngredient.RecipeItem.RecipeItems_Ingredients.Remove(selectedIngredient);
        }

        private void HandleAddNewStepCommand(object parameter)
        {
            RecipeItem selectedrecipeItem = parameter as RecipeItem;
            selectedrecipeItem.Steps.Add(new Step());
        }

        private void HandleDeleteStepCommand(object parameter)
        {
            Step selectedStep = parameter as Step;
            selectedStep.RecipeItem.Steps.Remove(selectedStep);
        }

        private void HandleAddNewRecipeCommand(object parameter)
        {
            bool hasError = false;

            if (!this.NewImageName.Equals("null_image.png"))
            {
                string imageStoreFolder = this.RecipesImagesFolderPath;
                try
                {
                    File.Copy(this.Recipe.ImageURL, imageStoreFolder + this.NewImageName);
                    this.Recipe.ImageVersion=this.Db.Recipes.UploadCloudinaryRecipeImage(this.Recipe.ImageURL, this.NewImageName);
                    this.Recipe.ImageURL = this.NewImageName;
                }
                catch (Exception)
                {
                    MessageBox.Show("Вече съществува такава снимка");
                    hasError = true;
                }
            }
            else
            {
                this.Recipe.ImageURL = this.NewImageName;
            }

            if (!hasError)
            {
                this.Recipe.Categories.Add(this.RecipeCategory);
                this.Recipe.PreparationTime = this.PreparationTime;
                this.Recipe.CookingTime = this.CookingTime;
                this.Recipe.NumberOfHits = 1;
                this.Recipe.NumberOfLikes = 1;

                this.Db.Recipes.Add(this.Recipe);
                int result = this.Db.SaveChanges();

                if (result != 0)
                {
                    MessageBox.Show("Усщешно добавихте нова рецепта");
                    this.RecipeID = this.Recipe.RecipeID;
                    RaiseNewRecipeChanged(this.Recipe.RecipeID);
                }
            }
        }

        private void HandleEditRecipeCommand(object parameter)
        {
            this.SwitchToMode(RecipeMode.Update);
            this.RecipeCategory = this.Recipe.Categories.ToList()[0];
            this.PreparationTime = (int)this.Recipe.PreparationTime;
            this.CookingTime = (int)this.Recipe.CookingTime;
            this.NewImageName = this.Recipe.ImageURL;
            this.OldImageName = this.Recipe.ImageURL;
        }

        private void HandleUpdateRecipeCommand(object parameter)
        {
            var recipe = this.Db.Recipes.All()
                        .Include(r => r.Categories)
                        .Include(r => r.RecipeItems)
                        .FirstOrDefault(r => r.RecipeID == this.RecipeID);

            if (!this.NewImageName.Equals(this.OldImageName))
            {
                string imageStoreFolder = this.RecipesImagesFolderPath;

                try
                {
                    //add new image to RecipesImages(now exist two images old and new)
                    File.Copy(this.Recipe.ImageURL, imageStoreFolder + this.NewImageName);

                    //add new image to Cloudinary(now exist two images old and new)
                    this.Recipe.ImageVersion = this.Db.Recipes.UploadCloudinaryRecipeImage(this.Recipe.ImageURL, this.NewImageName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Снимката със същото име вече съществува");
                }
                finally
                {
                    this.Recipe.ImageURL = this.NewImageName;
                }
            }
            else
            {
                FileInfo fileInfo = new FileInfo(this.OldImageName);
                string nameWithoutPath = fileInfo.Name;
                this.Recipe.ImageURL = nameWithoutPath;
            }

            if (this.Recipe.Categories.Count != 0)
            {
                this.Recipe.Categories.Remove(this.Recipe.Categories.FirstOrDefault());
                this.Recipe.Categories.Add(this.RecipeCategory);
            }

            this.Recipe.PreparationTime = this.PreparationTime;
            this.Recipe.CookingTime = this.CookingTime;

            recipe = this.Recipe;

            this.Db.Recipes.Update(recipe);


            int result = this.Db.SaveChanges();

            #region Delete nullable recipeItems/recipeIngredients/steps

            var nullRecipeItems = this.Db.RecipeItems.All()
                                .Include(r => r.Steps)
                                .Include(r => r.RecipeItems_Ingredients)
                                .Where(r => r.RecipeID == null).Select(r => r);

            var nullSteps = this.Db.Steps.All()
                            .Where(s => s.RecipeItemID == null).Select(s => s);

            var nullRecipeIngredients = this.Db.RecipeItems_Ingredients.All()
                            .Where(ri => ri.RecipeItemID == null).Select(ri => ri);

            foreach (var nullRecipeItem in nullRecipeItems)
            {
                this.Db.RecipeItems.Delete(nullRecipeItem);
            }

            foreach (var nullRecipeIngredient in nullRecipeIngredients)
            {
                this.Db.RecipeItems_Ingredients.Delete(nullRecipeIngredient);
            }

            foreach (var nullStep in nullSteps)
            {
                this.Db.Steps.Delete(nullStep);
            }
            this.Db.SaveChanges();
            #endregion

            if (result != 0)
            {
                if (!this.NewImageName.Equals(this.OldImageName))
                {
                    try
                    {
                        //System.GC.Collect();
                        //System.GC.WaitForPendingFinalizers();
                        if (!this.OldImageName.Equals(this.RecipesImagesFolderPath + "null_image.png"))
                        {
                            //delete old image from RecipesImages(now exist one images the new)
                            File.Delete(this.OldImageName);

                            //delete old image from Cloudinary(now exist one images the new)
                            this.Db.Recipes.DeleteCloudinaryRecipeImage(this.OldImageName);
                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }
                }
                MessageBox.Show("Усщешно променихте рецепта");
                RaiseUpdateRecipeChanged(recipe.RecipeID);
            }
        }

        private void HandleDeleteRecipeCommand(object parameter)
        {
            var recipe = this.Db.Recipes.All()
                        .Include(r => r.Categories)
                        .Include(r => r.RecipeItems)
                        .FirstOrDefault(r => r.RecipeID == this.RecipeID);

            var recipeItems = recipe.RecipeItems;

            this.Recipe.ImageURL = this.RecipesImagesFolderPath + "null_image.png";


            foreach (var recipeItem in recipeItems.ToList())
            {
                foreach (var step in recipeItem.Steps.ToList())
                {
                    this.Db.Steps.Delete(step);
                }
                // TODO RiI 7
                foreach (var ingredient in recipeItem.RecipeItems_Ingredients.ToList())
                {
                    this.Db.RecipeItems_Ingredients.Delete(ingredient);
                }
                this.Db.RecipeItems.Delete(recipeItem);
            }


            this.Db.Recipes.Delete(recipe);

            int result = this.Db.SaveChanges();

            if (result != 0)
            {
                MessageBox.Show("Усщешно изтрихте рецепта");
                RaiseDeleteRecipeChanged();
                try
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();

                    if (!this.OldImageName.Equals(this.RecipesImagesFolderPath+"null_image.png"))
                    {
                        File.Delete(this.OldImageName);
                        this.Db.Recipes.DeleteCloudinaryRecipeImage(this.OldImageName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void HandleCancelRecipeCommand(object parameter)
        {
            if (IsInsertMode)
            {
                RaiseCancelInsertModeChanged();
            }
            else
            {
                this.SwitchToMode(RecipeMode.View);

                this.Db = this.GetUowDataInstance();
                this.Recipe = this.Db.Recipes.GetById(this.RecipeID);
                this.Recipe.ImageURL = this.RecipesImagesFolderPath + this.Recipe.ImageURL;
            }

        }
        #endregion

        #region Events
        public event EventHandler<SelectedRecipeChangedArgs> NewRecipeChanged;

        private void RaiseNewRecipeChanged(long newRecipeID)
        {
            if (this.NewRecipeChanged != null)
            {
                this.NewRecipeChanged(this, new SelectedRecipeChangedArgs(newRecipeID));
            }
        }

        public event EventHandler<EventArgs> DeleteRecipeChanged;

        private void RaiseDeleteRecipeChanged()
        {
            if (this.DeleteRecipeChanged != null)
            {
                this.DeleteRecipeChanged(this, new EventArgs());
            }
        }

        public event EventHandler<SelectedRecipeChangedArgs> UpdateRecipeChanged;

        private void RaiseUpdateRecipeChanged(long updateRecipeID)
        {
            if (this.UpdateRecipeChanged != null)
            {
                this.UpdateRecipeChanged(this, new SelectedRecipeChangedArgs(updateRecipeID));
            }
        }

        public event EventHandler<EventArgs> CancelInsertModeChanged;

        private void RaiseCancelInsertModeChanged()
        {
            if (this.CancelInsertModeChanged != null)
            {
                this.CancelInsertModeChanged(this, new EventArgs());
            }
        }
        #endregion

        private void SwitchToMode(RecipeMode selectRecipeMode)
        {
            switch (selectRecipeMode)
            {
                case RecipeMode.View:
                    {
                        this.IsViewMode = true;
                        this.IsEditMode = false;
                        this.IsInsertMode = false;
                        this.IsUpdateMode = false;
                        break;
                    }
                case RecipeMode.Edit:
                    {
                        this.IsEditMode = true;
                        this.IsViewMode = false;
                        break;
                    }
                case RecipeMode.Insert:
                    {
                        this.IsViewMode = false;
                        this.IsEditMode = true;
                        this.IsInsertMode = true;
                        this.IsUpdateMode = false;
                        break;
                    }
                case RecipeMode.Update:
                    {
                        this.IsViewMode = false;
                        this.IsEditMode = true;
                        this.IsInsertMode = false;
                        this.IsUpdateMode = true;
                        break;
                    }
            }
        }

        private ImageSource GetImageFromPath(string imagePath)
        {
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.CacheOption = BitmapCacheOption.OnLoad;
            Uri imageSource = new Uri(imagePath);
            img.UriSource = imageSource;
            img.EndInit();
            return img;
        }

        #region Settings
        private string recipesImagesFolderPath;

        public string RecipesImagesFolderPath
        {
            get
            {
                int isDebug = this.AssemblyPath.IndexOf("Debug");
                if (isDebug != -1)
                {
                    this.recipesImagesFolderPath = this.AssemblyPath.Replace(@"bin\Debug", @"Images\");
                }
                else
                {
                    this.recipesImagesFolderPath = this.AssemblyPath.Replace(@"Application", @"RecipesImages\");
                }

                return this.recipesImagesFolderPath;
            }
            set
            {
                this.recipesImagesFolderPath = value;
                this.OnPropertyChanged("RecipesImagesFolderPath");
            }
        }
        #endregion
    }
}
