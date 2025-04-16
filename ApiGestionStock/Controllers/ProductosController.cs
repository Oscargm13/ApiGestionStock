using ApiGestionStock.Data;
using ApiGestionStock.DTOs;
using ApiGestionStock.Interfaces;
using ApiGestionStock.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiGestionStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IRepositoryAlmacen repo; // Inyección de la interfaz

        public ProductosController(IRepositoryAlmacen repo)
        {
            this.repo = repo;
        }

        // Productos
        
        [HttpGet]
        public async Task<ActionResult<List<Producto>>> GetProductos()
        {
            return await repo.GetProductosAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await repo.FindProductoAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return producto;
        }

        [HttpGet("proveedor/{proveedorId}")]
        public async Task<ActionResult<List<Producto>>> GetProductosProveedor(int proveedorId)
        {
            return await repo.GetProductosProveedorAsync(proveedorId);
        }

        [HttpGet("tienda")]
        public async Task<ActionResult<List<VistaProductoTienda>>> GetAllVistaProductosTienda()
        {
            return await repo.GetAllVistaProductosTiendaAsync();
        }

        [HttpGet("tienda/{idTienda}")]
        public async Task<ActionResult<List<VistaProductoTienda>>> GetVistaProductosTienda(int idTienda)
        {
            return await repo.GetVistaProductosTiendaAsync(idTienda);
        }

        [HttpGet("tienda/stockbajo")]
        public async Task<ActionResult<List<VistaProductoTienda>>> GetVistaProductosTiendaConStockBajo()
        {
            return await repo.GetVistaProductosTiendaConStockBajoAsync();
        }

        [HttpGet("gerente/{idGerente}")]
        public async Task<ActionResult<List<ProductosTienda>>> GetProductosTiendaGerente(int idGerente)
        {
            return await repo.GetProductosTiendaGerenteAsync(idGerente);
        }

        [HttpGet("gerente/productos/{idUsuarioGerente}")]
        public async Task<ActionResult<List<VistaProductosGerente>>> GetProductosGerente(int idUsuarioGerente)
        {
            return await repo.GetProductosGerenteAsync(idUsuarioGerente);
        }

        [HttpGet("gerente/stock/{idUsuarioGerente}")]
        public async Task<ActionResult<int>> GetTotalStockGerente(int idUsuarioGerente)
        {
            return await repo.GetTotalStockGerenteAsync(idUsuarioGerente);
        }

        [HttpGet("tienda/{idTienda}/producto/{idProducto}")]
        public async Task<ActionResult<VistaProductoTienda>> GetProductoTienda(int idProducto, int idTienda)
        {
            var vistaProductoTienda = await repo.FindProductoTiendaAsync(idProducto, idTienda);
            if (vistaProductoTienda == null)
            {
                return NotFound();
            }
            return vistaProductoTienda;
        }

        [HttpPost]
        public async Task<ActionResult> CreateProducto([FromBody] CrearProductoDto dto)
        {
            await repo.CrearProductoAsync(dto);
            return Ok();
        }

        [HttpPut("{idProducto}")]
        public async Task<ActionResult> UpdateProducto(int idProducto, [FromBody] Producto producto)
        {
            try
            {
                await repo.UpdateProductoAsync(
                    idProducto,
                    producto.Nombre,
                    producto.Precio,
                    producto.Coste,
                    producto.IdCategoria,
                    producto.Imagen
                );
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{idProducto}")]
        public async Task<ActionResult> DeleteProducto(int idProducto)
        {
            await repo.DeleteProductoAsync(idProducto);
            return Ok();
        }

        [HttpGet("categorias")]
        public async Task<ActionResult<List<Categoria>>> GetCategorias()
        {
            return await repo.GetCategoriasAsync();
        }

        [HttpGet("id/{productoId}")]
        public async Task<ActionResult<Producto>> GetProductoPorId(int productoId)
        {
            var producto = await repo.GetProductoPorIdAsync(productoId);
            if (producto == null)
            {
                return NotFound();
            }
            return producto;
        }

        [HttpPost("actualizar-stock")]
        public async Task<IActionResult> ActualizarStock([FromBody] ActualizarStockModel dto)
        {
            if (dto == null)
            {
                return BadRequest("El modelo es nulo. Revisa el cuerpo JSON.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var actualizado = await this.repo.ActualizarStockAsync(dto.IdProducto, dto.IdTienda, dto.Cantidad);
            if (!actualizado)
            {
                return NotFound("Producto no encontrado en la tienda especificada.");
            }

            return Ok(new { mensaje = "Stock actualizado correctamente." });
        }

        public class ActualizarStockModel
        {
            public int IdProducto { get; set; }
            public int IdTienda { get; set; }
            public int Cantidad { get; set; }
        }

    }
}
