using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Terret_Billing.Presentation.Converters
{
    public class FirmTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var firmType = value as string;
            if (string.Equals(firmType, "MY FIRM", StringComparison.OrdinalIgnoreCase))
                return new SolidColorBrush(Color.FromRgb(243, 156, 18)); // Orange
            if (string.Equals(firmType, "SELF", StringComparison.OrdinalIgnoreCase))
                return new SolidColorBrush(Color.FromRgb(39, 174, 96)); // Green
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
