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
        public async Task<ActionResult<List<TareaDTO>>> Get()
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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Tareas>> Get(int id)
        {
            var usuarioId = _serviciosUsuarios.ObtenerUsuarioId();

            var tarea = await _context.Tareas.FirstOrDefaultAsync(t => t.Id == id && t.UsuarioCreacionId == usuarioId);

            if (tarea is null)
            {
                return NotFound();
            }

            return tarea;
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

        [HttpPut("{id:int}")]
        public async Task<IActionResult> EditarTarea(int id, [FromBody] TareaEditarDTO tareaEditarDTO)
        {
            var usuarioId = _serviciosUsuarios.ObtenerUsuarioId();

            var tarea = await  _context.Tareas.FirstOrDefaultAsync(t => t.Id == id && t.UsuarioCreacionId == usuarioId);

            if (tarea is null)
            {
                return NotFound();
            }

            tarea.Titulo = tareaEditarDTO.Titulo;
            tarea.Descripcion = tareaEditarDTO.Descripcion;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("ordenar")]
        public async Task<IActionResult> Ordenar([FromBody] int[] ids)
        {
            var usuarioId = _serviciosUsuarios.ObtenerUsuarioId();

            var tareas = await _context.Tareas.Where(t => t.UsuarioCreacionId == usuarioId).ToListAsync();

            var tareasId = tareas.Select(t => t.Id);

            var idsTareasNoPertenecenAlusuario = ids.Except(tareasId).ToList();

            if (idsTareasNoPertenecenAlusuario.Any())
            {
                return Forbid();
            }

            var tareasDiccionario = tareas.ToDictionary(x => x.Id);

            for (int i = 0; i < ids.Length; i++)
            {
                var id = ids[i];
                var tarea = tareasDiccionario[id];
                tarea.Orden = i + 1;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
