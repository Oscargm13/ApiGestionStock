using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ApiGestionStock.Helpers;
using ApiGestionStock.Interfaces;
using ApiGestionStock.Models;
using ApiGestionStock.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            Usuario usuario = await this.repo.LoginUsuarioAsync(model.UserName, model.Password);
            if (usuario == null)
            {
                return Unauthorized();
            }
            else
            {
                //DEBEMOS CREAR UNAS CREDENCIALES PARA EL USUARIO PARA INCLUIRLAS DENTRO DEL TOKKEN
                //QUE ESTARAN COMPUESTAS POR EL SECRET KEY CIFRADO Y CON EL TOKEN
                SigningCredentials credentials =
                    new SigningCredentials(this.helper.GetKeyToken(), SecurityAlgorithms.HmacSha256);
                string jsonEmpleado = JsonConvert.SerializeObject(usuario);
                Claim[] informacion = new[]
                {
                    new Claim("UserData", jsonEmpleado),
                    //new Claim(ClaimTypes.Role, "PRESIDENTE")
                };
                //EL TOKEN SE GENEREA CON UNA CLASE Y DEBEMOS INDICAR LOS DATOS QUE ALMMACENARA EN SU INTERIOR
                JwtSecurityToken token = new JwtSecurityToken(
                    claims: informacion,
                    issuer: this.helper.Issuer,
                    audience: this.helper.Audience,
                    signingCredentials: credentials,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    notBefore: DateTime.UtcNow
                    );
                return Ok(new
                {
                    response = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
        }
    }
}
