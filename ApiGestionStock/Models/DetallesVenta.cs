using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiGestionStock.Models
{
    [Table("DetallesVenta")]
    public class DetallesVenta
    {
        [Key]
        [Column("IdDetallesVenta")]
        public int IdDetallesVenta { get; set; }

        [Required(ErrorMessage = "La venta es obligatoria")]
        [ForeignKey("Venta")]
        public int IdVenta { get; set; }

        [Required(ErrorMessage = "El producto es obligatorio")]
        [ForeignKey("Producto")]
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Column("Cantidad")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio")]
        [Column("PrecioUnidad")]
        public decimal PrecioUnidad { get; set; }
    }
}
