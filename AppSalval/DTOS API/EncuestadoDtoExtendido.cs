using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppSalval.DTOS_API;

namespace AppSalval.DTOS_API
{
    public class EncuestadoExtendidoDTO : EncuestadoDto
    {
        public EncuestadoExtendidoDTO(EncuestadoDto encuestadoDTO)
        {
            Identificacion = encuestadoDTO.Identificacion;
            TipoIdentificacion = encuestadoDTO.TipoIdentificacion;
            NombreCompleto = encuestadoDTO.NombreCompleto;
            FechaNacimiento = encuestadoDTO.FechaNacimiento;
            Sexo = encuestadoDTO.Sexo;
        }

        public int IdRespuesta { get; set; }
        public DateTime FechaRespuesta { get; set; }
        public int IdFormulario { get; set; }
    }
}