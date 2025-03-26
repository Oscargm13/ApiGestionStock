using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

#region
/*
 ALTER VIEW VistaProductosTienda AS
SELECT
    p.IdProducto,
    p.Nombre,
    p.Precio,
    p.Coste,
    p.IdCategoria,
    p.Imagen,
    pt.IdTienda,
    t.Nombre AS NombreTienda,
    pt.Cantidad AS StockTienda
FROM
    Productos p
JOIN
    ProductosTienda pt ON p.IdProducto = pt.IdProducto
JOIN
    Tiendas t ON pt.IdTienda = t.IdTienda;
 */
#endregion

namespace ApiGestionStock.Models
{
    [Table("VistaProductosTienda")]
    [PrimaryKey(nameof(IdProducto), nameof(IdTienda))]
    public class VistaProductoTienda
    {
        [ForeignKey("IdProducto")]
        public int IdProducto { get; set; }

        [Column("Nombre")]
        public string Nombre { get; set; }

        [Column("Precio")]
        public decimal Precio { get; set; }

        [Column("Coste")]
        public decimal Coste { get; set; }

        [Column("IdCategoria")]
        public int IdCategoria { get; set; }

        [Column("Imagen")]
        public string Imagen { get; set; }

        [ForeignKey("IdTienda")]
        public int IdTienda { get; set; }

        [Column("StockTienda")]
        public int StockTienda { get; set; }

        [Column("NombreTienda")]
        public string NombreTienda { get; set; }
    }
}
