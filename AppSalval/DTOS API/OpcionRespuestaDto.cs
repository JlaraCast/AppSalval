public class OpcionRespuestaDto
{
    public int IdOpcion { get; set; }
    public string NombreOpcion { get; set; }
    public int IdPregunta { get; set; }
    public bool IsSelected { get; set; }

    // 🔹 Nueva propiedad para que cada opción conozca el tipo de pregunta
    public string TipoPregunta { get; set; }

    // Constructor original
    public OpcionRespuestaDto(int idOpcion, string nombreOpcion, int idPregunta)
    {
        IdOpcion = idOpcion;
        NombreOpcion = nombreOpcion;
        IdPregunta = idPregunta;
        IsSelected = false;
    }

    // Constructor con TipoPregunta
    public OpcionRespuestaDto(int idOpcion, string nombreOpcion, int idPregunta, string tipoPregunta)
    {
        IdOpcion = idOpcion;
        NombreOpcion = nombreOpcion;
        IdPregunta = idPregunta;
        TipoPregunta = tipoPregunta;
        IsSelected = false;
    }

    public OpcionRespuestaDto() { }
}
