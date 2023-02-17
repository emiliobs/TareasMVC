﻿using Microsoft.AspNetCore.Mvc;
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
            _context = context;
            _serviciosUsuarios = serviciosUsuarios;
        }

        [HttpPost("{tareaId:int}")]
        public async Task<ActionResult<Paso>> Post(int tareaId, [FromBody] PasoCrearDTO pasoCrearDTO)
        {
            string UsuarioId = _serviciosUsuarios.ObtenerUsuarioId();

            Tareas tarea = await _context.Tareas.FirstOrDefaultAsync(t => t.Id == tareaId);

            if (tarea is null)
            {
                return NotFound();
            }

            if (tarea.UsuarioCreacionId != UsuarioId)
            {
                return Forbid();
            }

            bool existenPasos = await _context.Pasos.AnyAsync(p => p.TareaId == tareaId);

            int ordenMayor = 0;
            if (existenPasos)
            {
                ordenMayor = await _context.Pasos.Where(p => p.TareaId == tareaId).Select(p => p.Orden).MaxAsync();
            }



            //Aqui creo un nuevo paso:
            Paso paso = new()
            {
                TareaId = tareaId,
                Orden = ordenMayor,
                Descripcion = pasoCrearDTO.Descripcion,
                Realizado = pasoCrearDTO.Realizado,
            };

            _ = _context.Add(paso);
            _ = await _context.SaveChangesAsync();

            return paso;

        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] PasoCrearDTO pasoCrearDTO)
        {
            string usuarioId = _serviciosUsuarios.ObtenerUsuarioId();

            Paso paso = await _context.Pasos.Include(p => p.Tarea).FirstOrDefaultAsync(p => p.Id == id);

            if (paso is null)
            {
                return NotFound();
            }

            if (paso.Tarea.UsuarioCreacionId != usuarioId)
            {
                return Forbid();
            }

            paso.Descripcion = pasoCrearDTO?.Descripcion;
            paso.Realizado = pasoCrearDTO.Realizado;

            _ = _context.SaveChanges();

            return Ok();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id, Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Paso> _)
        {

            string usuarioId = _serviciosUsuarios.ObtenerUsuarioId();

            Paso paso = await _context.Pasos.Include(p => p.Tarea).FirstOrDefaultAsync(t => t.Id == id);

            if (paso is null)
            {
                return NotFound();
            }

            if (paso.Tarea.UsuarioCreacionId != usuarioId)
            {
                return Forbid();
            }

             _context.Remove(paso);
             await _context.SaveChangesAsync();

            return Ok();
        }

    }
}