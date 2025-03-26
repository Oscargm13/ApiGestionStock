using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#region
/*
CREATE VIEW VistaInventarioDetallado AS
SELECT
    i.IdInventario,
    i.IdProducto,
    p.Nombre AS NombreProducto,
    p.Imagen AS ImagenProducto,
    i.FechaMovimiento,
    i.TipoMovimiento,
    i.Cantidad,
    i.IdMovimiento,
    v.IdCliente,
    c.Nombre AS NombreCliente,
    v.IdTienda,
    t.Nombre AS NombreTienda,
    co.IdProveedor,
    pr.NombreEmpresa as NombreProveedor,
    co.IdTienda as IdTiendaCompra,
    ti.Nombre as NombreTiendaCompra
FROM
    Inventario i
LEFT JOIN
    Productos p ON i.IdProducto = p.IdProducto
LEFT JOIN
    Ventas v ON i.IdMovimiento = v.IdVenta
LEFT JOIN
    Clientes c ON v.IdCliente = c.IdCliente
LEFT JOIN
    Tiendas t ON v.IdTienda = t.IdTienda
LEFT JOIN
    Compras co ON i.IdMovimiento = co.IdCompra
LEFT JOIN
    Proveedores pr ON co.IdProveedor = pr.IdProveedor
LEFT JOIN
    Tiendas ti ON co.IdTienda = ti.IdTienda;
*/
#endregion

namespace ApiGestionStock.Models
{
    [Table("VistaInventarioDetallado")]
    public class VistaInventarioDetalladoVenta
    {
        [Key]
        [Column("IdInventario")]
        public int IdInventario { get; set; }

        [Column("IdProducto")]
        public int IdProducto { get; set; }

        [Column("NombreProducto")]
        public string NombreProducto { get; set; }

        [Column("ImagenProducto")]
        public string ImagenProducto { get; set; }

        [Column("FechaMovimiento")]
        public DateTime FechaMovimiento { get; set; }

        [Column("TipoMovimiento")]
        public string TipoMovimiento { get; set; }

        [Column("Cantidad")]
        public int Cantidad { get; set; }

        [Column("IdMovimiento")]
        public int IdMovimiento { get; set; }

        [Column("IdCliente")]
        public int? IdCliente { get; set; }
        [Column("NombreCliente")]
        public string? NombreCliente { get; set; }

        [Column("IdTienda")]
        public int? IdTienda { get; set; }

        [Column("NombreTienda")]
        public string? NombreTienda { get; set; }

        public Producto Producto { get; set; }
    }
}
