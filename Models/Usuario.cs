using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupabaseApiDemo.Models
{
    [Table("usuario")]
    public class Usuario
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [Column("id_rol")]
        public int IdRol { get; set; }

        [Required]
        [Column("contrasena")]
        public string Contrasena { get; set; } = string.Empty;

        [Column("id_bodega")]
        public int? IdBodega { get; set; }

        [Column("estado")]
        public bool Estado { get; set; } = true;
    }
}