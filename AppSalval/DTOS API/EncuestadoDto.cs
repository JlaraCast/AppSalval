using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AppSalval.DTOS_API
{
    public class EncuestadoDto
    {   public string Identificacion { get; set; } // PK

        public string TipoIdentificacion { get; set; } // Cédula, Pasaporte

        public string NombreCompleto { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public string Sexo { get; set; } // Masculino, Femenino, Otro

        public bool Habilitado { get; set; } = true; // Activo por defecto

      }

}

