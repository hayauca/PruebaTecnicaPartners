using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IPersonaRepository
    {
        Task<IEnumerable<Persona>> GetPersonasAsync();

        Task<int> CrearPesonaAsync(Persona persona);

        Task<bool> EditarPesonaAsync(Persona persona);

        Task<bool> EliminarPesonaAsync(int identificador);

    }
}
