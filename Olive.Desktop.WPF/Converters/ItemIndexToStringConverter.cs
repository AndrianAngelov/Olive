using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Collections.ObjectModel;
using Olive.Data;

namespace Olive.Desktop.WPF.Converters
{
    public class ItemIndexToStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string itemIndex = "";

            if (values[0] is RecipeItem)
            {
                ObservableCollection<RecipeItem> list = new ObservableCollection<RecipeItem>((IEnumerable<RecipeItem>)values[1]);
                itemIndex = "Секция " + (list.IndexOf((RecipeItem)values[0]) + 1).ToString();
            }
           
            if (values[0] is RecipeItems_Ingredients)
            {
                
                ObservableCollection<RecipeItems_Ingredients> list = new ObservableCollection<RecipeItems_Ingredients>((IEnumerable<RecipeItems_Ingredients>)values[1]);
                //list.OrderBy(x=>x.RecipeItemID);
                //itemIndex="Съставка_" + (list.IndexOf((RecipeItems_Ingredients)values[0])+1).ToString() +" :" + " ";
                //var view = CollectionViewSource.GetDefaultView(list);
                
                //int index = list.IndexOf((RecipeItems_Ingredients)values[0]);
                int index = list.IndexOf((RecipeItems_Ingredients)values[0]);
                itemIndex = "Съставка_" + index.ToString() + " :" + " ";
            }

            if (values[0] is Step)
            {
                ObservableCollection<Step> list = new ObservableCollection<Step>((IEnumerable<Step>)values[1]);
                //itemIndex = "Стъпка_" + (list.IndexOf((Step)values[0]) + 1).ToString() + " :" + " ";
                itemIndex = (list.IndexOf((Step)values[0]) + 1).ToString();
            }
            
            return itemIndex;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
