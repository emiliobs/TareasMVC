using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TareasMVC.Entidades;

namespace TareasMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        //Aqui la clase tareas la convertimos a una entodad por entityFramecore como una tabla de la base de datos:
        public DbSet<Tareas> Tareas { get; set; }

        public DbSet<Paso> Pasos { get; set; }

        public DbSet<ArchivoAdjunto> ArchivoAdjuntos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Tareas>().Property(t => t.Titulo).HasMaxLength(250).IsRequired();
        }
    }
}
