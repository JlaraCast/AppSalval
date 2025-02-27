using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace AppSalval.Converters
{
    public class CheckBoxVisibilityConverter : IValueConverter
    {
        // Método Convert: Convierte el valor de entrada en un valor booleano que indica si se debe mostrar el CheckBox
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Verifica si el valor de entrada es una cadena
            if (value is string tipoPregunta)
            {
                // Retorna true si el tipo de pregunta es "Selección Múltiple", de lo contrario retorna false
                return tipoPregunta == "Selección Múltiple"; // ✅ Solo muestra CheckBox si es selección múltiple
            }
            // Si el valor de entrada no es una cadena, retorna false
            return false;
        }

        // Método ConvertBack: No implementado, lanza una excepción si se llama
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
