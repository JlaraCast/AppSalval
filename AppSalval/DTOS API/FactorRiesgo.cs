
using System.Text.Json.Serialization;


namespace AppSalval.DTOS_API
{
    public class FactorRiesgo
    {
        [JsonPropertyName("idFactor")]
        public int IdFactor { get; set; }

        [JsonPropertyName("textoFactor")]
        public string? TextoFactor { get; set; }

        [JsonPropertyName("habilitado")]
        public bool Habilitado { get; set; }

        [JsonIgnore]
        public string EstadoTexto => Habilitado ? "Activo" : "Inactivo";
    }
}
