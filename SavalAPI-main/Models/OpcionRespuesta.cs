using System.Text.Json.Serialization;

namespace SavalAPI.Models
{
    public class OpcionRespuesta
    {
        public int IdOpcion { get; set; }
        public string NombreOpcion { get; set; }

        public int IdPregunta { get; set; }
        [JsonIgnore]
        public Pregunta? Pregunta { get; set; }
    }
}
