namespace AppSalval.Models_Api
{
    public class FormularioPreguntaDto
    {
        public int IdFormularioPregunta { get; set; } // ID único de la relación formulario-pregunta
        public int IdFormulario { get; set; } // ID del formulario al que pertenece la pregunta
        public int IdPregunta { get; set; } // ID de la pregunta
        public string TextoPregunta { get; set; } // Contenido de la pregunta
    }
}
