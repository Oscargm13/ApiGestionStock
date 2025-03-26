using ApiGestionStock.Data;
using ApiGestionStock.Interfaces;
using ApiGestionStock.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiGestionStock.Repositories
{
    public class RepositoryAlmacen : IRepositoryAlmacen
    {
        private AlmacenesContext context;

        public RepositoryAlmacen(AlmacenesContext context)
        {
            this.context = context;
        }
        #region Productos
        public List<Producto> GetProductos()
        {
            throw new NotImplementedException();
        }

        public Task<List<Producto>> GetProductosProveedor(int proveedorId)
        {
            throw new NotImplementedException();
        }

        public List<VistaProductoTienda> GetAllVistaProductosTienda()
        {
            throw new NotImplementedException();
        }

        public List<VistaProductoTienda> GetVistaProductosTienda(int idTienda)
        {
            throw new NotImplementedException();
        }

        public Task<List<VistaProductoTienda>> GetVistaProductosTiendaConStockBajo()
        {
            throw new NotImplementedException();
        }

        public List<ProductosTienda> GetProductosTiendaGerente(int idGerente)
        {
            throw new NotImplementedException();
        }

        public List<VistaProductosGerente> GetProductosGerente(int idUsuarioGerente)
        {
            throw new NotImplementedException();
        }

        public int GetTotalStockGerente(int idUsuarioGerente)
        {
            throw new NotImplementedException();
        }

        public VistaProductoTienda FindProductoTienda(int idProducto, int idTienda)
        {
            throw new NotImplementedException();
        }

        public Task<Producto> FindProductoAsync(int idProducto)
        {
            throw new NotImplementedException();
        }

        public List<VistaProductosGerente> FindProductoManager(int idProducto, int idUsuarioGerente)
        {
            throw new NotImplementedException();
        }

        public List<Producto> findProductosCategoria(int idCategoria)
        {
            throw new NotImplementedException();
        }

        public void CrearProducto(string nombreProducto, decimal precio, decimal coste, string nombreCategoria, int? idCategoriaPadre, string imagen)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProductoAsync(int idProducto, string nombreProducto, decimal precio, decimal coste, int idCategoria, string imagen)
        {
            throw new NotImplementedException();
        }

        public Task EliminarProducto(int idProducto)
        {
            throw new NotImplementedException();
        }

        public Task<List<Categoria>> GetCategoriasAsync()
        {
            throw new NotImplementedException();
        }

        public Producto GetProductoPorId(int productoId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Clientes y Proveedores
        public async Task<List<Cliente>> GetClientesAsync()
        {
            return await this.context.Clientes.ToListAsync();
        }

        public async Task<Cliente> FindClienteAsync(int id)
        {
            return await this.context.Clientes.FirstOrDefaultAsync(x => x.IdCliente == id);
        }

        public async Task CreateClienteAsync(int idCliente, string nombre, string apellido, string email, string direccion,
            string telefono, DateTime fechaNacimiento, string genero)
        {
            Cliente cliente = new Cliente();
            //cliente.IdCliente = idCliente;
            cliente.Nombre = nombre;
            cliente.Apellido = apellido;
            cliente.Email = email;
            cliente.Direccion = direccion;
            cliente.Telefono = telefono;
            cliente.FechaNacimiento = fechaNacimiento;
            cliente.Genero = genero;
            await this.context.Clientes.AddAsync(cliente);
            await this.context.SaveChangesAsync();
        }

        public async Task UpdateClienteAsync(int idCliente, string nombre, string apellido, string email, string direccion,
            string telefono, DateTime fechaNacimiento, string genero)
        {
            Cliente cliente = await this.FindClienteAsync(idCliente);
            cliente.IdCliente = idCliente;
            cliente.Nombre = nombre;
            cliente.Apellido = apellido;
            cliente.Email = email;
            cliente.Direccion = direccion;
            cliente.Telefono = telefono;
            cliente.FechaNacimiento = fechaNacimiento;
            cliente.Genero = genero;
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteClienteAsync(int id)
        {
            Cliente cliente = await this.FindClienteAsync(id);
            this.context.Clientes.Remove(cliente);
            await this.context.SaveChangesAsync();
        }

        public async Task<List<Proveedor>> GetProveedoresAsync()
        {
            return await this.context.Proveedores.ToListAsync();
        }

        public async Task<Proveedor> FindProveedorAsync(int id)
        {
            return await this.context.Proveedores.FirstOrDefaultAsync(x => x.IdProveedor == id);
        }

        public async Task CreateProveedorAsync(string nombreEmpresa, string telefono, string email, string nombreContacto, string direccion)
        {
            Proveedor proveedor = new Proveedor();
            proveedor.NombreEmpresa = nombreEmpresa;
            proveedor.Telefono = telefono;
            proveedor.Email = email;
            proveedor.NombreContacto = nombreContacto;
            proveedor.Direccion = direccion;
            await this.context.Proveedores.AddAsync(proveedor);
            await this.context.SaveChangesAsync();
        }

        public async Task UpdateProveedorAsync(int idProveedor, string nombreEmpresa, string telefono, string email, string nombreContacto, string direccion)
        {
            Proveedor proveedor = await this.FindProveedorAsync(idProveedor);
            proveedor.NombreEmpresa = nombreEmpresa;
            proveedor.Telefono = telefono;
            proveedor.Email = email;
            proveedor.NombreContacto = nombreContacto;
            proveedor.Direccion = direccion;
            this.context.Proveedores.Update(proveedor);
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteProveedorAsync(int id)
        {
            Proveedor proveedor = await this.FindProveedorAsync(id);
            this.context.Proveedores.Remove(proveedor);
            await this.context.SaveChangesAsync();
        }
        #endregion

        #region Inventario
        public Task<List<VistaInventarioDetalladoVenta>> GetMovimientos()
        {
            throw new NotImplementedException();
        }

        public Task<List<Notificacion>> GetNotificaciones()
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExisteNotificacion(int idProducto, int idTienda, AlmacenesContext context)
        {
            throw new NotImplementedException();
        }

        public Task CrearNotificacion(Notificacion notificacion, AlmacenesContext context)
        {
            throw new NotImplementedException();
        }

        public Task ProcesarVenta(Venta venta, List<DetallesVenta> detalles)
        {
            throw new NotImplementedException();
        }

        public Task ProcesarCompra(Compra compra, List<DetallesCompra> detalles)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> GetIngresosMes(int mes, int year)
        {
            throw new NotImplementedException();
        }

        public Task<DetallesVenta> GetDetallesVenta(int idDetallesVenta)
        {
            throw new NotImplementedException();
        }

        public Task DeleteNotificacion(int idNotificacion)
        {
            throw new NotImplementedException();
        }

        public Task<List<Venta>> GetVentas()
        {
            throw new NotImplementedException();
        }

        public Task<List<Compra>> GetCompras()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Usuario
        public Task<List<Usuario>> GetUsuariosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Rol>> GetRoles()
        {
            throw new NotImplementedException();
        }

        public Task PostUsuario(string nombre, string email, string pass, int idRole)
        {
            throw new NotImplementedException();
        }

        public Task<Usuario> CompararUsuario(string nombreUsuario, string password)
        {
            throw new NotImplementedException();
        }

        public Task<Usuario> findUsuario(int idUsuario)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Tienda
        public List<Tienda> GetTiendas()
        {
            throw new NotImplementedException();
        }

        public Tienda FindTienda(int idTienda)
        {
            throw new NotImplementedException();
        }

        public void CrearTienda(int idTienda, string nombre, string direccion, string telefono, string email)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
