using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Olive.Desktop.WPF.Helpers
{
    public class SelectedRecipeChangedArgs : EventArgs
    {
        public long RecipeID { get; set; }

        public SelectedRecipeChangedArgs(long recipeID)
        {
            this.RecipeID = recipeID;
        }
    }
}
