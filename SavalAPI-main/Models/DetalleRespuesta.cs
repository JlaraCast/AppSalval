using System.Text.Json.Serialization;

namespace SavalAPI.Models
{
    public class DetalleRespuesta
    {
        public int IdDetalleRespuesta { get; set; }

        public int IdRespuesta { get; set; }

        [JsonIgnore]
        public Respuesta? Respuesta { get; set; }

        public int IdPregunta { get; set; }

        [JsonIgnore]
        public Pregunta? Pregunta { get; set; }

        public int IdOpcion { get; set; }

        [JsonIgnore]
        public OpcionRespuesta? Opcion { get; set; }
    }
}
