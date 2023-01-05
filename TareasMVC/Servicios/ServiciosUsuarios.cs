using System.Security.Claims;

namespace TareasMVC.Servicios
{
    public class ServiciosUsuarios : IServiciosUsuarios
    {
        private HttpContext _HttpContext;

        public ServiciosUsuarios(IHttpContextAccessor httpContextAccessor)
        {
            _HttpContext = httpContextAccessor.HttpContext;
        }

        public string ObtenerUsuarioId()
        {
            if (_HttpContext.User.Identity.IsAuthenticated)
            {
                var idClaim = _HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();

                return idClaim.Value;
            }
            else
            {
                throw new Exception("El usuario no está autenticado.");
            }
        }
    }
}
