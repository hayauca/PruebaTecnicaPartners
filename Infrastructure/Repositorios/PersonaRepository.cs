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
    public class PersonaRepository:IPersonaRepository
    {
        private readonly string _connectionString;

        public PersonaRepository(string connectionString)
        {

            _connectionString = connectionString;
        }

        public async Task<int> CrearPesonaAsync(Persona persona)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("INSERT INTO Personas (Nombres, Apellidos, NumeroIdentificacion, Email, TipoIdentificacion) " +
                                                   "VALUES (@Nombres, @Apellidos, @NumeroIdentificacion, @Email, @TipoIdentificacion)", connection);

                command.Parameters.AddWithValue("@Nombres", persona.nombres);
                command.Parameters.AddWithValue("@Apellidos", persona.apellidos);
                command.Parameters.AddWithValue("@NumeroIdentificacion", persona.numeroIdentificacion);
                command.Parameters.AddWithValue("@Email", persona.email);
                command.Parameters.AddWithValue("@TipoIdentificacion", persona.tipoIdentificacion);

                await connection.OpenAsync();
                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<bool> EditarPesonaAsync(Persona persona)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("SP_EditarPersona", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                      
                        command.Parameters.AddWithValue("@Identificador", persona.identificador);
                        command.Parameters.AddWithValue("@Nombres", persona.nombres);
                        command.Parameters.AddWithValue("@Apellidos", persona.apellidos);
                        command.Parameters.AddWithValue("@NumeroIdentificacion", persona.numeroIdentificacion);
                        command.Parameters.AddWithValue("@Email", persona.email);
                        command.Parameters.AddWithValue("@TipoIdentificacion", persona.tipoIdentificacion);

                  
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

        public async Task<bool> EliminarPesonaAsync(int identificador)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("SP_EliminarPersona", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                     
                        command.Parameters.AddWithValue("@Identificador", identificador);
                      

                       
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

        public async Task<IEnumerable<Persona>> GetPersonasAsync()
        {
            try
            {
                var personas = new List<Persona>();

                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("SP_ConsultarPersonas", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    personas.Add(new Persona
                    {
                        identificador = reader.GetInt32("Identificador"),
                        nombres = reader.GetString("Nombres"),
                        apellidos = reader.GetString("Apellidos"),
                        numeroIdentificacion = reader.GetString("NumeroIdentificacion"),
                        email = reader.GetString("Email"),
                        tipoIdentificacion = reader.GetString("TipoIdentificacion"),
                        fechaCreacion = reader.GetDateTime("FechaCreacion")
                     
                    });
                }

                if(personas.Count == 0)
                  {


                  }

                return personas;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
    }
}
