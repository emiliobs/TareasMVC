using System.ComponentModel.DataAnnotations;

namespace TareasMVC.Entidades
{
    public class Tareas
    {
        public int Id { get; set; }

        [StringLength(250)]
        [Required]
        public string Titulo { get; set; }

        public string Description { get; set; }

        public int Orden { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}
