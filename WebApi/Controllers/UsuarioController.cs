using Application.Services;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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
            try
            {
                var usuarios = await _usuarioService.GetUsuariosAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {

                Log.Error(ex, "Ocurrió un error al obtener los usuarios.");
                return StatusCode(500, "Error interno del servidor.");
            }


           
        }

        [Authorize]
        [HttpPost]
        [Route("api/usuario/crearusuario")]

        public async Task<IActionResult> CrearUsuario([FromBody] Usuario request)
        {
            try
            {
                var respuesta = await _usuarioService.CrearUsuarioAsync(request);
                return Ok(new { message = "Datos grabados correctamente." });
            }
            catch (Exception ex)
            {

                Log.Error(ex, "Ocurrió un error al crear usuario.");
                return StatusCode(500, "Error interno del servidor.");
            }

        }

      
        [HttpPost]
        [Route("api/usuario/buscarusuario")]

        public async Task<IActionResult> GetUsuario(string usuario)
        {
            try
            {
                var respuesta = await _usuarioService.GetUsuarioAsync(usuario);
                return Ok(new { message = "Usuario Existe" });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Ocurrió un error al buscar usuario.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/usuario/editarusuario")]
        public async Task<IActionResult> EditarUsuario([FromBody] Usuario request)
        {
            try
            {
                var respuesta = await _usuarioService.EditarUsuarioAsync(request);
                return Ok(new { message = "Datos actualizados correctamente." });
            }
            catch (Exception ex)
            {

                Log.Error(ex, "Ocurrió un error al editar usuario.");
                return StatusCode(500, "Error interno del servidor.");
            }
            
        }

        [Authorize]
        [HttpPost]
        [Route("api/usuario/eliminarusuario/{id}")]

        public async Task<IActionResult> EliminarUsuario(int id)
        {
            try
            {
                var respuesta = await _usuarioService.EliminarUsuarioAsync(id);
                return Ok(new { message = "Datos eliminados correctamente." });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Ocurrió un error al eliminar el usuario.");
                return StatusCode(500, "Error interno del servidor.");
            }

        }
    }
}
