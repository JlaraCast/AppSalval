public class OpcionRespuestaDto
{
    public int IdOpcion { get; set; }
    public string NombreOpcion { get; set; }
    public int IdPregunta { get; set; }
    public bool IsSelected { get; set; } // Nuevo atributo para manejar selección múltiple

    // 🟢 Constructor original (sin IsSelected) para compatibilidad con código existente
    public OpcionRespuestaDto(int idOpcion, string nombreOpcion, int idPregunta)
    {
        IdOpcion = idOpcion;
        NombreOpcion = nombreOpcion;
        IdPregunta = idPregunta;
        IsSelected = false; // Valor por defecto en caso de que no se inicialice
    }

    // 🟢 Nuevo constructor que incluye IsSelected
    public OpcionRespuestaDto(int idOpcion, string nombreOpcion, int idPregunta, bool isSelected)
    {
        IdOpcion = idOpcion;
        NombreOpcion = nombreOpcion;
        IdPregunta = idPregunta;
        IsSelected = isSelected;
    }

    // 🟢 Constructor vacío para serialización/deserialización JSON
    public OpcionRespuestaDto() { }
}
