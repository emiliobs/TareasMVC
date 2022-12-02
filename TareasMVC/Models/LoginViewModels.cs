﻿using System.ComponentModel.DataAnnotations;

namespace TareasMVC.Models
{
    public class LoginViewModels
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [EmailAddress(ErrorMessage = "El campo debe ser un correo electrónico válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Recuérdame")]
        public bool Recuerdame { get; set; }

        public decimal Pagos { get; set; }
    }
}