namespace Olive.Desktop.WPF.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Data;

    public class HoursToMinutesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object hours;
            try
            {
                hours=(int)value/60;
            }
            catch (Exception)
            {
                hours=null;
            }
            return hours;
            //Old
            //int hours = (int)value/60;
            //return hours;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value==null)
            {
                return value;
            }
            int hours = (int)value;
            int minutes = hours * 60;

            if (minutes==0)
            {
                minutes = -1;
            }
            return minutes;
        }
    }
}
