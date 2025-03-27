using ApiGestionStock.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiGestionStock.Data
{
    public class AlmacenesContext: DbContext
    {
        public AlmacenesContext(DbContextOptions<AlmacenesContext> options): base(options) { }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<VistaProductoTienda> VistaProductosTienda { get; set; }
        public DbSet<ManagerTienda> ManagerTiendas { get; set; }
        public DbSet<VistaProductosGerente> VistaProductosGerente { get; set; }
        public DbSet<ProductosTienda> ProductosTienda { get; set; }
        public DbSet<Tienda> Tiendas { get; set; }
        public DbSet<ProductoProveedor> ProductosProveedores { get; set; }
    }
}
