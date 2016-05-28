using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using Olive.Data.Uow;
using Olive.Data;

namespace Olive.Web.MVC.ActionFilters
{
    public class HitCounter : ActionFilterAttribute
    {
        //public override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //    //var currentController = filterContext.RouteData.Values["controller"].ToString();
        //    //var currentAction = filterContext.RouteData.Values["action"].ToString();

        //    //if (currentController.Equals("Recipes") && currentAction.Equals("Details"))
        //    //{
        //    //    //Debug.WriteLine(filterContext.RouteData.Values["action"].ToString());
        //    //    //Debug.WriteLine(filterContext.RouteData.Values["recipeID"].ToString());
        //    //    //int currentRecipeID=(int)filterContext.RouteData.Values["recipeID"];

        //    //    int currentRecipeID = Convert.ToInt32(filterContext.RouteData.Values["recipeID"].ToString());
        //    //    IUowData db = new UowData();

        //    //    var oldRecipe = from rec in db.Recipes.All()
        //    //                    where rec.RecipeID == currentRecipeID
        //    //                    select rec;

        //    //    Recipe newRecipe = oldRecipe.FirstOrDefault();
        //    //    newRecipe.NumberOfHits = newRecipe.NumberOfHits + 1;
        //    //    db.Recipes.Update(newRecipe);
        //    //    int result = db.SaveChanges();
        //    //}
            
        //}
    }
}