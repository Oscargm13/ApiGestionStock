using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiGestionStock.Models
{
    [Table("CLIENTES")]
    public class Cliente
    {
        [Key]
        [Column("IdCliente")]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres")]
        [Column("Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(100, ErrorMessage = "El apellido no puede tener más de 100 caracteres")]
        [Column("Apellido")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        [Column("Email")]
        public string Email { get; set; }

        [StringLength(255, ErrorMessage = "La dirección no puede tener más de 255 caracteres")]
        [Column("Direccion")]
        public string Direccion { get; set; }

        [StringLength(20, ErrorMessage = "El teléfono no puede tener más de 20 caracteres")]
        [Column("Telefono")]
        public string Telefono { get; set; }

        [DataType(DataType.Date)]
        [Column("FechaNacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Column("Genero")]
        public string Genero { get; set; }

    }
}
