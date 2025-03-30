using System.Data;
using System.Xml.Linq;
using ApiGestionStock.Data;
using ApiGestionStock.Interfaces;
using ApiGestionStock.Models;
using ApiGestionStock.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiGestionStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventarioController : ControllerBase
    {
        private readonly IRepositoryAlmacen repo;
        private AlmacenesContext context;
        private readonly ILogger<RepositoryAlmacen> logger;

        public InventarioController(IRepositoryAlmacen repo, AlmacenesContext context, ILogger<RepositoryAlmacen> logger)
        {
            this.repo = repo;
            this.context = context;
            this.logger = logger;
        }

        [HttpGet("movimientos")]
        public async Task<ActionResult<List<VistaInventarioDetalladoVenta>>> GetMovimientos()
        {
            try
            {
                var movimientos = await this.repo.GetMovimientosAsync();
                return Ok(movimientos);
            }
            catch (Exception ex)
            {
                // Loguear el error
                return StatusCode(500, "Error al obtener movimientos: " + ex.Message);
            }
        }

        [HttpGet("notificaciones")]
        public async Task<ActionResult<List<Notificacion>>> GetNotificaciones()
        {
            try
            {
                var notificaciones = await this.repo.GetNotificacionesAsync();
                return Ok(notificaciones);
            }
            catch (Exception ex)
            {
                // Loguear el error
                return StatusCode(500, "Error al obtener notificaciones: " + ex.Message);
            }
        }

        [HttpGet("notificaciones/existe/{idProducto}/{idTienda}")]
        public async Task<ActionResult<bool>> ExisteNotificacion(int idProducto, int idTienda)
        {
            try
            {
                var existe = await this.repo.ExisteNotificacionAsync(idProducto, idTienda);
                return Ok(existe);
            }
            catch (Exception ex)
            {
                // Loguear el error
                return StatusCode(500, "Error al verificar existencia de notificación: " + ex.Message);
            }
        }

        [HttpPost("notificaciones")]
        public async Task<ActionResult<Notificacion>> CreateNotificacion([FromBody] Notificacion notificacion)
        {
            try
            {
                if (notificacion == null)
                {
                    return BadRequest("Datos de notificación inválidos.");
                }

                await this.repo.CreateNotificacionAsync(notificacion);
                return CreatedAtAction(nameof(GetNotificaciones), new { }, notificacion);
            }
            catch (Exception ex)
            {
                // Loguear el error
                return StatusCode(500, "Error al crear notificación: " + ex.Message);
            }
        }

        #region Clases de Ayuda para Procesar Venta
        public class VentaConDetalles
        {
            public Venta Venta { get; set; }
            public List<DetallesVenta> Detalles { get; set; }
        }
        #endregion

        [HttpPost("ventas")]
        public async Task<ActionResult<int>> CreateVenta(DateTime fechaVenta, int idTienda, int idUsuario, decimal importeTotal, int idCliente)
        {
            try
            {
                int ventaId = await this.repo.CreateVentaAsync(fechaVenta, idTienda, idUsuario, importeTotal, idCliente);
                return CreatedAtAction(nameof(GetVenta), new { id = ventaId }, ventaId); // Devuelve 201 Created y la ruta al recurso creado
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error al crear la venta.");
                return StatusCode(500, "Error al crear la venta.");
            }
        }
        #region Venta DTOs

        public class VentaConDetallesDto
        {
            public DateTime FechaVenta { get; set; }
            public int IdTienda { get; set; }
            public int IdUsuario { get; set; }
            public decimal ImporteTotal { get; set; }
            public int IdCliente { get; set; }
            public List<DetalleVentaDto> Detalles { get; set; }
        }

        public class DetalleVentaDto
        {
            public int IdProducto { get; set; }
            public int Cantidad { get; set; }
            public decimal PrecioUnidad { get; set; }
        }

        #endregion

        [HttpPost("procesarventa")]
        public async Task<ActionResult> ProcesarVenta([FromBody] VentaConDetallesDto ventaConDetallesDto)
        {
            if (ventaConDetallesDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                // 1. Mapear VentaConDetallesDto a Venta
                var venta = new Venta
                {
                    FechaVenta = ventaConDetallesDto.FechaVenta,
                    IdTienda = ventaConDetallesDto.IdTienda,
                    IdUsuario = ventaConDetallesDto.IdUsuario,
                    ImporteTotal = ventaConDetallesDto.ImporteTotal,
                    IdCliente = ventaConDetallesDto.IdCliente
                };

                // 2. Mapear DetallesVentaDto a Lista de DetallesVenta
                var detallesVentaList = ventaConDetallesDto.Detalles.Select(dto => new DetallesVenta
                {
                    IdProducto = dto.IdProducto,
                    Cantidad = dto.Cantidad,
                    PrecioUnidad = dto.PrecioUnidad
                }).ToList();

                // 3. Ejecutar Procedimiento Almacenado
                await repo.EjecutarProcedimientoAlmacenadoVentaAsync(venta, detallesVentaList);

                // 4. Verificar Stock y Crear Notificaciones
                await repo.VerificarStockBajoYCrearNotificacionesAsync(venta, detallesVentaList);

                await transaction.CommitAsync();

                return Ok("Venta procesada con éxito.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                logger.LogError(ex, "Error al procesar la venta");
                return StatusCode(500, "Error al procesar la venta");
            }
        }

        private async Task VerificarStockYCrearNotificacionesAsync(VentaConDetallesDto ventaConDetallesDto, int idVenta)
        {
            foreach (var detalle in ventaConDetallesDto.Detalles)
            {
                var productoTienda = await this.repo.GetProductoTiendaAsync(detalle.IdProducto, ventaConDetallesDto.IdTienda); // Asumiendo que es común a todos los detalles

                if (productoTienda != null && productoTienda.Cantidad < 10) // Umbral de stock bajo
                {
                    var notificacionExistente = await this.repo.ExisteNotificacionAsync(detalle.IdProducto, ventaConDetallesDto.IdTienda);

                    if (!notificacionExistente)
                    {
                        var notificacion = new Notificacion
                        {
                            Mensaje = $"Aviso de stock bajo: En {ventaConDetallesDto.IdTienda} la cantidad de {detalle.IdProducto} es de {productoTienda.Cantidad}.",
                            Fecha = DateTime.Now,
                            IdProducto = detalle.IdProducto,
                            IdTienda = ventaConDetallesDto.IdTienda
                        };

                        await this.repo.CreateNotificacionAsync(notificacion);
                    }
                }
                // Actualizar el stock
                if (productoTienda != null)
                {
                    productoTienda.Cantidad -= detalle.Cantidad;
                    await this.repo.UpdateProductoTiendaAsync(productoTienda);
                }

            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Venta>> GetVenta(int id)
        {
            try
            {
                //Supongamos que el IInventarioRepository también tiene un método para obtener una venta por ID
                var venta = await this.repo.GetVentaByIdAsync(id); // Debes implementarlo en tu repo
                if (venta == null)
                {
                    return NotFound();
                }
                return Ok(venta);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error al obtener la venta con ID {id}.", id);
                return StatusCode(500, "Error al obtener la venta.");
            }
        }

        [HttpPost("ventas/{idVenta}/detalles")]
        public async Task<ActionResult> AgregarDetalleVenta(int idVenta, [FromBody] DetallesVenta detalle)
        {
            if (detalle == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState); // Devuelve detalles de los errores de validación
            }

            using var transaction = await this.context.Database.BeginTransactionAsync();
            try
            {
                await this.repo.AgregarDetalleVentaAsync(idVenta, detalle);

                await transaction.CommitAsync();

                return Ok("Detalle de venta agregado con éxito.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                this.logger.LogError(ex, "Error al agregar detalle de venta");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet]
        [Route("{idVenta}/detalles/{idDetalle}")]
        public IActionResult GetDetalleVenta(int idVenta, int idDetalle)
        {
            // If you need a method to retrieve a specific detail
            // you would implement the logic here to retrieve it.
            // For this example, I'm returning NotFound since it's not the
            // focus of adding a detail, but you will need to implement
            // this logic.
            return NotFound();
        }

        [HttpPost("compras")]
        public async Task<ActionResult> ProcesarCompra([FromBody] CompraConDetalles compraConDetalles)
        {
            if (compraConDetalles == null || compraConDetalles.Compra == null || compraConDetalles.Detalles == null || compraConDetalles.Detalles.Count == 0)
            {
                return BadRequest("Datos de compra o detalles inválidos.");
            }

            try
            {
                await this.repo.ProcesarCompraAsync(compraConDetalles.Compra, compraConDetalles.Detalles);
                return Ok("Compra procesada exitosamente.");
            }
            catch (Exception ex)
            {
                // Loguear el error
                return StatusCode(500, "Error al procesar compra: " + ex.Message);
            }
        }

        [HttpGet("ingresos/{mes}/{year}")]
        public async Task<ActionResult<decimal>> GetIngresosMes(int mes, int year)
        {
            try
            {
                var ingresos = await this.repo.GetIngresosMesAsync(mes, year);
                return Ok(ingresos);
            }
            catch (Exception ex)
            {
                // Loguear el error
                return StatusCode(500, "Error al obtener ingresos del mes: " + ex.Message);
            }
        }

        [HttpGet("detallesventa/{idDetallesVenta}")]
        public async Task<ActionResult<DetallesVenta>> GetDetallesVenta(int idDetallesVenta)
        {
            try
            {
                var detallesVenta = await this.repo.GetDetallesVentaAsync(idDetallesVenta);
                if (detallesVenta == null)
                {
                    return NotFound("Detalles de venta no encontrados.");
                }
                return Ok(detallesVenta);
            }
            catch (Exception ex)
            {
                // Loguear el error
                return StatusCode(500, "Error al obtener detalles de venta: " + ex.Message);
            }
        }

        [HttpDelete("notificaciones/{idNotificacion}")]
        public async Task<ActionResult> DeleteNotificacion(int idNotificacion)
        {
            try
            {
                await this.repo.DeleteNotificacionAsync(idNotificacion);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Loguear el error
                return StatusCode(500, "Error al eliminar notificación: " + ex.Message);
            }
        }

        [HttpGet("ventas")]
        public async Task<ActionResult<List<Venta>>> GetVentas()
        {
            try
            {
                var ventas = await this.repo.GetVentasAsync();
                return Ok(ventas);
            }
            catch (Exception ex)
            {
                // Loguear el error
                return StatusCode(500, "Error al obtener ventas: " + ex.Message);
            }
        }

        [HttpGet("compras")]
        public async Task<ActionResult<List<Compra>>> GetCompras()
        {
            try
            {
                var compras = await this.repo.GetComprasAsync();
                return Ok(compras);
            }
            catch (Exception ex)
            {
                // Loguear el error
                return StatusCode(500, "Error al obtener compras: " + ex.Message);
            }
        }

        #region Clases de Ayuda para Procesar Venta y Compra
        
        public class CompraConDetalles
        {
            public Compra Compra { get; set; }
            public List<DetallesCompra> Detalles { get; set; }
        }
        #endregion

        
    }
}
