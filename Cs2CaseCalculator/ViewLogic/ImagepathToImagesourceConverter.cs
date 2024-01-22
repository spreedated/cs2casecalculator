using neXn.Lib.Wpf.ViewLogic;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace Cs2CaseCalculator.ViewLogic
{
    public class ImagepathToImagesourceConverter : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string path = (string)value;

            if (path == null || string.IsNullOrEmpty(path))
            {
                return null;
            }

            BitmapImage bi = new();
            bi.BeginInit();
            bi.UriSource = new Uri(Path.Combine(Environment.CurrentDirectory, path.Replace("/", "\\")));
            bi.EndInit();

            return bi;
        }
    }
}
