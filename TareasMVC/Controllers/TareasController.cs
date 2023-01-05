using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TareasMVC.Data;
using TareasMVC.Entidades;
using TareasMVC.Servicios;

namespace TareasMVC.Controllers
{
    [Route("api/tareas")]
    [ApiController]
    public class TareasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiciosUsuarios _serviciosUsuarios;

        public TareasController(ApplicationDbContext context, IServiciosUsuarios serviciosUsuarios)
        {
            _context = context;
            _serviciosUsuarios = serviciosUsuarios;
        }

        [HttpGet]
        public  async Task<List<Tareas>> Get()
        {
            return await _context.Tareas.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Tareas>> Post([FromBody] string titulo)
        {
            var usuarioId = _serviciosUsuarios.ObtenerUsuarioId();

            var existeTareas = await _context.Tareas.AnyAsync(t => t.UsuarioCreacionId == usuarioId);

            var orderMayor = 0;
            if (existeTareas)
            {
                orderMayor = await _context.Tareas.Where(t => t.UsuarioCreacionId == usuarioId).Select(t => t.Orden).MaxAsync();
            }

            var  tarea = new Tareas
            {
                Titulo = titulo,
                UsuarioCreacionId = usuarioId,
                FechaCreacion = DateTime.UtcNow,
                Orden = orderMayor + 1, 
            };

            _context.Tareas.Add(tarea);
            await _context.SaveChangesAsync();
            
            return tarea;

        }

    }
}
