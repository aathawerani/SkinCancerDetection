using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Data;
using System.Windows.Controls;

namespace DiagramDesigner.Controls
{
    public class ImageConvertor:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((value != null) & ((value as string) != ""))
            {
                string imageName = String.Format(@"pack://application:,,/{0}", value as string);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(imageName);
                image.EndInit();
                return image;
            }
            return null;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
