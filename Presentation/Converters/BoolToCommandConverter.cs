using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace Terret_Billing.Presentation.Converters
{
    /// <summary>
    /// Converts a boolean value to determine which command to execute
    /// </summary>
    public class BoolToCommandConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue && parameter is object[] commands && commands.Length >= 2)
            {
                return boolValue ? commands[0] : commands[1];
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
