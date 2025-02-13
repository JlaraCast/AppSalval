using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Encuestado
{
    [Key]
    [Required]
    [MaxLength(50)]
    public string Identificacion { get; set; }

    [Required]
    [MaxLength(50)]
    public string TipoIdentificacion { get; set; }

    [Required]
    [MaxLength(255)]
    public string NombreCompleto { get; set; }

    [Required]
    public DateTime FechaNacimiento { get; set; }

    [Required]
    [Column(TypeName = "decimal(5,2)")]
    public decimal Altura { get; set; } // En metros

    [Required]
    [Column(TypeName = "decimal(5,2)")]
    public decimal Peso { get; set; } // En kilogramos

        [NotMapped] // No se almacena en la base de datos, se calcula dinámicamente
        public decimal IMC => Peso / (Altura * Altura); // Cálculo del Índice de Masa Corporal

    [Required]
    [MaxLength(50)]
    public string Sexo { get; set; }

        [MaxLength(255)]
        public string? Ubicacion { get; set; } // Provincia, cantón, distrito (opcional)

        public int? IdUsuario { get; set; } // Relación con usuario
        [ForeignKey("IdUsuario")]
        public Usuario? Usuario { get; set; } // Relación con el usuario
    }
}
