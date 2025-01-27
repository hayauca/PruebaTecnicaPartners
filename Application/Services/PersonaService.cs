using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PersonaService
    {
        private readonly IPersonaRepository _personaRepository;

        public PersonaService(IPersonaRepository personaRepository)
        {
            _personaRepository = personaRepository;
        }

        public async Task<IEnumerable<Persona>> GetPersonasAsync()
        {

            return await _personaRepository.GetPersonasAsync();
        }


        public async Task<int> CrearPersonaAsync(Persona persona)
        {
            return await _personaRepository.CrearPesonaAsync(persona);
        }

        public async Task<bool> EditarPersonaAsync(Persona persona)
        {
            return await _personaRepository.EditarPesonaAsync(persona);
        }

        public async Task<bool> EliminarPersonaAsync(int identificador)
        {
            return await _personaRepository.EliminarPesonaAsync(identificador);
        }

    }
}
