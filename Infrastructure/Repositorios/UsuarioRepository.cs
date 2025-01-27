using Core.Entities;
using Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositorios
{
    public class UsuarioRepository: IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(string connectionString)
        {

            _connectionString = connectionString;
        }

        public async Task<int> CrearUsuarioAsync(Usuario usuario)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("INSERT INTO Usuarios (Identificador, Usuario, Pass)  VALUES (@Identificador, @Usuario, @Pass)", connection);

                command.Parameters.AddWithValue("@Identificador", usuario.identificador);
                command.Parameters.AddWithValue("@Usuario", usuario.usuarioNombre);
                command.Parameters.AddWithValue("@Pass", usuario.pass);
              

                await connection.OpenAsync();
                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Usuario>> GetUsuariosAsync()
        {
            try
            {
                var usuarios = new List<Usuario>();

                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("SP_ConsultarUsuarios", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    usuarios.Add(new Usuario
                    {
                        identificador = reader.GetInt32("Identificador"),
                        usuarioNombre = reader.GetString("Usuario"),
                        pass = reader.GetString("Pass"),                      
                        fechaCreacion = reader.GetDateTime("FechaCreacion")
                    });

                }

                if (usuarios.Count == 0)
                {                  

                }

                return usuarios;
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        public async Task<bool> EditarUsuarioAsync(Usuario usuario)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("SP_EditarUsuario", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        // Agregar los parámetros necesarios
                        command.Parameters.AddWithValue("@Identificador", usuario.identificador);
                        command.Parameters.AddWithValue("@Usuario", usuario.usuarioNombre);
                        command.Parameters.AddWithValue("@Pass", usuario.pass);
                     

                 
                        await connection.OpenAsync();
                        var result = await command.ExecuteScalarAsync();

                  
                        if (result != null && result.ToString() == "Registro actualizado correctamente.")
                        {
                            return true; 
                        }
                        else if (result != null && result.ToString() == "No se encontró el registro con el Identificador proporcionado.")
                        {
                            return false; 
                        }

                        return false;

                       
                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<bool> EliminarUsuarioAsync(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("SP_EliminarUsuario", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                   
                        command.Parameters.AddWithValue("@Identificador", id);
                 
                        
                        await connection.OpenAsync();
                        var result = await command.ExecuteScalarAsync();

                       
                        if (result != null && result.ToString() == "Registro eliminado.")
                        {
                            return true; 
                        }
                        else if (result != null && result.ToString() == "No se encontró el registro con el Identificador proporcionado.")
                        {
                            return false; 
                        }

                        return false;


                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public async Task<bool> GetUsuarioAsync(string usuario)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("SP_BuscarUsuario", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                       
                        command.Parameters.AddWithValue("@Usuario", usuario);
                      

                        
                        await connection.OpenAsync();
                        var result = await command.ExecuteScalarAsync();

                       
                        if (result != null && result.ToString() == "Usuario Existe")
                        {
                            return true; 
                        }
                        else if (result != null && result.ToString() == "No se encontró el registro con el Identificador proporcionado.")
                        {
                            return false; 
                        }

                        return false;


                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
