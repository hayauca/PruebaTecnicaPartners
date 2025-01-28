using Application.Services;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controllers
{
   
    public class PersonaController : ControllerBase
    {
        private readonly PersonaService _personaService;

        public PersonaController(PersonaService personaService)
        {
            _personaService = personaService;
        }

        [Authorize]
        [HttpGet]
        [Route("api/persona/getpersonas")]

        public async Task<IActionResult> GetPersonas()
        {

            try
            {
                var personas = await _personaService.GetPersonasAsync();

                return Ok(personas);
            }
            catch (Exception ex)
            {

                Log.Error(ex, "Ocurrió un error al obtener las personas.");
                return StatusCode(500, "Error interno del servidor.");
            }
                   
        }

        [Authorize]
        [HttpPost]
        [Route("api/persona/crearpersona")]

        public async Task<IActionResult> CrearPersona([FromBody] Persona request)
        {

            try
            {
                var respuesta = await _personaService.CrearPersonaAsync(request);
                return Ok(new { message = "Datos grabados correctamente." });

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Ocurrió un error al crear persona.");
                return StatusCode(500, "Error interno del servidor.");
            }

        }

        [Authorize]
        [HttpPost]
        [Route("api/persona/editarpersona")]

        public async Task<IActionResult> EditarPersona([FromBody] Persona request)
        {
            try
            {
                var respuesta = await _personaService.EditarPersonaAsync(request);
                return Ok(new { message = "Datos actualizados correctamente." });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Ocurrió un error al editar persona.");
                return StatusCode(500, "Error interno del servidor.");
            }
            
         

        }

        [Authorize]
        [HttpPost]
        [Route("api/persona/eliminarpersona/{id}")]
    
       public async Task<IActionResult> EliminarPersona(int id)
        {
            try
            {
                var respuesta = await _personaService.EliminarPersonaAsync(id);
                return Ok(new { message = "Datos eliminados correctamente." });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Ocurrió un error al eliminar persona.");
                return StatusCode(500, "Error interno del servidor.");
            }
                    

        }

    }
}
