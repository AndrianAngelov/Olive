using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Olive.Web.MVC.Areas.Administration.Controllers
{
    public class HomeController : BaseAdminController
    {
        //
        // GET: /Administration/Home/
        public ActionResult Index()
        {
            return View();
        }

       
    }
}
