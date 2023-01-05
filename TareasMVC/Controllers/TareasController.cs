using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TareasMVC.Data;
using TareasMVC.Entidades;
using TareasMVC.Models;
using TareasMVC.Servicios;

namespace TareasMVC.Controllers
{
    [Route("api/tareas")]
    [ApiController]
    public class TareasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiciosUsuarios _serviciosUsuarios;
        private readonly IMapper _mapper;

        public TareasController(ApplicationDbContext context, IServiciosUsuarios serviciosUsuarios, IMapper mapper)
        {
            _context = context;
            _serviciosUsuarios = serviciosUsuarios;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<List<TareaDTO>> Get()
        {
            var usuarioId = _serviciosUsuarios.ObtenerUsuarioId();

            var tareas = await _context.Tareas
                                       .Where(t => t.UsuarioCreacionId == usuarioId)
                                       .OrderBy(t => t.Orden)
                                       .ProjectTo<TareaDTO>(_mapper.ConfigurationProvider).ToListAsync();

            //var tareas = await _context.Tareas
            //                     .Where(t => t.UsuarioCreacionId.Equals(usuarioId))
            //                     .OrderBy(t => t.Orden)
            //                     .Select(t => new
            //                     {
            //                         t.Id,
            //                         t.Titulo,
            //                     }).ToListAsync();

            //return Ok(tareas);

            return tareas;
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

        var tarea = new Tareas
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
