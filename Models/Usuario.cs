namespace SupabaseApiDemo.Models;

public class Usuario
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Nombre { get; set; }
    public int IdRol { get; set; }
    public string Contrasena { get; set; }
    public int? IdBodega { get; set; }
    public bool Estado { get; set; }
}
