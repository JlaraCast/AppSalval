using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SavalAPI.Models
{
    public class Respuesta
    {
        [Key]
        public int IdRespuesta { get; set; }

        // Relación con Formulario
        [Required]
        public int IdFormulario { get; set; }

        [JsonIgnore]
        public Formulario? Formulario { get; set; }

        // Relación con Encuestado (opcional, puede ser NULL)
        [JsonIgnore]
        public string? IdentificacionEncuestado { get; set; } // Clave foránea opcional
        [ForeignKey("IdentificacionEncuestado")]

        [JsonIgnore]
        public Encuestado? Encuestado { get; set; } // Relación con encuestado (opcional)

        [Required]
        public DateTime FechaRespuesta { get; set; } = DateTime.Now;
        [JsonIgnore]
        public ICollection<DetalleRespuesta>? Detalles { get; set; }
    }
}
