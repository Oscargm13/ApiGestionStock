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
        [ForeignKey("Proveedor")]
        public int IdProveedor { get; set; }

        [Required(ErrorMessage = "La tienda es obligatoria")]
        [ForeignKey("Tienda")]
        public int IdTienda { get; set; }

        [ForeignKey("Usuario")]
        public int? IdUsuario { get; set; }

        [Required(ErrorMessage = "El importe total es obligatorio")]
        [Column("ImporteTotal")]
        public decimal ImporteTotal { get; set; }

    }

}
