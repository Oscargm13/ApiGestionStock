using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiGestionStock.Models
{
    [Table("DetallesCompra")]
    public class DetallesCompra
    {
        [Key]
        [Column("IdDetallesCompra")]
        public int IdDetallesCompra { get; set; }

        [Required(ErrorMessage = "La compra es obligatoria")]
        [Column("IdCompra")]
        public int IdCompra { get; set; }

        [Required(ErrorMessage = "El producto es obligatorio")]
        [Column("IdProducto")]
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Column("Cantidad")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio")]
        [Column("PrecioUnidad")]
        public decimal PrecioUnidad { get; set; }

        public Compra Compra { get; set; }
        public Producto Producto { get; set; }
    }
}
