using ApiGestionStock.Interfaces;
using ApiGestionStock.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiGestionStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiendasController : ControllerBase
    {
        private readonly IRepositoryAlmacen repo;

        public TiendasController(IRepositoryAlmacen repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Tienda>>> GetTiendas()
        {
            return await this.repo.GetTiendasAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tienda>> GetTienda(int id)
        {
            var tienda = await this.repo.FindTiendaAsync(id);
            if (tienda == null)
            {
                return NotFound();
            }
            return tienda;
        }

        [HttpPost]
        public async Task<ActionResult> CreateTienda([FromBody] Tienda tienda)
        {
            await this.repo.CreateTiendaAsync(
                tienda.Nombre,
                tienda.Direccion,
                tienda.Telefono,
                tienda.Email);

            return CreatedAtAction(nameof(GetTienda), new { id = 0 }, null);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTienda([FromBody] Tienda tienda)
        {
            await this.repo.UpdateTiendaAsync(
                tienda.IdTienda,
                tienda.Nombre,
                tienda.Direccion,
                tienda.Telefono,
                tienda.Email);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTienda(int id)
        {
            await this.repo.DeleteTiendaAsync(id);
            return NoContent();
        }
    }
}
