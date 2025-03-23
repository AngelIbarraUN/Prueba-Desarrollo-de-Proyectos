using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using DesarrollodeProyectos.Identity;

namespace DesarrollodeProyectos.Identity
{
    public class SupplierModel
    {
        public SupplierModel()
        {
            MaterialList = new List<SelectListItem>(); // Lista de materiales asociados al proveedor
        }

        public Guid Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Nombre del proveedor")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Número de Teléfono")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime CreationTime { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Estado")]
        public bool IsActive { get; set; } = true; 

        [Display(Name = "Materiales del proveedor")]
        public List<SelectListItem> MaterialList { get; set; } 

        public List<Material>? Materials { get; set; }

        
    }
}
