using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace AppSalval.Converters
{
    public class RadioButtonVisibilityConverter : IValueConverter
    {
        // Método Convert: Convierte el valor de entrada en un valor booleano para controlar la visibilidad del RadioButton
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Verifica si el valor de entrada es una cadena
            if (value is string tipoPregunta)
            {
                // Retorna true si la cadena es "Selección Única", lo que hace visible el RadioButton
                return tipoPregunta == "Selección Única"; // ✅ Solo muestra RadioButton si es selección única
            }
            // Retorna false si el valor de entrada no es una cadena o no es "Selección Única"
            return false;
        }

        // Método ConvertBack: No implementado, lanza una excepción si se intenta usar
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
