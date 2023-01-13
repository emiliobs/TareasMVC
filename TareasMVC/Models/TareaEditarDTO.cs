using System.ComponentModel.DataAnnotations;

namespace TareasMVC.Models
{
    public class TareaEditarDTO
    {
       
        [StringLength(250)]
        public required  string Titulo { get; set; }

        public string Descripcion{ get; set; }
    }
}
