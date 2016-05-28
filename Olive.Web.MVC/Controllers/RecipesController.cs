using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using Olive.Web.MVC.ViewModels.Recipes;
using System.Diagnostics;
using System.Net;
using Olive.Web.MVC.ActionFilters;
using Olive.Data;
using Olive.Web.MVC.ViewModels;
using System.Linq.Expressions;
using Olive.Data.Uow;

namespace Olive.Web.MVC.Controllers
{
    public class RecipesController : BaseController
    {

        //
        // GET: /Recipes/List/2
        public ActionResult List(int? id, bool? isPagingBtn)
        {
            int pageSize = 8;
            int pageNumber = id.GetValueOrDefault(1);

            var viewModel = GetAllRecipes().Skip((pageNumber - 1) * pageSize).Take(pageSize);
            double numberOfAllPages = Math.Ceiling((double)GetAllRecipes().Count() / pageSize);

            RecipesListViewModel recipesListVM = new RecipesListViewModel()
            {
                ListTitle="Olive-Всички рецепти",
                Recipes = viewModel,
                PageSize = pageSize,
                RecipesListNumberOfPages = (int)numberOfAllPages,
                PagingBtnsClass="allRecipesList"
            };
            if (Request.IsAjaxRequest())
            {
                Debug.WriteLine(isPagingBtn);
                if (isPagingBtn == null)
                {
                    return PartialView("_RecipesListPartial", recipesListVM);
                }
                else
                {
                    return PartialView("_RecipesListViewPartial", recipesListVM.Recipes);
                }

            }
            return View(recipesListVM);
        }

        //
        // GET: /Recipes/RecipesByCategory/1/Десерти
        public ActionResult RecipesByCategory(int? categoryID, string categoryName, int? id,bool? isPagingBtn)
        {
            int pageSize = 8;
            int pageNumber = id.GetValueOrDefault(1);

            var data = this.Db.Recipes.AllRecipesByCategoryID((int)categoryID).OrderByDescending(x => x.Date).Select(AsRecipeViewModel);
            var viewModel = data.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            double numberOfAllPages = Math.Ceiling((double)data.Count() / pageSize);

            RecipesListViewModel recipesListVM = new RecipesListViewModel()
            {
                ListTitle = "Olive-" + categoryName,
                Recipes = viewModel,
                PageSize = pageSize,
                RecipesListNumberOfPages = (int)numberOfAllPages,
                PagingBtnsClass = "recipesByCategory",
                CurrentCategoryID = (int)categoryID
            };
            if (Request.IsAjaxRequest())
            {
                Debug.WriteLine(isPagingBtn);
                if (isPagingBtn==null)
                {
                    return PartialView("_RecipesListPartial", recipesListVM);
                }
                else
                {
                    return PartialView("_RecipesListViewPartial", recipesListVM.Recipes);
                }
                
            }
            return View("List", recipesListVM);
        }

        public ActionResult Search(string searchString, int? id)
        {
            int pageSize = 8;
            int pageNumber = id.GetValueOrDefault(1);
            
            var viewModel = GetAllSearchRecipes(searchString).Skip((pageNumber - 1) * pageSize).Take(pageSize);
            double numberOfAllPages = Math.Ceiling((double)GetAllSearchRecipes(searchString).Count() / pageSize);

            RecipesListViewModel recipesListVM = new RecipesListViewModel()
            {
                ListTitle = "Olive-Резултати от търсене за: " + searchString,
                Recipes = viewModel,
                PageSize = pageSize,
                RecipesListNumberOfPages = (int)numberOfAllPages,
                PagingBtnsClass = "searchRecipesList"
            };
            
            if (Request.IsAjaxRequest())
            {
                return PartialView("_RecipesListViewPartial", recipesListVM.Recipes);
            }
            else
            {
                if (String.IsNullOrEmpty(searchString))
                {
                    return RedirectToAction("List");
                }
                return View(recipesListVM);
               
            }
           
        }

        /*[HitCounter]*/
        public ActionResult Details(int? recipeID)
        {
            if (recipeID != null)
            {
                var recipe = this.Db.Recipes.GetById((int)recipeID);
                ViewBag.NubersOfrecupeItems = recipe.RecipeItems.Count();
                return View(recipe);
            }

            return RedirectToAction("All", "Recipes");
        }

        public ActionResult RecipeHitCounter(int recipeID)
        {
            IUowData db = new UowData();

            var oldRecipe = from rec in db.Recipes.All()
                            where rec.RecipeID == recipeID
                            select rec;

            Recipe newRecipe = oldRecipe.FirstOrDefault();
            newRecipe.NumberOfHits = newRecipe.NumberOfHits + 1;
            db.Recipes.Update(newRecipe);
            int result = db.SaveChanges();

            return RedirectToAction("Details", recipeID);
        }

        public ActionResult RecipeLikeCounter(int recipeID)
        {
            return Content(recipeID.ToString());
        }

        [ChildActionOnly]
        public virtual ActionResult CategoriesList()
        {
            var categoriesList = this.Db.Categories.AllParentCategories().ToList();
            return PartialView("_CategoriesPartial", categoriesList);
        }

        private IQueryable<RecipeViewModel> GetAllRecipes()
        {
            var data = this.Db.Recipes.All().OrderBy(x => x.RecipeID).Select(AsRecipeViewModel);

            return data;
        }

        private IQueryable<RecipeViewModel> GetAllSearchRecipes(string searchString)
        {
            IQueryable<RecipeViewModel> data;
            if (!String.IsNullOrEmpty(searchString))
            {
                data = this.Db.Recipes.All().Where(r => r.Title.ToUpper().Contains(searchString.ToUpper())
                                               || r.Categories.Any(c => c.Name.ToUpper().Contains(searchString.ToUpper())
                                                                   || c.Category1.Name.ToUpper().Contains(searchString.ToUpper()))
                                                || r.RecipeItems.Any(ri=>ri.RecipeItems_Ingredients.Any(riIn=>riIn.Ingredient.Name.Contains(searchString.ToUpper()))))
                                               .OrderBy(x => x.RecipeID).Select(AsRecipeViewModel);
            }
            else
            {
                data = this.Db.Recipes.All().OrderBy(x => x.RecipeID).Select(AsRecipeViewModel);
            }

            return data;
        }
    }
}
