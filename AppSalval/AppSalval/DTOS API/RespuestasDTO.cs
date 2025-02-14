using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AppSalval.DTOS_API
{
    public class RespuestasDTO
    {
        public int IdRespuesta { get; set; }

        public int IdFormulario { get; set; }

        public string? IdentificacionEncuestado { get; set; } // Clave foránea opcional
        
        public DateTime FechaRespuesta { get; set; }
    }
}
