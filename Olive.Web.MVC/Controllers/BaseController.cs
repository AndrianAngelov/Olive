namespace Olive.Web.MVC.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Olive.Data.Uow;
    using System.Linq.Expressions;
    using Olive.Data;
    using Olive.Web.MVC.ViewModels;

    public class BaseController : Controller
    {
        private IUowData db;

        public BaseController()
            : this(new UowData())
        {
        }

        public BaseController(IUowData db)
        {
            this.Db = db;
        }

        public IUowData Db
        {
            get { return db; }
            set { db = value; }
        }

        protected static readonly Expression<Func<Recipe, RecipeViewModel>> AsRecipeViewModel =
        entity => new RecipeViewModel
        {
            RecipeID = entity.RecipeID,
            Title = entity.Title,
            ImageURL = entity.ImageURL,
            PreparationTime = (int)entity.PreparationTime,
            CookingTime = (int)entity.CookingTime,
            Serves = (int)entity.Serves,
            Date = (DateTime)entity.Date ,
            Rating = (int)entity.Rating,
            NumberOfHits = (int)entity.NumberOfHits,
            NumberOfLikes = (int)entity.NumberOfLikes,
            Category = entity.Categories.FirstOrDefault().Name,
            ParentCategory = entity.Categories.Select(x => x.Category1.Name).FirstOrDefault(),
            SourceName = entity.Source.Name
        };
    }
}
