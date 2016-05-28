using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Olive.Data;
using System.Diagnostics;
using Olive.Web.MVC.ViewModels;
using Olive.Web.MVC.Models;

namespace Olive.Web.MVC.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            HomeViewModel homeModel = new HomeViewModel();
            homeModel.RecipesByCreationDate = GetAllRecipesByCreationDate();
            homeModel.RecipesByRosiRecommendation = GetAllRecipesByRecipesByRosiRecommendation(1);
            homeModel.RecipesByRating = GetAllRecipesByRating(1);
            homeModel.RecipesByNubmerOfHits = GetAllRecipesByNubmerOfHits(1);
            return View(homeModel);
        }

        public ActionResult RecipesByRosiRecommendation(int? id)
        {
            int pageNumber = id.GetValueOrDefault(1);
            var viewModel = GetAllRecipesByRecipesByRosiRecommendation(pageNumber);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_RecipesListViewPartial", viewModel);
            }
            return View(viewModel);
        }

        public ActionResult RecipesByRating(int? id)
        {
            int pageNumber = id.GetValueOrDefault(1);
            var viewModel = GetAllRecipesByRating(pageNumber);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_RecipesListViewPartial", viewModel);
            }
            return View(viewModel);
        }

        public ActionResult RecipesByNubmerOfHits(int? id)
        {
            int pageNumber = id.GetValueOrDefault(1);
            var viewModel = GetAllRecipesByNubmerOfHits(pageNumber);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_RecipesListViewPartial", viewModel);
            }
            return View(viewModel);
        }

        private IQueryable<RecipeViewModel> GetAllRecipesByCreationDate()
        {
            var data = this.Db.Recipes.GetTopRecipesByCreationDate(3).Select(AsRecipeViewModel);
            return data;
        }

        private IQueryable<RecipeViewModel> GetAllRecipesByRecipesByRosiRecommendation(int? id)
        {
            int pageSize = 2;
            int pageNumber = id.GetValueOrDefault(1);
            var data = this.Db.Recipes.GetRecipesByRosiRecommendation().Select(AsRecipeViewModel);
            var viewModel = data.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            ViewBag.RosiRecommodationPages = Math.Ceiling((double)data.ToList().Count() / pageSize);

            return viewModel;
        }

        private IQueryable<RecipeViewModel> GetAllRecipesByRating(int? id)
        {
            int pageSize = 4;
            int pageNumber = id.GetValueOrDefault(1);
            var data = this.Db.Recipes.GetTopRecipesByRating(20).Select(AsRecipeViewModel);
            var viewModel = data.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            ViewBag.RatingPages = Math.Ceiling((double)data.Count() / pageSize);

            return viewModel;
        }

        private IQueryable<RecipeViewModel> GetAllRecipesByNubmerOfHits(int? id)
        {
            int pageSize = 4;
            int pageNumber = id.GetValueOrDefault(1);
            var data = this.Db.Recipes.GetTopRecipesByNubmerOfHits(20).Select(AsRecipeViewModel);
            var viewModel = data.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            ViewBag.NubmerOfHitsPages = Math.Ceiling((double)data.Count() / pageSize);

            return viewModel;
        }

        [ChildActionOnly]
        public virtual ActionResult RecipesByDate()
        {
            var categoriesList = this.Db.Categories.AllParentCategories().ToList();
            return PartialView("_AllCategoriesPartial", categoriesList);
        }

        [ChildActionOnly]
        public virtual ActionResult CategoriesList()
        {
            var categoriesList = this.Db.Categories.AllParentCategories().ToList();
            return PartialView("_AllCategoriesPartial", categoriesList);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
