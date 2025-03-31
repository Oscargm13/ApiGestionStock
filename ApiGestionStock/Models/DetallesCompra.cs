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
        [ForeignKey("Compra")]
        public int IdCompra { get; set; }

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
