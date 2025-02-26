using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppSalval.DTOS_API
{
    public class RespuestasDTO
    {
        public int IdRespuesta { get; set; }
        public int IdFormulario { get; set; }
        public string? IdentificacionEncuestado { get; set; } // Clave foránea opcional
        public DateTime FechaRespuesta { get; set; }
        public List<DetallesRespDTO> DetallesRespuestas { get; set; } = new List<DetallesRespDTO>();

        public RespuestasDTO(){ }

        // Constructor original (para compatibilidad con código previo)
        public RespuestasDTO(int idRespuesta, int idFormulario, string? identificacionEncuestado, DateTime fechaRespuesta)
        {
            IdRespuesta = idRespuesta;
            IdFormulario = idFormulario;
            IdentificacionEncuestado = identificacionEncuestado;
            FechaRespuesta = fechaRespuesta;
        }

        // Constructor extendido con los detalles de respuestas
        public RespuestasDTO(int idRespuesta, int idFormulario, string? identificacionEncuestado, DateTime fechaRespuesta, List<DetallesRespDTO> detallesRespuestas)
        {
            IdRespuesta = idRespuesta;
            IdFormulario = idFormulario;
            IdentificacionEncuestado = identificacionEncuestado;
            FechaRespuesta = fechaRespuesta;
            DetallesRespuestas = detallesRespuestas ?? new List<DetallesRespDTO>();
        }
    }

    public class DetallesRespDTO
    {
        public int IdPregunta { get; set; }
        public int IdOpcion { get; set; }

        // Constructor por defecto
        public DetallesRespDTO() { }

        // Constructor para inicializar datos
        public DetallesRespDTO(int idPregunta, int idOpcion)
        {
            IdPregunta = idPregunta;
            IdOpcion = idOpcion;
        }
    }
}
