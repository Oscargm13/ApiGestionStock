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
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<VistaInventarioDetalladoVenta> vistaInventarioDetalladoVenta { get; set; }
        public DbSet<DetallesVenta> DetallesVenta { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<Inventario> Inventarios { get; set; }
    }
}
