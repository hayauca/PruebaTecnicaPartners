using Application.Services;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

            var personas = await _personaService.GetPersonasAsync();

            return Ok(personas);
        }

        [Authorize]
        [HttpPost]
        [Route("api/persona/crearpersona")]

        public async Task<IActionResult> CrearPersona([FromBody] Persona request)
        {

            var respuesta = await _personaService.CrearPersonaAsync(request);
            return Ok(new { message = "Datos grabados correctamente." });

        }

        [Authorize]
        [HttpPost]
        [Route("api/persona/editarpersona")]

        public async Task<IActionResult> EditarPersona([FromBody] Persona request)
        {
            var respuesta = await _personaService.EditarPersonaAsync(request);
            return Ok(new { message = "Datos actualizados correctamente." });

        }

        [Authorize]
        [HttpPost]
        [Route("api/persona/eliminarpersona/{id}")]
    
       public async Task<IActionResult> EliminarPersona(int id)
        {
          
            var respuesta = await _personaService.EliminarPersonaAsync(id);
            return Ok(new { message = "Datos eliminados correctamente." });

        }

    }
}
