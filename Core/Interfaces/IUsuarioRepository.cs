using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetUsuariosAsync();

        Task<int> CrearUsuarioAsync(Usuario usuario);

        Task<bool> EditarUsuarioAsync(Usuario usuario);

        Task<bool> EliminarUsuarioAsync(int id);

        Task<bool> GetUsuarioAsync(string usuario);
    }
}
