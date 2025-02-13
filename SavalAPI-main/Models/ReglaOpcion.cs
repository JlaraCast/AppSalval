using SavalAPI.Models;
using System.Text.Json.Serialization;

public class ReglaOpcion
{
    public int IdRegla { get; set; }
    public int IdOpcion { get; set; }

    [JsonIgnore] // No incluir en la respuesta
    public OpcionRespuesta? Opcion { get; set; } // Hacerlo opcional con "?"

    public int? IdRecomendacion { get; set; }

    [JsonIgnore] // No incluir en la respuesta
    public Recomendacion? Recomendacion { get; set; } // Hacerlo opcional con "?"

    public int? IdFactorRiesgo { get; set; }

    [JsonIgnore] // No incluir en la respuesta
    public FactorRiesgo? FactorRiesgo { get; set; } // Hacerlo opcional con "?"

    public string Condicion { get; set; }
}
