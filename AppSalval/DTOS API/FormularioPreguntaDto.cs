using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AppSalval.DTOS_API
{
    public class FormularioPreguntaDto
    {
        [JsonPropertyName("idPregunta")]
        public int IdPregunta { get; set; }

        [JsonPropertyName("tipoPregunta")]
        public string TipoPregunta { get; set; }

        [JsonPropertyName("textoPregunta")] // 🔹 Corregido para que coincida con el JSON
        public string TextPregunta { get; set; }

        public List<OpcionRespuestaDto> OpcionesRespuesta { get; set; } = new List<OpcionRespuestaDto>();
    }
}
