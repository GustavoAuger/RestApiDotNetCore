using SupabaseApiDemo.Models;
using SupabaseApiDemo.Data;
using Microsoft.EntityFrameworkCore;

namespace SupabaseApiDemo.Services{
    public static class Validacion{
        public static async Task<bool> UsuarioExistsAsync(ApplicationDbContext context, int id){
            return await context.Usuarios
                .AnyAsync(u => u.Id == id && u.Estado);
        }
        public static async Task<bool> EmailExistsAsync(ApplicationDbContext context, string email){
            return await context.Usuarios
                .AnyAsync(u => u.Email == email && u.Estado);
        }
    }
}