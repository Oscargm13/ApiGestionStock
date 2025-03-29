using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiGestionStock.Models
{
    [Table("Inventario")]
    public class Inventario
    {
        [Key]
        [Column("IdInventario")]
        public int IdInventario { get; set; }

        [ForeignKey("Producto")]
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        [Column("FechaMovimiento")]
        public DateTime FechaMovimiento { get; set; }

        [Required(ErrorMessage = "El tipo de movimiento es obligatorio")]
        [StringLength(10, ErrorMessage = "El tipo de movimiento no puede tener más de 10 caracteres")]
        [Column("TipoMovimiento")]
        public string TipoMovimiento { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Column("Cantidad")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El ID de movimiento es obligatorio")]
        [Column("IdMovimiento")]
        public int IdMovimiento { get; set; }

    }
}
