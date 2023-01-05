using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TareasMVC.Data;
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
    }
}
