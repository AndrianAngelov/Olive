using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Olive.Data;

namespace Olive.Desktop.WPF.Converters
{
    public class ParentCategoryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Category category ;

            try
            {
                category = (value as Category);
                category = category.Category1;
            }
            catch (Exception)
            {
                category = null;
            }

            return category;

            //Old
            //Category category = (value as Category);

            //if (category.Category1!=null)
            //{
            //    category = category.Category1;
            //}

            //return category;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Category category = (value as Category);
            return category;
        }
    }
}
