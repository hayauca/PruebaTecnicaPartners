using Application.Services;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApi.Controllers
{

    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly UsuarioService _usuarioService;

        public TokenController(IConfiguration configuration, UsuarioService usuarioService)
        {
            _configuration = configuration;
            _usuarioService = usuarioService;
        }

  

        [HttpPost]
        [Route("api/token")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
          
            if (!String.IsNullOrEmpty(login.usuario)  && !String.IsNullOrEmpty(login.password))
            {

                var respuesta = await _usuarioService.GetUsuarioAsync(login.usuario);
              

                if(respuesta)
                {
                    var token = GenerateJwtToken(login.usuario);
                    return Ok(new { token });
                }
                else
                {
                    return Unauthorized("Usuario no Existe.");

                }             
            }

            return Unauthorized("Credenciales inválidas.");
        }

        private string GenerateJwtToken(string username)
        {

            try
            {
                var jwtKey = _configuration["Jwt:Key"];
                var jwtIssuer = _configuration["Jwt:Issuer"];

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

                var token = new JwtSecurityToken(
                    issuer: jwtIssuer,
                    audience: jwtIssuer,
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
    }
}
