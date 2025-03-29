using ApiGestionStock.Interfaces;
using ApiGestionStock.Models;
using ApiGestionStock.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiGestionStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedoresController : ControllerBase
    {
        private IRepositoryAlmacen repo;
        public ProveedoresController(IRepositoryAlmacen repo)
        {
            this.repo = repo;
        }
        [HttpGet]
        public async Task<ActionResult<List<Proveedor>>> GetProveedores()
        {
            return await this.repo.GetProveedoresAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Proveedor>> FindProveedor(int id)
        {
            return await this.repo.FindProveedorAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult> InsertProveedor(Proveedor proveedor)
        {
            await this.repo.CreateProveedorAsync(proveedor.NombreEmpresa, proveedor.Telefono, proveedor.Email, proveedor.NombreContacto, proveedor.Direccion);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProveedor(int id)
        {
            await this.repo.DeleteProveedorAsync(id);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProveedor(Proveedor proveedor)
        {
            await this.repo.UpdateProveedorAsync(proveedor.IdProveedor, proveedor.NombreEmpresa, proveedor.Telefono, proveedor.Email, proveedor.NombreContacto, proveedor.Direccion);
            return Ok();
        }
    }
}
