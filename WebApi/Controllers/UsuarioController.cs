using Application.Services;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
  
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [Authorize]
        [HttpGet]
        [Route("api/usuario/getusuarios")]

        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _usuarioService.GetUsuariosAsync();


            return Ok(usuarios);
        }

        [Authorize]
        [HttpPost]
        [Route("api/usuario/crearusuario")]

        public async Task<IActionResult> CrearUsuario([FromBody] Usuario request)
        {

            var respuesta = await _usuarioService.CrearUsuarioAsync(request);
            return Ok(new { message = "Datos grabados correctamente." });

        }

      
        [HttpPost]
        [Route("api/usuario/buscarusuario")]

        public async Task<IActionResult> GetUsuario(string usuario)
        {

            var respuesta = await _usuarioService.GetUsuarioAsync(usuario);
            return Ok(new { message = "Usuario Existe" });


        }

        [Authorize]
        [HttpPost]
        [Route("api/usuario/editarusuario")]
        public async Task<IActionResult> EditarUsuario([FromBody] Usuario request)
        {
            var respuesta = await _usuarioService.EditarUsuarioAsync(request);
            return Ok(new { message = "Datos actualizados correctamente." });


        }

        [Authorize]
        [HttpPost]
        [Route("api/usuario/eliminarusuario/{id}")]

        public async Task<IActionResult> EliminarUsuario(int id)
        {

            var respuesta = await _usuarioService.EliminarUsuarioAsync(id);
            return Ok(new { message = "Datos eliminados correctamente." });


        }
    }
}
