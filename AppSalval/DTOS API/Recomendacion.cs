using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AppSalval.DTOS_API
{
    public class Recomendacion
    {
        [JsonPropertyName("idRecomendacion")]
        public int IdRecomendacion { get; set; }

        [JsonPropertyName("textoRecomendacion")]
        public string? TextoRecomendacion { get; set; }

        [JsonPropertyName("habilitado")]
        public bool Habilitado { get; set; }

        [JsonIgnore]
        public string EstadoTexto => Habilitado ? "Activo" : "Inactivo";
    }
}
