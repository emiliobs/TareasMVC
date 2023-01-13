using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TareasMVC.Entidades
{
    public class Tareas
    {
        public int Id { get; set; }

        [StringLength(250)]
        [Required]
        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        public int Orden { get; set; }

        public DateTime FechaCreacion { get; set; }

        //propiedad de navegación de uno a muchos
        public List<Paso> Pasos { get; set; }

        public List<ArchivoAdjunto> ArchivoAdjuntos { get; set; }

        //Relacion con la tabla Usuarios:
        public IdentityUser UsuarioCreacion { get; set; }
        public string UsuarioCreacionId { get; set; }

    }
}
