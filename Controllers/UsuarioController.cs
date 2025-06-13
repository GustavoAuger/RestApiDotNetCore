namespace SupabaseApiDemo.Controllers;

using Microsoft.AspNetCore.Mvc;
using SupabaseApiDemo.Services;



[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly UsuarioService _usuarioService;

    public UsuarioController(UsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var usuarios = _usuarioService.GetUsuarios();
        return Ok(usuarios);
    }
}
