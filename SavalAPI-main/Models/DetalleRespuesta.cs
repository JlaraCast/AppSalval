namespace SavalAPI.Models
{
    public class DetalleRespuesta
    {
        public int IdDetalleRespuesta { get; set; }

        public int IdRespuesta { get; set; }
        public Respuesta Respuesta { get; set; }

        public int IdPregunta { get; set; }
        public Pregunta Pregunta { get; set; }

        public int IdOpcion { get; set; }
        public OpcionRespuesta Opcion { get; set; }
    }
}
