namespace SavalAPI.Models
{
    public class FormularioPregunta
    {
        public int IdFormularioPregunta { get; set; }
        public int IdFormulario { get; set; }
        public Formulario Formulario { get; set; }

        public int IdPregunta { get; set; }
        public Pregunta Pregunta { get; set; }
    }
}
