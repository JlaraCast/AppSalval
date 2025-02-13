using System.Text.Json.Serialization;

namespace SavalAPI.Models
{
    public class Formulario
    {
        public int IdFormulario { get; set; }
        public string TituloFormulario { get; set; }
        public string? DescripcionFormulario { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Habilitado { get; set; }
        public bool Anonimo { get; set; }
        [JsonIgnore]
        public ICollection<FormularioPregunta>? Preguntas { get; set; }
    }
}
