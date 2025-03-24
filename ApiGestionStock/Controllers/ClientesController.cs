using ApiGestionStock.Models;
using ApiGestionStock.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiGestionStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private RepositoryClientes repo;
        public ClientesController(RepositoryClientes repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Cliente>>>GetDepartamentos()
        {
            return await this.repo.GetClientesAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> FindDepartamento(int id)
        {
            return await this.repo.FindClienteAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult> InsertDepartamento(Cliente cliente)
        {
            await this.repo.CreateClienteAsync(cliente.IdCliente, cliente.Nombre, cliente.Apellido, cliente.Email, cliente.Direccion,
                cliente.Telefono, cliente.FechaNacimiento, cliente.Genero);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDepartamento(int id)
        {
            await this.repo.DeleteClienteAsync(id);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateDepartamento(Cliente cliente)
        {
            await this.repo.UpdateClienteAsync(cliente.IdCliente, cliente.Nombre, cliente.Apellido, cliente.Email, cliente.Direccion,
                cliente.Telefono, cliente.FechaNacimiento, cliente.Genero);
            return Ok();
        }
    }
}
