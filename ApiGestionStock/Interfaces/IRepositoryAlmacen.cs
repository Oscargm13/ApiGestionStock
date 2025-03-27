using ApiGestionStock.Data;
using ApiGestionStock.Models;

namespace ApiGestionStock.Interfaces
{
    public interface IRepositoryAlmacen
    {
        // Productos
        #region
        Task<List<Producto>> GetProductosAsync();
        Task<Producto> FindProductoAsync(int id);
        Task<List<Producto>> GetProductosProveedorAsync(int proveedorId);
        Task<List<VistaProductoTienda>> GetAllVistaProductosTiendaAsync();
        Task<List<VistaProductoTienda>> GetVistaProductosTiendaAsync(int idTienda);
        Task<List<VistaProductoTienda>> GetVistaProductosTiendaConStockBajoAsync();
        Task<List<ProductosTienda>> GetProductosTiendaGerenteAsync(int idGerente);
        Task<List<VistaProductosGerente>> GetProductosGerenteAsync(int idUsuarioGerente);
        Task<int> GetTotalStockGerenteAsync(int idUsuarioGerente);
        Task<VistaProductoTienda> FindProductoTiendaAsync(int idProducto, int idTienda);
        Task CrearProductoAsync(string nombre, decimal precio, decimal coste, string nombreCategoria, int? idCategoriaPadre, string imagen);
        Task UpdateProductoAsync(int idProducto, string nombreProducto, decimal precio, decimal coste, int idCategoria, string imagen);
        Task DeleteProductoAsync(int idProducto);
        Task<List<Categoria>> GetCategoriasAsync();
        Task<Producto> GetProductoPorIdAsync(int productoId);
        #endregion

        // Clientes y Proveedores
        #region
        Task<List<Cliente>> GetClientesAsync();
        Task<Cliente> FindClienteAsync(int id);
        Task CreateClienteAsync(int idCliente, string nombre, string apellido, string email, string direccion,
            string telefono, DateTime fechaNacimiento, string genero);
        Task UpdateClienteAsync(int idCliente, string nombre, string apellido, string email, string direccion,
            string telefono, DateTime fechaNacimiento, string genero);
        Task DeleteClienteAsync(int id);
        Task<List<Proveedor>> GetProveedoresAsync();
        Task<Proveedor> FindProveedorAsync(int id);
        Task CreateProveedorAsync(string nombreEmpresa, string telefono, string email, string nombreContacto, string direccion);
        Task UpdateProveedorAsync(int idProveedor, string nombreEmpresa, string telefono, string email, string nombreContacto, string direccion);
        Task DeleteProveedorAsync(int id);
        #endregion

        // Inventario
        #region
        Task<List<VistaInventarioDetalladoVenta>> GetMovimientos();
        Task<List<Notificacion>> GetNotificaciones();
        Task<bool> ExisteNotificacion(int idProducto, int idTienda, AlmacenesContext context);
        Task CrearNotificacion(Notificacion notificacion, AlmacenesContext context);
        Task ProcesarVenta(Venta venta, List<DetallesVenta> detalles);
        Task ProcesarCompra(Compra compra, List<DetallesCompra> detalles);
        Task<decimal> GetIngresosMes(int mes, int year);
        Task<DetallesVenta> GetDetallesVenta(int idDetallesVenta);
        Task DeleteNotificacion(int idNotificacion);
        Task<List<Venta>> GetVentas();
        Task<List<Compra>> GetCompras();
        #endregion

        // Usuario
        #region
        Task<List<Usuario>> GetUsuariosAsync();
        Task<List<Rol>> GetRoles();
        Task PostUsuario(string nombre, string email, string pass, int idRole);
        Task<Usuario> CompararUsuario(string nombreUsuario, string password);
        Task<Usuario> findUsuario(int idUsuario);
        #endregion

        // Tiendas
        #region
        List<Tienda> GetTiendas();
        Tienda FindTienda(int idTienda);
        void CrearTienda(int idTienda, string nombre, string direccion, string telefono, string email);
        #endregion
    }
}