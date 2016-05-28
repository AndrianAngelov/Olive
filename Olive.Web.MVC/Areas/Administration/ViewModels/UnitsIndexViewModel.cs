using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Olive.Data;

namespace Olive.Web.MVC.Areas.Administration.ViewModels
{
    public class UnitsIndexViewModel
    {
        public IEnumerable<Unit> AllUnits{ get; set; }
        public Unit NewUnit { get; set; }
    }
}