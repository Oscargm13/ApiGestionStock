using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ApiGestionStock.Models
{
    [Table("ProductosProveedores")]
    [PrimaryKey(nameof(IdProducto), nameof(IdProveedor))] // Define la clave primaria compuesta
    public class ProductoProveedor
    {
        [Column("IdProducto")]
        public int IdProducto { get; set; }

        [Column("IdProveedor")]
        public int IdProveedor { get; set; }

        [ForeignKey("IdProducto")]
        public Producto Producto { get; set; }

        [ForeignKey("IdProveedor")]
        public Proveedor Proveedor { get; set; }
    }
}
