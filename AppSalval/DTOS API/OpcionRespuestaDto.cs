using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AppSalval.DTOS_API
{
    public class OpcionRespuestaDto
    {
        public int IdOpcion { get; set; }
        public string NombreOpcion { get; set; }

        public int IdPregunta { get; set; }
        
    }
}
