using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SavalAPI.Models
{
    public class Respuesta
    {
        [Key]
        public int IdRespuesta { get; set; }

        // Relación con Formulario
        [Required]
        public int IdFormulario { get; set; }
        public Formulario Formulario { get; set; }

        // Relación con Encuestado (opcional, puede ser NULL)
        public string? IdentificacionEncuestado { get; set; } // Clave foránea opcional
        [ForeignKey("IdentificacionEncuestado")]
        public Encuestado? Encuestado { get; set; } // Relación con encuestado (opcional)

        [Required]
        public DateTime FechaRespuesta { get; set; } = DateTime.Now;

        public ICollection<DetalleRespuesta> Detalles { get; set; }
    }
}
