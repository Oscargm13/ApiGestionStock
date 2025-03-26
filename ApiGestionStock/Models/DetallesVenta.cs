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
        [Column("IdVenta")]
        public int IdVenta { get; set; }

        [Required(ErrorMessage = "El producto es obligatorio")]
        [Column("IdProducto")]
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Column("Cantidad")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio")]
        [Column("PrecioUnidad")]
        public decimal PrecioUnidad { get; set; }

        public Venta Venta { get; set; }
        public Producto Producto { get; set; }
    }
}
