using System;
using System.Globalization;
using System.Windows.Data;

namespace Terret_Billing.Presentation.Converters
{
    public class EyeIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isVisible)
            {
                // Segoe MDL2 Assets font icons
                return isVisible ? "\uE8F5" : "\uE7B3"; // Open eye vs Closed eye
            }
            return "\uE7B3"; // Default to closed eye
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
