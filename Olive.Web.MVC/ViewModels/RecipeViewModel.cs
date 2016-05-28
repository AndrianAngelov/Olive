namespace Olive.Web.MVC.ViewModels
{
    using System;
    using System.Linq;
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using Olive.Data;
    using Microsoft.Win32;
    using System.IO;
    using System.Reflection;
    using System.Collections.Generic;

    public enum RecipeMode
    {
        View,
        Edit,
        Insert,
        Update
    }

    public class RecipeViewModel : ViewModelBase, IPageViewModel
    {
        public string Name
        {
            get { return "Recipe ViewModel"; }
        }

        public int RecipeID { get; set; }
        public string Title { get; set; }
        public string ImageURL { get; set; }
        public string ImageVersion { get; set; }
        public int Rating { get; set; }
        public int PreparationTime { get; set; }
        public int CookingTime { get; set; }
        public int Serves { get; set; }
        public string SourceName { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int NumberOfHits { get; set; }
        public int NumberOfLikes { get; set; }

        public string Category { get; set; }
        public string ParentCategory { get; set; }
        
    }
}
