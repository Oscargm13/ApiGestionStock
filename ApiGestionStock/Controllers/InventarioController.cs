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

        public InventarioController(IRepositoryAlmacen repo, AlmacenesContext context)
        {
            this.repo = repo;
            this.context = context;
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

        //[HttpPost("ventas")]
        //public async Task<ActionResult> ProcesarVenta([FromBody] VentaConDetalles ventaConDetalles)
        //{
        //    if (ventaConDetalles == null || ventaConDetalles.Venta == null || ventaConDetalles.Detalles == null || ventaConDetalles.Detalles.Count == 0)
        //    {
        //        return BadRequest("Datos de venta o detalles inválidos.");
        //    }

        //    try
        //    {
        //        await this.repo.ProcesarVentaAsync(ventaConDetalles.Venta, ventaConDetalles.Detalles);
        //        return Ok("Venta procesada exitosamente.");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Loguear el error
        //        return StatusCode(500, "Error al procesar venta: " + ex.Message);
        //    }
        //}

        #region Clases de Ayuda para Procesar Venta
        public class VentaConDetalles
        {
            public Venta Venta { get; set; }
            public List<DetallesVenta> Detalles { get; set; }
        }
        #endregion

        [HttpPost]
        [Route("{idVenta}/detalles")] // Usa una ruta específica para agregar detalles a una venta
        public async Task<IActionResult> AgregarDetalleVenta(int idVenta, [FromBody] DetallesVenta detalle) // Recibe el detalle en el cuerpo de la petición
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Devuelve errores de validación si el modelo no es válido
                }

                await this.repo.AgregarDetalleVenta(idVenta, detalle);

                return CreatedAtAction(nameof(GetDetalleVenta), new { idVenta = idVenta, idDetalle = detalle.IdProducto }, detalle); // Devuelve 201 Created y la ubicación del recurso creado
            }
            catch (Exception ex)
            {
                // Log the exception here
                return StatusCode(500, "Error interno del servidor: " + ex.Message); // Devuelve un error 500 y un mensaje descriptivo
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



        //[HttpPost("ventas")]
        //public async Task<ActionResult> ProcesarVenta([FromBody] VentaConDetalles ventaConDetalles)
        //{
        //    if (ventaConDetalles == null || !ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState); // Devuelve detalles de los errores de validación
        //    }

        //    using var transaction = await this.context.Database.BeginTransactionAsync();
        //    try
        //    {
        //        // 2. Ejecutar Procedimiento Almacenado (TVP)
        //        await this.repo.EjecutarProcedimientoAlmacenadoVentaAsync(ventaConDetalles.Venta, ventaConDetalles.Detalles);

        //        // 3. Verificar Stock y Crear Notificaciones
        //        await this.repo.VerificarStockBajoYCrearNotificacionesAsync(ventaConDetalles.Venta, ventaConDetalles.Detalles);

        //        await transaction.CommitAsync();

        //        return Ok("Venta procesada con éxito.");
        //    }
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync();
        //        //_logger.LogError(ex, "Error al procesar la venta");
        //        return StatusCode(500, "Error al procesar la venta");
        //    }
        //}

        

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
