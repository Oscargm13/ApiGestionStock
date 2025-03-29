using ApiGestionStock.Interfaces;
using ApiGestionStock.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiGestionStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IRepositoryAlmacen repo;

        public UsuarioController(IRepositoryAlmacen repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> GetUsuarios()
        {
            return await this.repo.GetUsuariosAsync();
        }

        [HttpGet("roles")]
        public async Task<ActionResult<List<Rol>>> GetRoles()
        {
            return await this.repo.GetRolesAsync();
        }

        [HttpPost]
        public async Task<ActionResult> CreateUsuario(string nombre, string email, string password, int idRol, string imagen, string nombreEmpresa)
        {
            await this.repo.CreateUsuarioAsync(nombre, email, password, idRol, imagen, nombreEmpresa);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<Usuario>> CompararUsuario(string nombreUsuario, string password)
        {
            var usuario = await this.repo.CompararUsuarioAsync(nombreUsuario, password);
            if (usuario == null)
            {
                return NotFound("Usuario o contraseña incorrectos");
            }
            return usuario;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await this.repo.FindUsuarioAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return usuario;
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUsuario(int id)
        {
            var usuario = await this.repo.FindUsuarioAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            await this.repo.DeleteUsuarioAsync(id);
            return NoContent();
        }
    }
}
