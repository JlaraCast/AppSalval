using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace AppSalval.Converters
{
    public class CheckBoxVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string tipoPregunta)
            {
                return tipoPregunta == "Selección Múltiple"; // ✅ Solo muestra CheckBox si es selección múltiple
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
