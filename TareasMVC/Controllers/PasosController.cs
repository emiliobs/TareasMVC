using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TareasMVC.Data;
using TareasMVC.Entidades;
using TareasMVC.Models;
using TareasMVC.Servicios;

namespace TareasMVC.Controllers
{
    [Route("api/pasos")]
    public class PasosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiciosUsuarios _serviciosUsuarios;

        public PasosController(ApplicationDbContext context, IServiciosUsuarios serviciosUsuarios)
        {
            this._context = context;
            this._serviciosUsuarios = serviciosUsuarios;
        }

        [HttpPost("{tareaId:int}")]
        public async Task<ActionResult<Paso>> Post(int tareaId, [FromBody] PasoCrearDTO pasoCrearDTO)
        {
            var UsuarioId = _serviciosUsuarios.ObtenerUsuarioId();

            var tarea = await _context.Tareas.FirstOrDefaultAsync(t => t.Id == tareaId);

            if (tarea is null)
            {
                return NotFound();  
            }

            if (tarea.UsuarioCreacionId != UsuarioId)
            {
                return Forbid();
            }

            var existenPasos = await _context.Pasos.AnyAsync(p => p.TareaId == tareaId);

            var ordenMayor = 0;
            if (existenPasos)
            {
                ordenMayor = await _context.Pasos.Where(p => p.TareaId == tareaId).Select(p => p.Orden).MaxAsync();
            }



            //Aqui creo un nuevo paso:
            var paso = new Paso 
            {
                TareaId = tareaId,                 
                Orden = ordenMayor,
                Descripcion = pasoCrearDTO.Descripcion,
                Realizado = pasoCrearDTO.Realizado,
            };

            _context.Add(paso);
            await _context.SaveChangesAsync();
          
            return paso;





        }
    }
}
