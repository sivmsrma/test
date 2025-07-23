using System;
using System.Globalization;
using System.Windows.Data;

namespace Terret_Billing.Presentation.Converters
{
    public class DecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;
            
            if (value is decimal decimalValue)
            {
                return decimalValue.ToString("N3");
            }
            
            if (value is double doubleValue)
            {
                return doubleValue.ToString("N3");
            }
            
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return 0m;
            
            if (string.IsNullOrWhiteSpace(value.ToString()))
                return 0m;
                
            if (decimal.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out decimal result))
            {
                return result;
            }
            
            return 0m;
        }
    }
}
