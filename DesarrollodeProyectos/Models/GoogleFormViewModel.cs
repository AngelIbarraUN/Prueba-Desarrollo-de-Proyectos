using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DesarrollodeProyectos.Models
{
    public class GoogleFormViewModel
    {
        
        [Required(ErrorMessage = "El campo {0} es requerido")]

        [EmailAddress(ErrorMessage = "Ingrese un email válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El asunto es requerido.")]
        public string Asunto { get; set; }


        public string SuccessMessage { get; set; } 
    }
}