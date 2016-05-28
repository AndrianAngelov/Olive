using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Olive.Web.MVC.Areas.Administration.Controllers
{
    [Authorize(Roles="Administrator")]
    public class BaseAdminController : Controller
    {
       
    }
}
