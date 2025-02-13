using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization; // Importante para usar [JsonIgnore]

namespace SavalAPI.Models
{
    public class Pregunta
    {
        [Key]
        public int IdPregunta { get; set; }

        [Required]
        [MaxLength(50)]
        public string TipoPregunta { get; set; }

        [Required]
        [MaxLength(500)]
        public string TextoPregunta { get; set; }

        [JsonIgnore] // Excluir siempre esta propiedad en la respuesta JSON
        public ICollection<FormularioPregunta>? Formularios { get; set; }

        [JsonIgnore] // Excluir siempre esta propiedad en la respuesta JSON
        public ICollection<OpcionRespuesta>? Opciones { get; set; }
    }
}
