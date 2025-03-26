using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiGestionStock.Models
{
    [Table("Proveedores")]
    public class Proveedor
    {
        [Key]
        [Column("IdProveedor")]
        public int IdProveedor { get; set; }

        [Required(ErrorMessage = "El nombre de la empresa es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre de la empresa no puede tener más de 100 caracteres")]
        [Column("NombreEmpresa")]
        public string NombreEmpresa { get; set; }

        [StringLength(20, ErrorMessage = "El teléfono no puede tener más de 20 caracteres")]
        [Column("Telefono")]
        public string Telefono { get; set; }

        [EmailAddress(ErrorMessage = "El email no es válido")]
        [Column("Email")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "El nombre de contacto no puede tener más de 100 caracteres")]
        [Column("NombreContacto")]
        public string NombreContacto { get; set; }

        [StringLength(255, ErrorMessage = "La dirección no puede tener más de 255 caracteres")]
        [Column("Direccion")]
        public string Direccion { get; set; }

        //public ICollection<ProductoProveedor> ProductosProveedores { get; set; }
        //public ICollection<Compra> Compras { get; set; }
    }
}
