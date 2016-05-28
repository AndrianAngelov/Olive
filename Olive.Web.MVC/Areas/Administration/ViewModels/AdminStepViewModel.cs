using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Olive.Web.MVC.Areas.Administration.ViewModels
{
    public class AdminStepViewModel
    {
        public int StepID { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}