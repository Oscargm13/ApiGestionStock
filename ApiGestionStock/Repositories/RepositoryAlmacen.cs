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
        public async Task<List<Producto>> GetProductosAsync()
        {
            return await this.context.Productos.ToListAsync();
        }

        public async Task<Producto> FindProductoAsync(int id)
        {
            return await this.context.Productos.FirstOrDefaultAsync(p => p.IdProducto == id);
        }

        public async Task<List<Producto>> GetProductosProveedorAsync(int proveedorId)
        {
            return await this.context.ProductosProveedores
                .Where(pp => pp.IdProveedor == proveedorId)
                .Join(this.context.Productos,
                    pp => pp.IdProducto,
                    p => p.IdProducto,
                    (pp, p) => p)
                .ToListAsync();
        }

        public async Task<List<VistaProductoTienda>> GetAllVistaProductosTiendaAsync()
        {
            return await this.context.VistaProductosTienda.ToListAsync();
        }

        public async Task<List<VistaProductoTienda>> GetVistaProductosTiendaAsync(int idTienda)
        {
            return await this.context.VistaProductosTienda
                .Where(vpt => vpt.IdTienda == idTienda)
                .ToListAsync();
        }

        public async Task<List<VistaProductoTienda>> GetVistaProductosTiendaConStockBajoAsync()
        {
            return await this.context.VistaProductosTienda
                .Where(vp => vp.StockTienda < 15)
                .ToListAsync();
        }

        public async Task<List<ProductosTienda>> GetProductosTiendaGerenteAsync(int idGerente)
        {
            return await this.context.ProductosTienda
                .Join(this.context.Tiendas, pt => pt.IdTienda, t => t.IdTienda, (pt, t) => new { pt, t })
                .Join(this.context.ManagerTiendas, pt_t => pt_t.t.IdTienda, mt => mt.IdTienda, (pt_t, mt) => new { pt_t.pt, mt })
                .Where(x => x.mt.IdUsuario == idGerente)
                .Select(x => x.pt)
                .ToListAsync();
        }

        public async Task<List<VistaProductosGerente>> GetProductosGerenteAsync(int idUsuarioGerente)
        {
            return await this.context.VistaProductosGerente
                .Join(this.context.ManagerTiendas,
                    vpg => vpg.IdTienda,
                    mt => mt.IdTienda,
                    (vpg, mt) => new { vpg, mt })
                .Where(x => x.mt.IdUsuario == idUsuarioGerente)
                .Select(x => x.vpg)
                .ToListAsync();
        }

        public async Task<int> GetTotalStockGerenteAsync(int idUsuarioGerente)
        {
            return await this.context.VistaProductosGerente
                .Join(this.context.ManagerTiendas,
                    vpg => vpg.IdTienda,
                    mt => mt.IdTienda,
                    (vpg, mt) => new { vpg, mt })
                .Where(x => x.mt.IdUsuario == idUsuarioGerente)
                .SumAsync(x => x.vpg.StockTienda);
        }

        public async Task<VistaProductoTienda> FindProductoTiendaAsync(int idProducto, int idTienda)
        {
            return await this.context.VistaProductosTienda
                .FirstOrDefaultAsync(vpt => vpt.IdProducto == idProducto && vpt.IdTienda == idTienda);
        }

        public async Task CrearProductoAsync(string nombre, decimal precio, decimal coste, string nombreCategoria, int? idCategoriaPadre, string imagen)
        {
            // 1. Crear o encontrar la categoría
            Categoria categoria = await this.context.Categorias.FirstOrDefaultAsync(c => c.Nombre == nombreCategoria);

            if (categoria == null)
            {
                categoria = new Categoria
                {
                    Nombre = nombreCategoria,
                    IdCategoriaPadre = idCategoriaPadre
                };

                this.context.Categorias.Add(categoria);
                await this.context.SaveChangesAsync(); // Se asegura que el IdCategoria se genera
            }

            // 2. Crear el producto
            Producto nuevoProducto = new Producto
            {
                Nombre = nombre,
                Precio = precio,
                Coste = coste,
                IdCategoria = categoria.IdCategoria,
                Imagen = imagen
            };

            this.context.Productos.Add(nuevoProducto);
            await this.context.SaveChangesAsync();
        }

        public async Task UpdateProductoAsync(int idProducto, string nombreProducto, decimal precio, decimal coste, int idCategoria, string imagen)
        {
            Producto productoExistente = await this.context.Productos.FindAsync(idProducto);

            if (productoExistente != null)
            {
                productoExistente.Nombre = nombreProducto;
                productoExistente.Precio = precio;
                productoExistente.Coste = coste;
                productoExistente.IdCategoria = idCategoria;
                productoExistente.Imagen = imagen;

                this.context.Productos.Update(productoExistente);
                await this.context.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"No se encontró el producto con ID {idProducto}");
            }
        }

        public async Task DeleteProductoAsync(int idProducto)
        {
            Producto productoAEliminar = await this.context.Productos.FindAsync(idProducto);

            if (productoAEliminar != null)
            {
                this.context.Productos.Remove(productoAEliminar);
                await this.context.SaveChangesAsync();
            }
        }

        public async Task<List<Categoria>> GetCategoriasAsync()
        {
            return await this.context.Categorias.ToListAsync();
        }

        public async Task<Producto> GetProductoPorIdAsync(int productoId)
        {
            return await this.context.Productos.FirstOrDefaultAsync(p => p.IdProducto == productoId);
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
