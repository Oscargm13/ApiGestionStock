﻿using System.Data;
using System.Xml.Linq;
using ApiGestionStock.Data;
using ApiGestionStock.Interfaces;
using ApiGestionStock.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public async Task<int> CreateVentaAsync(DateTime fecha, int idTienda, int idUsuario, int importeTotal, int idCliente)
        {
            Venta nuevaVenta = new Venta
            {
                FechaVenta = fecha,
                IdTienda = idTienda,
                IdUsuario = idUsuario,
                ImporteTotal = importeTotal,
                IdCliente = idCliente
            };

            this.context.Ventas.Add(nuevaVenta);
            await this.context.SaveChangesAsync();

            return nuevaVenta.IdVenta;
        }



        public async Task AgregarDetalleVenta(int idVenta, DetallesVenta detalle)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    using (var command = context.Database.GetDbConnection().CreateCommand())
                    {
                        command.Transaction = transaction.GetDbTransaction();
                        command.CommandText = "AgregarDetalleVenta";
                        command.CommandType = CommandType.StoredProcedure;

                        // Parámetros del Procedimiento Almacenado
                        command.Parameters.Add(new SqlParameter("@IdVenta", idVenta));
                        command.Parameters.Add(new SqlParameter("@IdProducto", detalle.IdProducto));
                        command.Parameters.Add(new SqlParameter("@Cantidad", detalle.Cantidad));
                        command.Parameters.Add(new SqlParameter("@PrecioUnidad", detalle.PrecioUnidad));

                        await command.ExecuteNonQueryAsync();

                        //No hay necesidad de hacer otro procedimiento almacenado. Esto lo podemos hacer en .net para mantener mejor mantenibilidad de la lógica
                        //1. Actualizar el inventario
                        var inventario = new Inventario
                        {
                            IdProducto = detalle.IdProducto,
                            FechaMovimiento = DateTime.Now,
                            TipoMovimiento = "Salida",
                            Cantidad = detalle.Cantidad,
                            IdMovimiento = idVenta  //Asumimos que el ID de la venta es el ID del movimiento en el inventario.
                        };

                        context.Inventarios.Add(inventario);
                        await context.SaveChangesAsync();

                        // 2. Actualizar el stock en ProductosTienda (puedes optar por manejar esto en el SP si lo prefieres)
                        var productoTienda = await context.ProductosTienda
                                                  .FirstOrDefaultAsync(pt => pt.IdProducto == detalle.IdProducto && pt.IdTienda == 1); //Asumimos que esto debe ser parametrizado para la tienda
                        if (productoTienda != null)
                        {
                            productoTienda.Cantidad -= detalle.Cantidad;
                            context.ProductosTienda.Update(productoTienda);
                            await context.SaveChangesAsync();
                        }

                        await transaction.CommitAsync();
                    }
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw; // Re-lanza la excepción para que se maneje en la capa superior
                }
            }
        }

        public async Task<List<VistaInventarioDetalladoVenta>> GetMovimientosAsync()
        {
            return await this.context.vistaInventarioDetalladoVenta
                .Include(i => i.Producto)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Notificacion>> GetNotificacionesAsync()
        {
            return await this.context.Notificaciones.ToListAsync();
        }

        public async Task<bool> ExisteNotificacionAsync(int idProducto, int idTienda)
        {
            return await this.context.Notificaciones
                .AnyAsync(n => n.IdProducto == idProducto && n.IdTienda == idTienda);
        }

        public async Task CreateNotificacionAsync(Notificacion notificacion)
        {
            this.context.Notificaciones.Add(notificacion);
            await this.context.SaveChangesAsync();
        }

        //public async Task ProcesarVentaAsync(Venta venta, List<DetallesVenta> detalles)
        //{
        //    using var transaction = await this.context.Database.BeginTransactionAsync();
        //    try
        //    {
        //        using var command = this.context.Database.GetDbConnection().CreateCommand();
        //        command.Transaction = transaction.GetDbTransaction();
        //        command.CommandText = "ProcesarVentaStock";
        //        command.CommandType = CommandType.StoredProcedure;

        //        command.Parameters.Add(new SqlParameter("@FechaVenta", venta.FechaVenta));
        //        command.Parameters.Add(new SqlParameter("@IdTienda", venta.IdTienda));
        //        command.Parameters.Add(new SqlParameter("@IdUsuario", venta.IdUsuario));
        //        command.Parameters.Add(new SqlParameter("@ImporteTotal", venta.ImporteTotal));
        //        command.Parameters.Add(new SqlParameter("@IdCliente", venta.IdCliente));

        //        var detallesXml = new XElement("Detalles",
        //            detalles.Select(d => new XElement("Detalle",
        //                new XElement("IdProducto", d.IdProducto),
        //                new XElement("Cantidad", d.Cantidad),
        //                new XElement("PrecioUnidad", d.PrecioUnidad)
        //            ))
        //        );

        //        command.Parameters.Add(new SqlParameter("@DetallesVenta", detallesXml.ToString()));

        //        await command.ExecuteNonQueryAsync();
        //        command.Parameters.Clear();

        //        // Verificar stock bajo y crear notificaciones (usando EF Core)
        //        foreach (var detalle in detalles)
        //        {
        //            var producto = await this.context.ProductosTienda
        //                .FirstOrDefaultAsync(pt => pt.IdProducto == detalle.IdProducto && pt.IdTienda == venta.IdTienda);

        //            if (producto != null && producto.Cantidad < 10) // Umbral de stock bajo
        //            {
        //                var notificacionExistente = await this.ExisteNotificacionAsync(detalle.IdProducto, venta.IdTienda);
        //                if (!notificacionExistente)
        //                {
        //                    var notificacion = new Notificacion
        //                    {
        //                        Mensaje = $"Aviso de stock bajo: En {venta.IdTienda} la cantidad de {detalle.IdProducto} es de {producto.Cantidad}.",
        //                        Fecha = DateTime.Now,
        //                        IdProducto = detalle.IdProducto,
        //                        IdTienda = venta.IdTienda
        //                    };
        //                    await this.CreateNotificacionAsync(notificacion);
        //                }
        //            }
        //        }

        //        await transaction.CommitAsync();
        //    }
        //    catch (Exception)
        //    {
        //        await transaction.RollbackAsync();
        //        throw;
        //    }
        //}

        public async Task EjecutarProcedimientoAlmacenadoVentaAsync(Venta venta, List<DetallesVenta> detalles)
        {
            using var command = this.context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "dbo.InsertarVentaYDetalles"; // Nombre completo (esquema.nombre)
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@FechaVenta", venta.FechaVenta));
            command.Parameters.Add(new SqlParameter("@IdTienda", venta.IdTienda));
            command.Parameters.Add(new SqlParameter("@IdUsuario", venta.IdUsuario));
            command.Parameters.Add(new SqlParameter("@ImporteTotal", venta.ImporteTotal));
            command.Parameters.Add(new SqlParameter("@IdCliente", venta.IdCliente));

            // Crear el DataTable para el TVP
            var detallesTable = new DataTable();
            detallesTable.Columns.Add("IdProducto", typeof(int));
            detallesTable.Columns.Add("Cantidad", typeof(int));
            detallesTable.Columns.Add("PrecioUnidad", typeof(decimal));

            foreach (var detalle in detalles)
            {
                detallesTable.Rows.Add(detalle.IdProducto, detalle.Cantidad, detalle.PrecioUnidad);
            }

            // Agregar el parámetro del TVP
            var detallesParameter = new SqlParameter("@DetallesVenta", SqlDbType.Structured);
            detallesParameter.TypeName = "dbo.DetallesVentaTableType"; // Nombre completo del tipo
            detallesParameter.Value = detallesTable;
            command.Parameters.Add(detallesParameter);

            await command.ExecuteNonQueryAsync();
        }

        public async Task VerificarStockBajoYCrearNotificacionesAsync(Venta venta, List<DetallesVenta> detalles)
        {
            foreach (var detalle in detalles)
            {
                var producto = await this.context.ProductosTienda
                    .FirstOrDefaultAsync(pt => pt.IdProducto == detalle.IdProducto && pt.IdTienda == venta.IdTienda);

                if (producto != null && producto.Cantidad < 10)
                {
                    var notificacionExistente = await ExisteNotificacionAsync(detalle.IdProducto, venta.IdTienda);
                    if (!notificacionExistente)
                    {
                        var notificacion = new Notificacion
                        {
                            Mensaje = $"Aviso de stock bajo: En {venta.IdTienda} la cantidad de {detalle.IdProducto} es de {producto.Cantidad}.",
                            Fecha = DateTime.Now,
                            IdProducto = detalle.IdProducto,
                            IdTienda = venta.IdTienda
                        };
                        await CreateNotificacionAsync(notificacion);
                    }
                }
            }
        }

        public async Task ProcesarCompraAsync(Compra compra, List<DetallesCompra> detalles)
        {
            using var connection = this.context.Database.GetDbConnection();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = "ProcesoCompraNotificaciones";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@FechaCompra", compra.FechaCompra));
            command.Parameters.Add(new SqlParameter("@IdProveedor", compra.IdProveedor));
            command.Parameters.Add(new SqlParameter("@IdTienda", compra.IdTienda));
            command.Parameters.Add(new SqlParameter("@ImporteTotal", compra.ImporteTotal));
            command.Parameters.Add(new SqlParameter("@IdUsuario", compra.IdUsuario));

            var detallesXml = new XElement("Detalles",
                detalles.Select(d => new XElement("Detalle",
                    new XElement("IdProducto", d.IdProducto),
                    new XElement("Cantidad", d.Cantidad),
                    new XElement("PrecioUnidad", d.PrecioUnidad)
                ))
            );

            command.Parameters.Add(new SqlParameter("@DetallesCompra", detallesXml.ToString()));

            await command.ExecuteNonQueryAsync();
            command.Parameters.Clear();
        }

        public async Task<decimal> GetIngresosMesAsync(int mes, int year)
        {
            using var command = this.context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "IngresosMes";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@mes", mes));
            command.Parameters.Add(new SqlParameter("@año", year));

            SqlParameter ingresosParameter = new("@ingresos", SqlDbType.Decimal) { Direction = ParameterDirection.Output };
            command.Parameters.Add(ingresosParameter);

            await this.context.Database.OpenConnectionAsync();
            await command.ExecuteNonQueryAsync();

            decimal ingresos = (decimal)ingresosParameter.Value;
            command.Parameters.Clear();

            return ingresos;
        }

        public async Task<DetallesVenta> GetDetallesVentaAsync(int idDetallesVenta)
        {
            return await this.context.DetallesVenta
                .FirstOrDefaultAsync(dv => dv.IdProducto == idDetallesVenta);
        }

        public async Task DeleteNotificacionAsync(int idNotificacion)
        {
            var notificacion = await this.context.Notificaciones.FindAsync(idNotificacion);

            if (notificacion != null)
            {
                this.context.Notificaciones.Remove(notificacion);
                await this.context.SaveChangesAsync();
            }
        }

        public async Task<List<Venta>> GetVentasAsync()
        {
            return await this.context.Ventas.ToListAsync();
        }

        public async Task<List<Compra>> GetComprasAsync()
        {
            return await this.context.Compras.ToListAsync();
        }
        #endregion

        #region Usuario
        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            return await this.context.Usuarios.ToListAsync();
        }

        public async Task<List<Rol>> GetRolesAsync()
        {
            return await this.context.Roles.ToListAsync();
        }

        public async Task CreateUsuarioAsync(string nombre, string email, string password, int idRol, string imagen, string nombreEmpresa)
        {
            Usuario usuario = new Usuario
            {
                Nombre = nombre,
                Email = email,
                Password = password,
                IdRol = idRol,
                Imagen = imagen,
                nombreEmpresa = nombreEmpresa
            };

            this.context.Usuarios.Add(usuario);
            await this.context.SaveChangesAsync();
        }

        public async Task<Usuario> CompararUsuarioAsync(string nombreUsuario, string password)
        {
            return await this.context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == nombreUsuario && u.Password == password);
        }

        public async Task<Usuario> FindUsuarioAsync(int idUsuario)
        {
            return await this.context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == idUsuario);
        }

        public async Task DeleteUsuarioAsync(int idUsuario)
        {
            var usuario = await this.context.Usuarios.FindAsync(idUsuario);

            if (usuario != null)
            {
                this.context.Usuarios.Remove(usuario);
                await this.context.SaveChangesAsync();
            }
        }
        #endregion

        #region Tienda
        public async Task<List<Tienda>> GetTiendasAsync()
        {
            return await this.context.Tiendas.ToListAsync();
        }

        public async Task<Tienda> FindTiendaAsync(int idTienda)
        {
            return await this.context.Tiendas.FirstOrDefaultAsync(t => t.IdTienda == idTienda);
        }

        public async Task CreateTiendaAsync(string nombre, string direccion, string telefono, string email)
        {
            Tienda nuevaTienda = new Tienda
            {
                Nombre = nombre,
                Direccion = direccion,
                Telefono = telefono,
                Email = email
            };

            this.context.Tiendas.Add(nuevaTienda);
            await this.context.SaveChangesAsync();
        }

        public async Task UpdateTiendaAsync(int idTienda, string nombre, string direccion, string telefono, string email)
        {
            Tienda tienda = await this.FindTiendaAsync(idTienda);

            if (tienda != null)
            {
                tienda.Nombre = nombre;
                tienda.Direccion = direccion;
                tienda.Telefono = telefono;
                tienda.Email = email;

                this.context.Tiendas.Update(tienda);
                await this.context.SaveChangesAsync();
            }
        }

        public async Task DeleteTiendaAsync(int idTienda)
        {
            Tienda tienda = await this.FindTiendaAsync(idTienda);

            if (tienda != null)
            {
                this.context.Tiendas.Remove(tienda);
                await this.context.SaveChangesAsync();
            }
        }
        #endregion
    }
}
