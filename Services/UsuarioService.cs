namespace SupabaseApiDemo.Services;
using SupabaseApiDemo.Models;
using Npgsql;



public class UsuarioService
{
    private readonly string _connectionString;

    public UsuarioService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Usuario> GetUsuarios()
    {
        var usuarios = new List<Usuario>();

        using var conn = new NpgsqlConnection(_connectionString);
        conn.Open();

        var query = "SELECT id, email, nombre, id_rol, contrasena, id_bodega, estado FROM usuario";
        using var cmd = new NpgsqlCommand(query, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            usuarios.Add(new Usuario
            {
                Id = reader.GetInt32(0),
                Email = reader.GetString(1),
                Nombre = reader.GetString(2),
                IdRol = reader.GetInt32(3),
                Contrasena = reader.GetString(4),
                IdBodega = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                Estado = reader.GetBoolean(6)
            });
        }

        return usuarios;
    }
}
