using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiGestionStock.Models
{
    [Table("Ventas")]
    public class Venta
    {
        [Key]
        [Column("IdVenta")]
        public int IdVenta { get; set; }

        [Required(ErrorMessage = "La fecha de venta es obligatoria")]
        [Column("FechaVenta")]
        public DateTime FechaVenta { get; set; }

        [Required(ErrorMessage = "La tienda es obligatoria")]
        [ForeignKey("Tienda")]
        public int IdTienda { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio")]
        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El importe total es obligatorio")]
        [Column("ImporteTotal")]
        public decimal ImporteTotal { get; set; }

        [Column("IdCliente")]
        public int IdCliente { get; set; }

    }
}
