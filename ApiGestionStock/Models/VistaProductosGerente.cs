using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

#region
/*CREATE VIEW VistaProductosGerente AS
SELECT
    p.IdProducto,
    p.Nombre AS NombreProducto,
    p.Precio,
    p.Coste,
    p.IdCategoria,
    p.Imagen,
    t.IdTienda,
    t.Nombre AS NombreTienda,
    pt.Cantidad AS StockTienda,
    mt.IdUsuario AS IdGerente,
    prov.IdProveedor,
    prov.NombreEmpresa,
    u.nombre_empresa AS NombreEmpresaGerente -- Agregamos el nombre de empresa del usuario
FROM
    Productos p
JOIN
    ProductosTienda pt ON p.IdProducto = pt.IdProducto
JOIN
    Tiendas t ON pt.IdTienda = t.IdTienda
JOIN
    ManagersTiendas mt ON t.IdTienda = mt.IdTienda
LEFT JOIN
    ProductosProveedores pp ON p.IdProducto = pp.IdProducto
LEFT JOIN
    Proveedores prov ON pp.IdProveedor = prov.IdProveedor
JOIN
    Usuarios u ON mt.IdUsuario = u.IdUsuario; -- Unimos con la tabla Usuarios
*/
#endregion
namespace ApiGestionStock.Models
{
    [Table("VistaProductosGerente")]
    [PrimaryKey(nameof(IdProducto), nameof(IdTienda), nameof(IdProveedor))]
    public class VistaProductosGerente
    {
        [ForeignKey("IdProducto")]
        public int IdProducto { get; set; }

        [Column("NombreProducto")]
        public string NombreProducto { get; set; }

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

        [Column("NombreTienda")]
        public string NombreTienda { get; set; }

        [Column("StockTienda")]
        public int StockTienda { get; set; }

        [Column("IdGerente")]
        public int IdGerente { get; set; }

        [ForeignKey("IdProveedor")]
        public int IdProveedor { get; set; }

        [Column("NombreEmpresa")]
        public string NombreEmpresa { get; set; }

        [Column("NombreEmpresaGerente")]
        public string NombreEmpresaGerente { get; set; }
    }
}
