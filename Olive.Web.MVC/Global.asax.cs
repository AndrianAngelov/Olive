using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Olive.Web.MVC.ActionFilters;

namespace Olive.Web.MVC
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new HitCounter());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Recipes/Login  
            routes.MapRoute(
                "Login", // Route name
                "Login",
                new { controller = "Account", action = "LogOn" } // Parameter defaults
             );

            routes.MapRoute(
               "RecipesByCategory1", // Route name
               "Recipes/Category/{categoryID}/{categoryName}/{id}", // URL with parameters
               new { controller = "Recipes", action = "RecipesByCategory", categoryID = UrlParameter.Optional, id = UrlParameter.Optional, categoryName = UrlParameter.Optional } // Parameter defaults
           );
            //Recipes/RecipesByCategory/2
            routes.MapRoute(
                "RecipesByCategoryPage", // Route name
                "Recipes/RecipesByCategory/{categoryID}/{isPagingBtn}/{id}", // URL with parameters
                new { controller = "Recipes", action = "RecipesByCategory", categoryID = UrlParameter.Optional, categoryName = UrlParameter.Optional, id = UrlParameter.Optional, isPagingBtn = UrlParameter.Optional } // Parameter defaults
            );

            // Recipes/List
            routes.MapRoute(
                "RecipesList", // Route name
                "Recipes/List/{id}/{isPagingBtn}", // URL with parameters
                new { controller = "Recipes", action = "List", id = UrlParameter.Optional, isPagingBtn = UrlParameter.Optional } // Parameter defaults
            );

            // Recipes/List
            routes.MapRoute(
                "RecipesSearch", // Route name
                "Recipes/Search/{searchString}/{id}", // URL with parameters
                new { controller = "Recipes", action = "Search", searchString = UrlParameter.Optional, id = UrlParameter.Optional } // Parameter defaults
            );

            // Recipes/Details/1
            routes.MapRoute(
                "RecipeHitCounter", // Route name
                "Recipes/RecipeHitCounter/{recipeID}", // URL with parameters
                new { controller = "Recipes", action = "RecipeHitCounter", recipeID = UrlParameter.Optional} // Parameter defaults
            );
            // Recipes/Details/1
            routes.MapRoute(
                "RecipeDetails", // Route name
                "Recipes/{recipeID}/{title}", // URL with parameters
                new { controller = "Recipes", action = "Details", recipeID = UrlParameter.Optional, title = UrlParameter.Optional } // Parameter defaults
            );

            // Home/Index/1
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },// Parameter defaults
                new[] { "Olive.Web.MVC.Controllers" }
           );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}