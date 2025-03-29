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
        public async Task<ActionResult> CreateTienda(string nombre, string direccion, string telefono, string email)
        {
            await this.repo.CreateTiendaAsync(nombre, direccion, telefono, email);
            return CreatedAtAction(nameof(GetTienda), new { id = 0 }, null); // Devolver CreatedAtAction correctamente
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTienda(int id, string nombre, string direccion, string telefono, string email)
        {
            await this.repo.UpdateTiendaAsync(id, nombre, direccion, telefono, email);
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
