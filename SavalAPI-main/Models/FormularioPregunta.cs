using System.Text.Json.Serialization;

namespace SavalAPI.Models
{
    public class FormularioPregunta
    {
        public int IdFormularioPregunta { get; set; }
        public int IdFormulario { get; set; }
        [JsonIgnore]
        public Formulario? Formulario { get; set; }

        public int IdPregunta { get; set; }
        [JsonIgnore]
        public Pregunta? Pregunta { get; set; }
    }
}
