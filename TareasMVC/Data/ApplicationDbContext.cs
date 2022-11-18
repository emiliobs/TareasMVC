using Microsoft.EntityFrameworkCore;
using TareasMVC.Entidades;

namespace TareasMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        //Aqqui la clase tareas la convertimos a una entodad por entityFramecore como una tabla de la base de datos:
        public DbSet<Tareas> Tareas { get; set; }
    }
}
