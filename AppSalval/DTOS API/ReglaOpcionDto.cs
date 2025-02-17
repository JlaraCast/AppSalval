using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AppSalval.DTOS_API
{
    public class ReglaOpcionDto
    {
        public int IdRegla { get; set; }
        public int IdOpcion { get; set; }
         public int? IdRecomendacion { get; set; }

        public int? IdFactorRiesgo { get; set; }

        public string Condicion { get; set; }
    }
}
