using System.Windows;
using System.Windows.Input;

namespace Terret_Billing.Presentation.Helpers
{
    /// <summary>
    /// Helper class to enable Enter key traversal between form fields
    /// </summary>
    public static class EnterKeyTraversal
    {
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(EnterKeyTraversal),
                new PropertyMetadata(false, OnIsEnabledPropertyChanged));

        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }

        private static void OnIsEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element)
            {
                if ((bool)e.NewValue)
                {
                    element.KeyDown += Element_KeyDown;
                }
                else
                {
                    element.KeyDown -= Element_KeyDown;
                }
            }
        }

        private static void Element_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Move to the next control
                var element = sender as UIElement;
                if (element != null)
                {
                    element.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    e.Handled = true;
                }
            }
        }
    }
}
