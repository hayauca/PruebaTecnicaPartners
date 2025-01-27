using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
      
        public async Task<IEnumerable<Usuario>> GetUsuariosAsync()
        {

            return await _usuarioRepository.GetUsuariosAsync();
        }

    
        public async Task<int> CrearUsuarioAsync(Usuario usuario)
        {
            return await _usuarioRepository.CrearUsuarioAsync(usuario);
        }

        public async Task<bool> EditarUsuarioAsync(Usuario usuario)
        {
            return await _usuarioRepository.EditarUsuarioAsync(usuario);
        }

        public async Task<bool> GetUsuarioAsync(string usuario)
        {
            return await _usuarioRepository.GetUsuarioAsync(usuario);
        }

        public async Task<bool> EliminarUsuarioAsync(int id)
        {
            return await _usuarioRepository.EliminarUsuarioAsync(id);
        }

    }
}
