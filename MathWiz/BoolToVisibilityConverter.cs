using System;
using System.Windows.Data;
using System.Windows;

namespace MathWiz
{
    class BoolToVisibilityConverter : IValueConverter  
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool IsWrong = (bool)value;

            if (IsWrong)
                return Visibility.Hidden;
            else
                return Visibility.Visible;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
