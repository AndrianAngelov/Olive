using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Olive.Desktop.WPF.ViewModels;

namespace Olive.Desktop.WPF.Helpers
{
    public class RecipesByCategoryChangedArgs : EventArgs
    {
        public long CategoryID { get; set; }

        public RecipesByCategoryChangedArgs(long categoryID)
            : base()
        {
            this.CategoryID = categoryID;
        }
    }
}
