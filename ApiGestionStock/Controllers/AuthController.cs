using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ApiGestionStock.Helpers;
using ApiGestionStock.Interfaces;
using ApiGestionStock.Models;  
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace ApiGestionStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IRepositoryAlmacen repo;
        private HelperActionServicesOAuth helper;
        public AuthController(IRepositoryAlmacen repo, HelperActionServicesOAuth helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Login(LoginModel model)
        {
            // Validamos credenciales
            Usuario usuario = await this.repo.LoginUsuarioAsync(model.UserName, model.Password);
            if (usuario == null)
            {
                return Unauthorized();
            }

            // Configuramos credenciales para firmar el token
            var credentials = new SigningCredentials(this.helper.GetKeyToken(), SecurityAlgorithms.HmacSha256);

            // Creamos los claims individuales (más limpios y seguros)
            var claims = new[]
            {
                new Claim("IdUsuario", usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, usuario.Email),
                new Claim("Nombre", usuario.Nombre),
                //new Claim("Rol", usuario.Rol ?? "") // Evitamos null
            };

            // Creamos el token JWT
            var token = new JwtSecurityToken(
                issuer: this.helper.Issuer,
                audience: this.helper.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials
            );

            // Devolvemos el token como string
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                response = tokenString
            });
        }

    }
}
