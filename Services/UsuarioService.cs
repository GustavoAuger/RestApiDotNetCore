using SupabaseApiDemo.Models;
using SupabaseApiDemo.Data;
using SupabaseApiDemo.Utils;
using Microsoft.EntityFrameworkCore;

namespace SupabaseApiDemo.Services
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> GetAllUsuariosAsync();
        Task<Usuario?> GetUsuarioByIdAsync(int id);
        Task<Usuario?> GetUsuarioByEmailAsync(string email);
        Task<Usuario> CreateUsuarioAsync(Usuario usuario);
        Task<Usuario?> UpdateUsuarioAsync(int id, Usuario usuario);
        Task<bool> DeleteUsuarioAsync(int id);

    }

    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDbContext _context;

        public UsuarioService(ApplicationDbContext context){
            _context = context;
        }
        public async Task<IEnumerable<Usuario>> GetAllUsuariosAsync() { // get usuarios activos
            return await _context.Usuarios
                .Where(u => u.Estado)
                .ToListAsync();
        }
        public async Task<Usuario?> GetUsuarioByIdAsync(int id){ // get usuariio con una id en especifico
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id && u.Estado);
        }
        public async Task<Usuario?> GetUsuarioByEmailAsync(string email){ // get usuariio con un email en especifico y que este activo
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email && u.Estado);
        }
        public async Task<Usuario> CreateUsuarioAsync(Usuario usuario){ // crear usuario
            // Validar email
            if (await Validacion.EmailExistsAsync(_context, usuario.Email)) {
                throw new InvalidOperationException("El email ya está registrado");
            }

            usuario.Contrasena = PassworHash.HashPassword(usuario.Contrasena);
            usuario.Estado = true;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario; 
        }
        public async Task<Usuario?> UpdateUsuarioAsync(int id, Usuario usuario){ // actualizar usuario
            var existingUsuario = await _context.Usuarios.FindAsync(id);
            if (existingUsuario == null || !existingUsuario.Estado)
            {
                return null;
            }
            // Validar email si cambió
            if (existingUsuario.Email != usuario.Email && await Validacion.EmailExistsAsync(_context, usuario.Email)){
                throw new InvalidOperationException("El email ya está registrado");
            }
            existingUsuario.Email = usuario.Email;
            existingUsuario.Nombre = usuario.Nombre;
            existingUsuario.IdRol = usuario.IdRol;
            existingUsuario.IdBodega = usuario.IdBodega;
            if (!string.IsNullOrEmpty(usuario.Contrasena)){ // Solo actualiza contraseña si se proporciona una nueva
                existingUsuario.Contrasena = PassworHash.HashPassword(usuario.Contrasena); // encriptar contraseña
            }
            await _context.SaveChangesAsync();
            return await GetUsuarioByIdAsync(id);
        }
        public async Task<bool> DeleteUsuarioAsync(int id){ // eliminar usuario soft delete
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null){ // cambia estado a false
                return false;
            }
            usuario.Estado = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}