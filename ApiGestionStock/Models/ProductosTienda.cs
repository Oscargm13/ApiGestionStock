using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestionStock.Models
{
    [Table("ProductosTienda")]
    [PrimaryKey(nameof(IdProducto), nameof(IdTienda))]
    public class ProductosTienda
    {
        [Column("IdProducto")]
        public int IdProducto { get; set; }

        [Column("IdTienda")]
        public int IdTienda { get; set; }

        [Column("Cantidad")]
        public int Cantidad { get; set; }

        [ForeignKey("IdProducto")]
        public Producto Producto { get; set; }

        [ForeignKey("IdTienda")]
        public Tienda Tienda { get; set; }
    }
}
