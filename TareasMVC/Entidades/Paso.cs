namespace TareasMVC.Entidades
{
    public class Paso
    {
        public Guid Id { get; set; }

        public int TareaId { get; set; }

        public bool Realizado { get; set; }

        public int Orden { get; set; }

        //propiedad de navegacion entre entidades
        public Tareas Tarea { get; set; }
        public string Descripcion { get; set; }
    }
}
