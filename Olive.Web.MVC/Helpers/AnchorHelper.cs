using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace Olive.Web.MVC.Helpers
{
    public static class AnchorHelper
    {
        public static MvcHtmlString AnchorHtmlString(string className, object innerText)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("&#60;a ");
            sb.Append("class=");
            sb.Append(className);
            sb.Append("");
            sb.Append(" &#62;");
            sb.Append(innerText);
            sb.Append("&#60;/a&#62;");
            return new MvcHtmlString(sb.ToString());
        }
    }
}