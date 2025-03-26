using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiGestionStock.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        [Column("IdUsuario")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres")]
        [Column("Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(255, ErrorMessage = "La contraseña no puede tener más de 255 caracteres")]
        [Column("Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        [Column("Email")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "La ruta de la imagen no puede tener más de 100 caracteres")]
        [Column("Imagen")]
        public string Imagen { get; set; }

        
        [Required(ErrorMessage = "El rol es obligatorio")]
        [Column("IdRol")]
        public int IdRol { get; set; }

        [Column("nombre_empresa")]
        public string nombreEmpresa { get; set; }

        [ForeignKey("IdRol")]
        public Rol Rol { get; set; }
    }
}
