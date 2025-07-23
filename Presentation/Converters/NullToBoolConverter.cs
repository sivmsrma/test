using System;
using System.Globalization;
using System.Windows.Data;

namespace Terret_Billing.Presentation.Converters
{
   
    public class NullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // This is a one-way converter, so ConvertBack is not implemented
            throw new NotImplementedException();
        }
    }
}
