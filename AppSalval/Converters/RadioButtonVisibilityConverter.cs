using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace AppSalval.Converters
{
    public class RadioButtonVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is string tipoPregunta && tipoPregunta == "Seleccion única";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
