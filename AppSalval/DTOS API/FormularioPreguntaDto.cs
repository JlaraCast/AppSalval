using System.Collections.Generic;

namespace AppSalval.DTOS_API
{
    public class FormularioPreguntaDto
    {
        public int IdPregunta { get; set; }
        public string TipoPregunta { get; set; }
        public string TextoPregunta { get; set; }
        public List<OpcionRespuestaDto> Opciones { get; set; } = new List<OpcionRespuestaDto>();
    }

    public class OpcionRespuestaormularioDto
    {
        public int IdOpcion { get; set; }
        public int IdPregunta { get; set; }
        public string TextoOpcion { get; set; }
    }
}
