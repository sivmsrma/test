using System;
using System.Globalization;
using System.Windows.Data;
using Terret_Billing.Presentation.Models;

namespace Terret_Billing.Presentation.Converters
{
    [ValueConversion(typeof(object[]), typeof(string))]
    public class CustomerNameConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (values == null || values.Length < 2)
                    return string.Empty;

                // If we have a search text, return it to allow searching
                string searchText = values[1] as string;
                if (!string.IsNullOrEmpty(searchText))
                {
                    return searchText;
                }

                // If we have a selected customer but no search text, return the customer name
                if (values[0] is Customer selectedCustomer && selectedCustomer != null)
                {
                    return selectedCustomer.Name ?? string.Empty;
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CustomerNameConverter error: {ex}");
                return string.Empty;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] { null, value };
        }
    }
}
