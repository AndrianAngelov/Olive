using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Olive.Desktop.WPF.Converters
{
    public class MinutesFromHoursConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object minutes;
            try
            {
                minutes = (int)value;

                if ((int)minutes > 55)
                {
                    minutes = (int)value % 60;
                }
            }
            catch (Exception)
            {
                minutes=null;
            }

            return minutes;

            //Old
            //int minutes = (int)value;

            //if (minutes > 55)
            //{
            //    minutes = (int)value % 60;
            //}

            //return minutes;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return value;
            }
            return (int)value;
        }
    }
}
