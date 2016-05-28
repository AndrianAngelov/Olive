using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Olive.Data;

namespace Olive.Web.MVC.Areas.Administration.ViewModels
{
    public class SourcesIndexViewModel
    {
        public IEnumerable<Source> AllSources { get; set; }
        public Source NewSource { get; set; }
    }
}