using Microsoft.EntityFrameworkCore;
using SupabaseApiDemo.Models;

namespace SupabaseApiDemo.Data{
    public class ApplicationDbContext : DbContext{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){
        }
        public DbSet<Usuario> Usuarios { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);
            // Configuración para Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Contrasena).IsRequired();
                entity.Property(e => e.Estado).HasDefaultValue(true);

                // Índice único para email
                entity.HasIndex(e => e.Email).IsUnique();
            });
        }
    }
}