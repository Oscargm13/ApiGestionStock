using ApiGestionStock.Interfaces;
using ApiGestionStock.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiGestionStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private IRepositoryAlmacen repo;
        public ClientesController(IRepositoryAlmacen repo)
        {
            this.repo = repo;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<Cliente>>>GetClientes()
        {
            return await this.repo.GetClientesAsync();
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> FindCliente(int id)
        {
            return await this.repo.FindClienteAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult> InsertCliente(Cliente cliente)
        {
            await this.repo.CreateClienteAsync(cliente.IdCliente, cliente.Nombre, cliente.Apellido, cliente.Email, cliente.Direccion,
                cliente.Telefono, cliente.FechaNacimiento, cliente.Genero);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCliente(int id)
        {
            await this.repo.DeleteClienteAsync(id);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateCliente(Cliente cliente)
        {
            await this.repo.UpdateClienteAsync(cliente.IdCliente, cliente.Nombre, cliente.Apellido, cliente.Email, cliente.Direccion,
                cliente.Telefono, cliente.FechaNacimiento, cliente.Genero);
            return Ok();
        }
    }
}
