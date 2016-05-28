using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Olive.Data;
using System.Linq.Expressions;
using Olive.Web.MVC.ViewModels;
using Olive.Web.MVC.Areas.Administration.ViewModels;
using Olive.Data.Uow;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using Olive.Web.MVC.Helpers;
using Olive.Web.MVC.ActionFilters;
using CloudinaryDotNet;
using System.Configuration;
using CloudinaryDotNet.Actions;
using System.IO;

namespace Olive.Web.MVC.Areas.Administration.Controllers
{

    public class AdminRecipesController : BaseAdminController
    {
        //protected static readonly Expression<Func<Recipe, AdminRecipeIndexViewModel>> AsAdminRecipeIndexViewModel =
        //entity => new AdminRecipeIndexViewModel
        //{
        //    RecipeID = entity.RecipeID,
        //    Title = entity.Title,
        //    ImageURL = entity.ImageURL,
        //    PreparationTime = (int)entity.PreparationTime,
        //    CookingTime = (int)entity.CookingTime,
        //    Serves = (int)entity.Serves,
        //    Date = (DateTime)entity.Date,
        //    Rating = (int)entity.Rating,
        //    NumberOfHits = (int)entity.NumberOfHits,
        //    NumberOfLikes = (int)entity.NumberOfLikes,
        //    Category = entity.Categories.FirstOrDefault().Name,
        //    ParentCategory = entity.Categories.Select(x => x.Category1.Name).FirstOrDefault()
        //};

        private UowData db = new UowData();
        private AdminRecipeViewModel newRecipeVM;
        private AdminRecipeViewModel editRecipeVM;
        private int[] minutes;
        private int[] hours;
        private int[] servesArray;
        private int[] rating;
        private string[] recommendationArray;

        public AdminRecipesController()
        {
            this.hours = new int[] 
            {
                0,1,2,3,4,5,6,7,8,9,10,11,12
            };
            this.minutes = new int[] 
            {
                0,5,10,15,20,25,30,35,40,45,50,55
            };
            this.servesArray = new int[] 
            {
                1,2,3,4,5,6,7,8,9,10,11,12,13,14,15
            };
            this.rating = new int[] 
            {
                1,2,3,4,5
            };

            this.recommendationArray = new string[] 
            { 
                "Да","Не"
            };

            this.newRecipeVM = new AdminRecipeViewModel();
            //newRecipeVM.Recipe = new Recipe();
            //List<Step> stepsList = new List<Step>();
            //stepsList.Add(new Step());
            //List<RecipeItems_Ingredients> ingredientsList = new List<RecipeItems_Ingredients>();
            //ingredientsList.Add(new RecipeItems_Ingredients());
            //newRecipeVM.Recipe.RecipeItems.Add(new RecipeItem() { Steps = stepsList, RecipeItems_Ingredients = ingredientsList });
            //newRecipeVM.Recipe.Date = DateTime.Now;
            //newRecipeVM.Title = "";
            newRecipeVM.ImageURL = @"http://res.cloudinary.com/hqz5ohs1v/image/upload/v1413825708/null_image.png";
            newRecipeVM.ParentCategories = db.Categories.AllParentCategories();
            newRecipeVM.ChildrenCategories = db.Categories.AllChildrenCategories();
            newRecipeVM.PrepTimeHours = this.hours;
            newRecipeVM.SelectedPrepTimeHours = -1;
            newRecipeVM.PrepTimeMinutes = this.minutes;
            newRecipeVM.SelectedPrepTimeMinutes = -1;
            newRecipeVM.CookTimeHours = this.hours;
            newRecipeVM.SelectedCookTimeHours = -1;
            newRecipeVM.CookTimeMinutes = this.minutes;
            newRecipeVM.SelectedCookTimeMinutes = -1;
            newRecipeVM.Serves = this.servesArray;
            newRecipeVM.SelectedServes = 0;
            newRecipeVM.Rating = this.rating;
            newRecipeVM.SelectedRating = 0;
            newRecipeVM.Sources = this.db.Sources.All();
            newRecipeVM.SelectedSource = 0;
            newRecipeVM.Recommendation = this.recommendationArray.ToList();
            newRecipeVM.SelectedRecommendation = "--- избери дали да е препоръчана ---";
            newRecipeVM.PublishedDate = DateTime.Now;


            ViewBag.UnitsList = this.db.Units.All().ToList();
            ViewBag.IngredientsList = this.db.Ingredients.All().ToList();

            RecipeItem recipeItem = new RecipeItem();
            recipeItem.Steps = new List<Step>();
            recipeItem.RecipeItems_Ingredients = new List<RecipeItems_Ingredients>();
            newRecipeVM.RecipeItems.Add(recipeItem);
        }

        //
        // GET: /Administration/Recipes/
        public ViewResult Index()
        {
            //var recipes = db.Recipes.Include(r => r.Source);
            var recipes = db.Recipes.All();
            return View(recipes.ToList());
        }

        // GET: /Administration/Recipes/Details/5
        public ViewResult Details(int id)
        {
            Recipe recipe = db.Recipes.GetById(id);
            return View(recipe);
        }

        // GET: /Administration/Recipes/Create
        public ActionResult Create()
        {
            return View(this.newRecipeVM);
        }

        // POST: /Administration/Recipes/Create
        [HttpPost]
        public ActionResult Create(AdminRecipeViewModel recipeModel)
        {
            //HttpPostedFileBase img = recipeModel.Image;
            //UploadRecipeImage(recipeModel.Image,recipeModel.Title);
            if (ModelState.IsValid)
            {
                Recipe newRecipe = ParseRecipeVMToRecipeModel(recipeModel);

                this.db.Recipes.Add(newRecipe);
                int result = this.db.SaveChanges();

                if (result != 0)
                {
                    return RedirectToAction("Index");
                }
            }
            this.newRecipeVM.RecipeItems = recipeModel.RecipeItems;
            return View(this.newRecipeVM);
        }


        // GET: /Administration/Recipes/Edit/5
        //[AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id)
        {
            //ViewData["RecipeItemID"] = Request.Form["recipeItems.index"];
            Recipe recipeForEdit = db.Recipes.GetById(id);
            this.editRecipeVM = ParseRecipeModelToRecipeVM(recipeForEdit);
            return View(this.editRecipeVM);
        }

        // POST: /Administration/Recipes/Edit/5
        [HttpPost]
        public ActionResult Edit(AdminRecipeViewModel recipeEditModel)
        {
            //if (ModelState.IsValid)
            //{

            //}
            #region Delete old RecipeItems
            var recipe = this.db.Recipes.All()
                    .Include(r => r.Categories)
                    .Include(r => r.RecipeItems)
                    .FirstOrDefault(r => r.RecipeID == recipeEditModel.RecipeID);
            var recipeItems = recipe.RecipeItems;
            foreach (var recipeItem in recipeItems.ToList())
            {
                foreach (var step in recipeItem.Steps.ToList())
                {
                    this.db.Steps.Delete(step);
                }
                foreach (var ingredient in recipeItem.RecipeItems_Ingredients.ToList())
                {
                    this.db.RecipeItems_Ingredients.Delete(ingredient);
                }
                this.db.RecipeItems.Delete(recipeItem);
            }

            db.SaveChanges();
            #endregion
            Recipe recipeForEdit = this.db.Recipes.GetById(recipeEditModel.RecipeID);
            recipeForEdit = ParseRecipeVMToRecipeModel_Update(recipeForEdit, recipeEditModel);

            this.db.Recipes.Update(recipeForEdit);
            int result = this.db.SaveChanges();

            

            if (result != 0)
            {
                return RedirectToAction("Details", "Recipes", new { recipeID = recipeEditModel.RecipeID, title = recipeEditModel.Title.Replace(' ', '-'), area = "" });
            }

            //ViewData["RecipeItemID"] = Request.Form["recipeItems.index"];
            recipeEditModel.ParentCategories = db.Categories.AllParentCategories();
            recipeEditModel.ChildrenCategories = db.Categories.AllChildrenCategories();
            recipeEditModel.PrepTimeHours = this.hours;
            recipeEditModel.PrepTimeMinutes = this.minutes;
            recipeEditModel.CookTimeHours = this.hours;
            recipeEditModel.CookTimeMinutes = this.minutes;
            recipeEditModel.Serves = this.servesArray;
            recipeEditModel.Rating = this.rating;
            recipeEditModel.Sources = this.db.Sources.All();
            recipeEditModel.Recommendation = this.recommendationArray;
            return View(recipeEditModel);
        }

        public ActionResult Delete(int id)
        {
            //Recipe recipe = db.Recipes.Find(id);
            Recipe recipe = db.Recipes.GetById(id);
            return View(recipe);
        }

        //
        // POST: /Administration/Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var recipe = this.db.Recipes.All()
                        .Include(r => r.Categories)
                        .Include(r => r.RecipeItems)
                        .FirstOrDefault(r => r.RecipeID == id);
            var recipeItems = recipe.RecipeItems;
            foreach (var recipeItem in recipeItems.ToList())
            {
                foreach (var step in recipeItem.Steps.ToList())
                {
                    this.db.Steps.Delete(step);
                }
                foreach (var ingredient in recipeItem.RecipeItems_Ingredients.ToList())
                {
                    this.db.RecipeItems_Ingredients.Delete(ingredient);
                }
                this.db.RecipeItems.Delete(recipeItem);
            }

            db.Recipes.Delete(recipe);
            int result=db.SaveChanges();
            if (result!=0)
            {
                DeleteRecipeImage(recipe.ImageURL);
            }
            return RedirectToAction("Index");
        }

        public ActionResult GetChildrenCategories(int? id)
        {
            var childrenCategories = db.Categories.GetChildrenCategoriesByParentID((int)id);
            var childrenCategoriesJSON = childrenCategories.Select(x => new
            {
                ID = x.CategoryID,
                Name = x.Name
            });
            //Add JsonRequest behavior to allow retrieving states over http get
            return Json(childrenCategoriesJSON, JsonRequestBehavior.AllowGet);
            //return Content(childrenCategoriesJSON.ToString(), "application/json");
        }

        public ActionResult AddIngredient(string recipeItemID)
        {
            RecipeItems_Ingredients recipeItemIngredient = new RecipeItems_Ingredients();
            recipeItemIngredient.Quantity = 0;
            recipeItemIngredient.Unit = new Unit();
            recipeItemIngredient.Ingredient = new Ingredient();
            //ViewBag.UnitsList = this.db.Units.All().ToList();
            if (Request.IsAjaxRequest())
            {
                //ViewBag.UnitsList = this.db.Units.All().ToList();
                //ViewBag.IngredientsList = this.db.Ingredients.All().ToList();
                ViewBag.RecipeItemID = recipeItemID;
                ViewData["RecipeItemID"] = recipeItemID;
                return PartialView("_IngredientEditor", recipeItemIngredient);
                //return PartialView("_StepEditor", newStep, new ViewDataDictionary { { "RecipeItemID", recipeItemID } });
            }
            else
            {
                return RedirectToAction("Create");
                //return PartialView("_StepPartial", newStep);
            }
        }

        public ActionResult AddStep(string recipeItemID)
        {
            Step newStep = new Step();
            if (Request.IsAjaxRequest())
            {
                ViewBag.RecipeItemID = recipeItemID;
                ViewData["RecipeItemID"] = recipeItemID;
                return PartialView("_StepEditor", newStep);
                //return PartialView("_StepEditor", newStep, new ViewDataDictionary { { "RecipeItemID", recipeItemID } });
            }
            else
            {
                return RedirectToAction("Create");
                //return PartialView("_StepPartial", newStep);
            }
        }

        public ActionResult AddSection(AdminRecipeViewModel recipeModel)
        {
            if (Request.IsAjaxRequest())
            {
                RecipeItem recipeItem = new RecipeItem();
                recipeItem.Steps = new List<Step>();
                //recipeItem.Steps.Add(new Step() { Description = "bbb" });
                newRecipeVM.RecipeItems.Add(recipeItem);
                return PartialView("_RecipeItemEditor", recipeItem);
            }
            else
            {
                //return RedirectToAction("Create");
                //return PartialView("_StepPartial", newStep);
                return View(@"~/Areas/Administration/Views/AdminRecipes/Create.cshtml", this.newRecipeVM);
            }
        }

        //For Create Action
        private Recipe ParseRecipeVMToRecipeModel(AdminRecipeViewModel recipeVM)
        {
            Recipe newRecipe = new Recipe();
            HMLToRTFConverter converter = new HMLToRTFConverter();
            ImageUploadResult recipeImgUploadResult = new ImageUploadResult();

            newRecipe.Title = recipeVM.Title;
            Category newRecipeParentCategory = this.db.Categories.GetById(recipeVM.SelectedParentCategory);
            newRecipe.Categories.Add(newRecipeParentCategory);
            Category newRecipeChildrenCategory = null;
            if (recipeVM.SelectedChildrenCategory != null)
            {
                newRecipeChildrenCategory = this.db.Categories.GetById((int)(recipeVM.SelectedChildrenCategory));
                newRecipe.Categories.Add(newRecipeChildrenCategory);
            }
            if ((recipeVM.Image!=null&&(recipeVM.Image.FileName != "null_image.png" || recipeVM.Image.FileName != "null_image")))
            {
                if (newRecipeChildrenCategory == null)
                {
                    recipeImgUploadResult = UploadRecipeImage(recipeVM.Image, recipeVM.Title, newRecipeParentCategory.Name, null);

                }
                else
                {
                    recipeImgUploadResult = UploadRecipeImage(recipeVM.Image, recipeVM.Title, newRecipeParentCategory.Name, newRecipeChildrenCategory.Name);
                }
                newRecipe.ImageVersion = "v" + recipeImgUploadResult.Version;
                newRecipe.ImageURL = "http://res.cloudinary.com/" + recipeImgUploadResult.Uri.AbsolutePath;
                //newRecipe.ImageURL = recipeVM.Title.Replace(" ", "-") + ".jpg";
            }
            else
            {
                newRecipe.ImageURL = "http://res.cloudinary.com/hqz5ohs1v/image/upload/v1413825708/null_image.png";
            }
            newRecipe.PreparationTime = recipeVM.SelectedPrepTimeHours * 60 + recipeVM.SelectedPrepTimeMinutes;
            newRecipe.CookingTime = recipeVM.SelectedCookTimeHours * 60 + recipeVM.SelectedCookTimeMinutes;
            newRecipe.Serves = recipeVM.SelectedServes;
            newRecipe.Rating = recipeVM.SelectedRating;
            newRecipe.Source = this.db.Sources.GetById(recipeVM.SelectedSource);
            if (recipeVM.SelectedRecommendation == "Да")
            {
                newRecipe.Recommended = true;
            }
            //if (recipeVM.SelectedRecommendation == "Не")
            else
            {
                newRecipe.Recommended = false;
            }
            DateTime dt = recipeVM.PublishedDate;
            dt.AddHours(DateTime.Now.Hour);
            dt.AddMinutes(DateTime.Now.Minute);
            dt.AddSeconds(DateTime.Now.Second);
            newRecipe.Date = dt;
            //newRecipe.Date = recipeVM.PublishedDate;
            newRecipe.Description = converter.ConvertHTMLToRTF(recipeVM.Description);
            newRecipe.NumberOfHits = 1;
            newRecipe.NumberOfLikes = 1;

            foreach (var recipeItem in recipeVM.RecipeItems)
            {
                RecipeItem newRecipeItem = new RecipeItem();
                newRecipeItem.RecipeItemName = recipeItem.RecipeItemName;
                foreach (var recipeItem_ingredient in recipeItem.RecipeItems_Ingredients)
                {
                    RecipeItems_Ingredients newRecipeItem_ingredient = new RecipeItems_Ingredients();
                    Unit newUnit = this.db.Units.GetById(recipeItem_ingredient.Unit.UnitID);
                    Ingredient newIngredient = this.db.Ingredients.GetById(recipeItem_ingredient.Ingredient.IngredientID);
                    newRecipeItem_ingredient.Quantity = recipeItem_ingredient.Quantity;
                    newRecipeItem_ingredient.Unit = newUnit;
                    newRecipeItem_ingredient.Ingredient = newIngredient;
                    newRecipeItem.RecipeItems_Ingredients.Add(newRecipeItem_ingredient);
                }
                foreach (var step in recipeItem.Steps)
                {
                    newRecipeItem.Steps.Add(step);
                }

                newRecipe.RecipeItems.Add(newRecipeItem);
            }

            //newRecipe.RecipeItems = recipeVM.RecipeItems;

            return newRecipe;
        }

        //For GET Edit Action
        private AdminRecipeViewModel ParseRecipeModelToRecipeVM(Recipe recipeForEdit)
        {
            AdminRecipeViewModel editRecipeVM = new AdminRecipeViewModel();

            RTFToHTMLConverter converter = new RTFToHTMLConverter();
            editRecipeVM.RecipeID = recipeForEdit.RecipeID;
            editRecipeVM.Title = recipeForEdit.Title;
            editRecipeVM.ImageURL = recipeForEdit.ImageURL;
            editRecipeVM.ParentCategories = db.Categories.AllParentCategories();
            editRecipeVM.SelectedParentCategory = recipeForEdit.Categories.Where(cat => cat.ParentCategoryID == null).Select(cat => cat.CategoryID).FirstOrDefault();
            editRecipeVM.ChildrenCategories = db.Categories.AllChildrenCategories();
            editRecipeVM.SelectedChildrenCategory = recipeForEdit.Categories.Where(cat => cat.ParentCategoryID != null).Select(cat => cat.CategoryID).FirstOrDefault();
            editRecipeVM.PrepTimeHours = this.hours.ToList();
            editRecipeVM.SelectedPrepTimeHours = (int)recipeForEdit.PreparationTime / 60;
            editRecipeVM.PrepTimeMinutes = this.minutes;
            editRecipeVM.SelectedPrepTimeMinutes = (int)recipeForEdit.PreparationTime % 60;
            editRecipeVM.CookTimeHours = this.hours;
            editRecipeVM.SelectedCookTimeHours = (int)recipeForEdit.CookingTime / 60;
            editRecipeVM.CookTimeMinutes = this.minutes;
            editRecipeVM.SelectedCookTimeMinutes = (int)recipeForEdit.CookingTime % 60;
            editRecipeVM.Serves = this.servesArray;
            editRecipeVM.SelectedServes = (int)recipeForEdit.Serves;
            editRecipeVM.Rating = this.rating;
            editRecipeVM.SelectedRating = (int)recipeForEdit.Rating;
            editRecipeVM.Sources = this.db.Sources.All();
            editRecipeVM.SelectedSource = recipeForEdit.Source.SourceID;
            editRecipeVM.Recommendation = this.recommendationArray;
            //editRecipeVM.SelectedRecommendation = recipeForEdit.Recommended;
            if (recipeForEdit.Recommended == true)
            {
                editRecipeVM.SelectedRecommendation = "Да";
            }
            else
            {
                editRecipeVM.SelectedRecommendation = "Не";
            }
            editRecipeVM.PublishedDate = (DateTime)recipeForEdit.Date;
            editRecipeVM.Description = RtfHelper.PlainText(recipeForEdit.Description);
            //editRecipeVM.RecipeItems = recipeForEdit.RecipeItems.ToList();

            foreach (var recipeItem in recipeForEdit.RecipeItems)
            {
                RecipeItem newRecipeItem = new RecipeItem();
                newRecipeItem.RecipeItemName = recipeItem.RecipeItemName;
                foreach (var recipeItem_ingredient in recipeItem.RecipeItems_Ingredients)
                {
                    RecipeItems_Ingredients newRecipeItem_ingredient = new RecipeItems_Ingredients();
                    Unit newUnit = this.db.Units.GetById(recipeItem_ingredient.Unit.UnitID);
                    Ingredient newIngredient = this.db.Ingredients.GetById(recipeItem_ingredient.Ingredient.IngredientID);
                    newRecipeItem_ingredient.Quantity = recipeItem_ingredient.Quantity;
                    newRecipeItem_ingredient.Unit = newUnit;
                    newRecipeItem_ingredient.Ingredient = newIngredient;
                    newRecipeItem.RecipeItems_Ingredients.Add(newRecipeItem_ingredient);
                }
                foreach (var step in recipeItem.Steps)
                {
                    newRecipeItem.Steps.Add(step);
                }

                editRecipeVM.RecipeItems.Add(newRecipeItem);
            }

            return editRecipeVM;
        }

        //For POST Edit Action
        private Recipe ParseRecipeVMToRecipeModel_Update(Recipe recipeModel, AdminRecipeViewModel recipeVM)
        {
            Recipe recipeForEdit = recipeModel;
            ImageUploadResult recipeImgUploadResult = new ImageUploadResult();
            //var recipeForEdit = this.db.Recipes.All()
            //            .Include(r => r.Categories)
            //            .Include(r => r.RecipeItems)
            //            .FirstOrDefault(r => r.RecipeID == recipeVM.RecipeID);
            HMLToRTFConverter converter = new HMLToRTFConverter();

            recipeForEdit.Title = recipeVM.Title;

            //recipeForEdit.ImageURL = "null_image.png";
            Category newRecipeParentCategory = this.db.Categories.GetById(recipeVM.SelectedParentCategory);
            recipeForEdit.Categories.Clear();
            recipeForEdit.Categories.Add(newRecipeParentCategory);
            Category newRecipeChildrenCategory = null;
            if (recipeVM.SelectedChildrenCategory != null)
            {
                newRecipeChildrenCategory = this.db.Categories.GetById((int)(recipeVM.SelectedChildrenCategory));
                recipeForEdit.Categories.Add(newRecipeChildrenCategory);
            }
            if (recipeVM.Image != null && (recipeVM.Image.FileName != "null_image.png" || recipeVM.Image.FileName != "null_image"))
            {
                if (newRecipeChildrenCategory == null)
                {
                    DeleteRecipeImage(recipeVM.ImageURL);
                    recipeImgUploadResult = UploadRecipeImage(recipeVM.Image, recipeVM.Title, newRecipeParentCategory.Name, null);
                }
                else
                {
                    DeleteRecipeImage(recipeVM.ImageURL);
                    recipeImgUploadResult = UploadRecipeImage(recipeVM.Image, recipeVM.Title, newRecipeParentCategory.Name, newRecipeChildrenCategory.Name);
                }
                recipeForEdit.ImageVersion = "v" + recipeImgUploadResult.Version;
                recipeForEdit.ImageURL = "http://res.cloudinary.com" + recipeImgUploadResult.Uri.AbsolutePath;
                //newRecipe.ImageURL = recipeVM.Title.Replace(" ", "-") + ".jpg";
            }
            else
            {
                RenameResult renameResult = null;
                if (newRecipeChildrenCategory!=null)
                {
                    renameResult = RenameRecipeImage(recipeVM.ImageURL, newRecipeParentCategory.Name, newRecipeChildrenCategory.Name);
                }
                else
                {
                    renameResult = RenameRecipeImage(recipeVM.ImageURL, newRecipeParentCategory.Name);
                }
                if (renameResult!=null)
                {
                    recipeForEdit.ImageURL = HttpUtility.UrlDecode(renameResult.Url);
                }
            }
            recipeForEdit.PreparationTime = recipeVM.SelectedPrepTimeHours * 60 + recipeVM.SelectedPrepTimeMinutes;
            recipeForEdit.CookingTime = recipeVM.SelectedCookTimeHours * 60 + recipeVM.SelectedCookTimeMinutes;
            recipeForEdit.Serves = recipeVM.SelectedServes;
            recipeForEdit.Rating = recipeVM.SelectedRating;
            recipeForEdit.Source = this.db.Sources.GetById(recipeVM.SelectedSource);
            if (recipeVM.SelectedRecommendation == "Да")
            {
                recipeForEdit.Recommended = (bool?)true;
            }
            //if (recipeVM.SelectedRecommendation == "Не")
            else
            {
                recipeForEdit.Recommended = (bool?)false;
            }
            recipeForEdit.Date = recipeVM.PublishedDate;
            recipeForEdit.Description = converter.ConvertHTMLToRTF(recipeVM.Description);
            //recipeForEdit.RecipeItems = recipeVM.RecipeItems;
            foreach (var recipeItem in recipeVM.RecipeItems)
            {
                RecipeItem newRecipeItem = new RecipeItem();
                newRecipeItem.RecipeItemName = recipeItem.RecipeItemName;
                foreach (var recipeItem_ingredient in recipeItem.RecipeItems_Ingredients)
                {
                    RecipeItems_Ingredients newRecipeItem_ingredient = new RecipeItems_Ingredients();
                    Unit newUnit = this.db.Units.GetById(recipeItem_ingredient.Unit.UnitID);
                    Ingredient newIngredient = this.db.Ingredients.GetById(recipeItem_ingredient.Ingredient.IngredientID);
                    newRecipeItem_ingredient.Quantity = recipeItem_ingredient.Quantity;
                    newRecipeItem_ingredient.Unit = newUnit;
                    newRecipeItem_ingredient.Ingredient = newIngredient;
                    newRecipeItem.RecipeItems_Ingredients.Add(newRecipeItem_ingredient);
                }
                foreach (var step in recipeItem.Steps)
                {
                    newRecipeItem.Steps.Add(step);
                }

                recipeForEdit.RecipeItems.Add(newRecipeItem);
            }

            //newRecipe.RecipeItems = recipeVM.RecipeItems;

            return recipeForEdit;
        }

        static string cloudinaryAccount = ConfigurationManager.AppSettings["CLOUDINARY_URL"];
        static string cloudinaryStoreURL = ConfigurationManager.AppSettings["RecipesStoreURL"];
        static string cloudinaryRecipesFolder = ConfigurationManager.AppSettings["RecipesFolderName"];

        //private static string UploadRecipeImage(HttpPostedFileBase imageUploader, string categoryName, string recipeName)
        private static ImageUploadResult UploadRecipeImage(HttpPostedFileBase imageUploader, string recipeTitle, string parentCategoryName, string childCategoryName)
        {
            Cloudinary cloudinary = new Cloudinary(cloudinaryAccount);
            string folderName = cloudinaryRecipesFolder;
            if (childCategoryName != null)
            {
                folderName = folderName + "/" + parentCategoryName + "/" + childCategoryName;
            }
            else
            {
                folderName = folderName + "/" + parentCategoryName;
            }
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imageUploader.FileName, imageUploader.InputStream),

                //Folder = folderName,
                PublicId = folderName+"/"+recipeTitle.Replace(" ", "-"),
                Invalidate = true
            };
            var uploadResult = cloudinary.Upload(uploadParams);
            return uploadResult;
        }

        private static void DeleteRecipeImage(string recipeImageURL)
        {
            Cloudinary cloudinaryDelete = new Cloudinary(cloudinaryAccount);
            string recipeImageURLWithoutExtension = recipeImageURL.Substring(0, recipeImageURL.LastIndexOf('.'));
            string recipeImagePublicID = recipeImageURLWithoutExtension.Substring(recipeImageURLWithoutExtension.IndexOf("Recipes"));
            var delParams = new DelResParams()
            {
                PublicIds = new List<string>() { recipeImagePublicID },
                Invalidate = true
            };
            DelResResult result = cloudinaryDelete.DeleteResources(delParams);
        }

        private static RenameResult RenameRecipeImage(string recipeImageURL, string parentCategory, string childCategory=null)
        {
            Cloudinary cloudinaryRename = new Cloudinary(cloudinaryAccount);

            recipeImageURL = recipeImageURL.Substring(0, recipeImageURL.LastIndexOf('.'));
            string imageName = Path.GetFileNameWithoutExtension(recipeImageURL);
            string oldName = recipeImageURL.Substring(recipeImageURL.IndexOf("Recipes"));
            oldName = HttpUtility.UrlDecode(oldName);

            string newName = "";
            if (childCategory != null)
            {
                newName = String.Format(@"Recipes/{0}/{1}/{2}", parentCategory, childCategory, imageName);
            }
            else
            {
                newName = String.Format(@"Recipes/{0}/{1}", parentCategory, imageName);
            }
            if (!oldName.Equals(newName))
            {
                RenameResult renameResult = cloudinaryRename.Rename(oldName, newName, true);
                return renameResult;
            }

            return null;
        }
    }
}