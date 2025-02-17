using System.Text.Json.Serialization;

public class Usuario
{
    [JsonPropertyName("idUsuario")]
    public int IdUsuario { get; set; }

    [JsonPropertyName("correo")]
    public string Correo { get; set; }

    [JsonPropertyName("contraseña")]
    public string Contraseña { get; set; }

    [JsonPropertyName("idRol")]
    public int IdRol { get; set; }

    public string RoleName => IdRol switch
    {
        1 => "Administrador",
        2 => "Encuestador",
        3 => "Desarrollador",
        _ => "Desconocido"
    };
}
