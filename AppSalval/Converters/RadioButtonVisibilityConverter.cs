using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace AppSalval.Converters
{
    public class RadioButtonVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string tipoPregunta)
            {
                return tipoPregunta == "Selección Única"; // ✅ Solo muestra RadioButton si es selección única
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
