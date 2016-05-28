using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Olive.Desktop.WPF.Converters
{
    public class DateConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTime dt;
            if (value == null)
            {
                dt = DateTime.Now;
            }
            else
            {
                dt = (DateTime)value;
            }
            //DateTime dt = (DateTime)value;

            return dt.ToShortDateString();

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTime dt;
            dt = DateTime.Now;
            //throw new NotImplementedException();
            return dt;

        }

        #endregion

    }
}
