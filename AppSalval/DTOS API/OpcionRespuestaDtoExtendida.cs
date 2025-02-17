using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppSalval.DTOS_API;
namespace AppSalval.DTOS_API
{
    public class OpcionRespuestaDtoExtendida : OpcionRespuestaDto
    {
        public OpcionRespuestaDtoExtendida(OpcionRespuestaDto opcionRespuestaDto)
        {
            IdOpcion = opcionRespuestaDto.IdOpcion;
            NombreOpcion = opcionRespuestaDto.NombreOpcion;
            IdPregunta = opcionRespuestaDto.IdPregunta;
        }

        public string SeleccionadaRecomendacion { get; set; }
        public string SeleccionadaRiesgo { get; set; }
        public double Condicion { get; set; }
    }
}
