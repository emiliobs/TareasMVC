using Microsoft.EntityFrameworkCore;
using TareasMVC.Entidades;

namespace TareasMVC
{
    public class ArchivoAdjunto
    {
        public Guid Id { get; set; }

        [Unicode]
        public string Url { get; set; }

        public string Titulo { get; set; }

        public int Orden { get; set; }

        public DateTime FechaCreacion { get; set; }

        //Relacion de entidades de muchos a uno
        public int TareaId { get; set; }

        public Tareas Tarea { get; set; }
    }
}
