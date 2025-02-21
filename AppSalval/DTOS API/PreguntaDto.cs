using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSalval.DTOS_API
{
    public class PreguntaDto
    {

        public int IdPregunta { get; set; }

        public string TipoPregunta { get; set; }

        public string TextoPregunta { get; set; }
    }
}
