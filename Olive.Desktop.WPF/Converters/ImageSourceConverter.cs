using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows;

namespace Olive.Desktop.WPF.Converters
{
    public class ImageSourceConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //declarations
            string assemblyPath = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
            string filePath;
            string nullPath;
            int isDebug = assemblyPath.IndexOf("Debug");
            if (isDebug != -1)
            {
                nullPath = assemblyPath.Replace(@"bin\Debug", @"Images\");
            }
            else
            {
                nullPath = assemblyPath.Replace(@"Application", @"RecipesImages\");
            }
            BitmapImage imageToBitmap = new BitmapImage();

            try
            {
                //concatenate the values
                //filePath = Path.Combine(values[0].ToString(), values[1].ToString());
                filePath = values[1].ToString();

                imageToBitmap.BeginInit();
                imageToBitmap.CacheOption = BitmapCacheOption.OnLoad;
                imageToBitmap.UriSource = new Uri(filePath);
                imageToBitmap.EndInit();
            }
            //FileNotFoundException
            catch (UriFormatException)
            {
                //concatenate the values
                filePath = Path.Combine(nullPath, "null_image.png");

                imageToBitmap.CacheOption = BitmapCacheOption.OnLoad;
                imageToBitmap.UriSource = new Uri(filePath);
                imageToBitmap.EndInit();
            }
            return imageToBitmap;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
            //return value == null ? DependencyProperty.UnsetValue : value;
        }
    }
}
