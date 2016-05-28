using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Olive.Desktop.WPF.Converters
{
    class RecommendationCoverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isRecommendated;
            if (value==null)
            {
                isRecommendated = false;
            }
            else
            {
                isRecommendated = (bool)value;
            }
            

            return isRecommendated ? "Да":"Не";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string isRecommendated;
            if (value==null)
            {
                return null;
            }
            else
            {
                isRecommendated = (string)value;
            }
            if (isRecommendated.Equals("Да"))
            {
                return true;
            }
            return false;
        }
    }
}
