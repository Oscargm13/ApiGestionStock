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
        [Column("IdTienda")]
        public int IdTienda { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio")]
        [Column("IdUsuario")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El importe total es obligatorio")]
        [Column("ImporteTotal")]
        public decimal ImporteTotal { get; set; }

        [Column("IdCliente")]
        public int IdCliente { get; set; }

        public Tienda Tienda { get; set; }
        public Usuario Usuario { get; set; }
        public ICollection<DetallesVenta> DetallesVentas { get; set; }
    }
}
