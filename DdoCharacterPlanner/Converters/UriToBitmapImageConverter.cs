using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;


namespace DdoCharacterPlanner.Converters {

  public class UriToBitmapImageConverter : IValueConverter {

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is Uri uri) {
        BitmapImage bi = new BitmapImage();

        bi.BeginInit();

        bi.UriSource = uri;

        bi.CacheOption = BitmapCacheOption.OnLoad;

        bi.EndInit();

        return bi;
      }

      return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException("Two way conversion is not supported.");
    }

  }

}
