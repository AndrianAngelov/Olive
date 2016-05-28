using System.Web.Mvc;

namespace Olive.Web.MVC.Areas.Administration
{
    public class AdministrationAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Administration";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               "Administration_Recipes",
               "Administration/Recipes/{action}/{id}",
               new { controller = "AdminRecipes", action = "Index", id = UrlParameter.Optional }
           );
           
            context.MapRoute(
                "Administration_default",
                "Administration/{controller}/{action}/{id}",
                new { controller="Home",action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
