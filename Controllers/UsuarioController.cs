using Microsoft.AspNetCore.Mvc;
using SupabaseApiDemo.Models;
using SupabaseApiDemo.Services;
using SupabaseApiDemo.Utils;
using SupabaseApiDemo.Data;

namespace SupabaseApiDemo.Controllers{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase{
        private readonly IUsuarioService _usuarioService;
        private readonly ApplicationDbContext _context;

        public UsuarioController(IUsuarioService usuarioService, ApplicationDbContext context){
            _usuarioService = usuarioService;
            _context = context;
        }
        [HttpGet] // Obtener todos los usuarios
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios(){
            var usuarios = await _usuarioService.GetAllUsuariosAsync();
            return Ok(usuarios);
        }
        [HttpGet("{id}")] // Obtener un usuario por ID
        public async Task<ActionResult<Usuario>> GetUsuario(int id){
            var usuario = await _usuarioService.GetUsuarioByIdAsync(id);
            return usuario == null 
                ? NotFound(new { message = "Usuario no encontrado" })
                : Ok(usuario);
        }
        [HttpGet("email/{email}")] // Obtener un usuario por Email (activo)
        public async Task<ActionResult<Usuario>> GetUsuarioByEmail(string email){
            var usuario = await _usuarioService.GetUsuarioByEmailAsync(email);
            return usuario == null 
                ? NotFound(new { message = "Usuario no encontrado" })
                : Ok(usuario);
        }
        [HttpPost] // Crear un nuevo usuario
        public async Task<ActionResult> CreateUsuario([FromBody] Usuario usuario){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try {
                var nuevoUsuario = await _usuarioService.CreateUsuarioAsync(usuario);
                return Ok(new {
                    message = "Usuario creado correctamente",
                    usuario = nuevoUsuario
                });
            } catch (InvalidOperationException ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")] // Actualizar un usuario por ID
        public async Task<ActionResult<Usuario>> UpdateUsuario(int id, [FromBody] Usuario usuario){ 
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try{
                var usuarioActualizado = await _usuarioService.UpdateUsuarioAsync(id, usuario);
                return usuarioActualizado == null
                    ? NotFound(new { message = "Usuario no encontrado" })
                    : Ok(usuarioActualizado);
            }
            catch (InvalidOperationException ex){
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpDelete("{id}")] // Eliminar un usuario por ID soft
        public async Task<ActionResult> DeleteUsuario(int id){ 
            var resultado = await _usuarioService.DeleteUsuarioAsync(id);
            return resultado
                ? Ok(new { message = "Usuario eliminado exitosamente" })
                : NotFound(new { message = "Usuario no encontrado" });
        }
    }
}