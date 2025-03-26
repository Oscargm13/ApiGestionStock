using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiGestionStock.Models
{
    [Table("Compras")]
    public class Compra
    {
        [Key]
        [Column("IdCompra")]
        public int IdCompra { get; set; }

        [Required(ErrorMessage = "La fecha de compra es obligatoria")]
        [Column("FechaCompra")]
        public DateTime FechaCompra { get; set; }

        [Required(ErrorMessage = "El proveedor es obligatorio")]
        [Column("IdProveedor")]
        public int IdProveedor { get; set; }

        [ForeignKey("IdProveedor")]  // 🔹 Indica que la FK real es "IdProveedor"
        public Proveedor Proveedor { get; set; }

        [Required(ErrorMessage = "La tienda es obligatoria")]
        [Column("IdTienda")]
        public int IdTienda { get; set; }

        [ForeignKey("IdTienda")]  // 🔹 Indica que la FK real es "IdTienda"
        public Tienda Tienda { get; set; }

        [Column("IdUsuario")]
        public int? IdUsuario { get; set; }  // Puede ser nullable si no es obligatorio

        [ForeignKey("IdUsuario")]  // 🔹 Indica que la FK real es "IdUsuario"
        public Usuario Usuario { get; set; }

        [Required(ErrorMessage = "El importe total es obligatorio")]
        [Column("ImporteTotal")]
        public decimal ImporteTotal { get; set; }

        public ICollection<DetallesCompra> DetallesCompras { get; set; }
    }

}
