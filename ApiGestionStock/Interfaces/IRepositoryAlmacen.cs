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
        Task<List<VistaInventarioDetalladoVenta>> GetMovimientosAsync();
        Task<List<Notificacion>> GetNotificacionesAsync();
        Task<bool> ExisteNotificacionAsync(int idProducto, int idTienda);
        Task CreateNotificacionAsync(Notificacion notificacion);
        //Task ProcesarVentaAsync(Venta venta, List<DetallesVenta> detalles);
        Task ProcesarCompraAsync(Compra compra, List<DetallesCompra> detalles);
        Task<decimal> GetIngresosMesAsync(int mes, int year);
        Task<DetallesVenta> GetDetallesVentaAsync(int idDetallesVenta);
        Task DeleteNotificacionAsync(int idNotificacion);
        Task<List<Venta>> GetVentasAsync();
        Task<List<Compra>> GetComprasAsync();
        Task VerificarStockBajoYCrearNotificacionesAsync(Venta venta, List<DetallesVenta> detalles);
        Task EjecutarProcedimientoAlmacenadoVentaAsync(Venta venta, List<DetallesVenta> detalles);
        Task<int>CreateVentaAsync(DateTime fecha, int idTienda, int idUsuario, int importeTotal, int idCliente);
        Task AgregarDetalleVenta(int idVenta, DetallesVenta detalle);
        #endregion

        // Usuario
        #region
        Task<List<Usuario>> GetUsuariosAsync();
        Task<List<Rol>> GetRolesAsync();
        Task CreateUsuarioAsync(string nombre, string email, string pass, int idRole, string imagen, string nombreEmpresa);
        Task<Usuario> CompararUsuarioAsync(string nombreUsuario, string password);
        Task<Usuario> FindUsuarioAsync(int idUsuario);
        Task DeleteUsuarioAsync(int idUsuario);
        #endregion

        // Tiendas
        #region
        Task<List<Tienda>> GetTiendasAsync();
        Task<Tienda> FindTiendaAsync(int idTienda);
        Task CreateTiendaAsync(string nombre, string direccion, string telefono, string email);
        Task UpdateTiendaAsync(int idTienda, string nombre, string direccion, string telefono, string email);
        Task DeleteTiendaAsync(int idTienda);
        #endregion
    }
}