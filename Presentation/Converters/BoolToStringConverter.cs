using System;
using System.Globalization;
using System.Windows.Data;

namespace Terret_Billing.Presentation.Converters
{
    /// <summary>
    /// Converts a boolean value to a string
    /// </summary>
    public class BoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                if (parameter is string paramString)
                {
                    var parts = paramString.Split('|');
                    if (parts.Length == 2)
                    {
                        return boolValue ? parts[0] : parts[1];
                    }
                }
                return boolValue ? "True" : "False";
            }
            return "False";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                if (parameter is string paramString)
                {
                    var parts = paramString.Split('|');
                    if (parts.Length == 2)
                    {
                        return stringValue == parts[0];
                    }
                }
                return stringValue == "True";
            }
            return false;
        }
    }
}
