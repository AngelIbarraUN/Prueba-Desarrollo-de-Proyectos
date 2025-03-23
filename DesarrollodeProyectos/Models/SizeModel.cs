using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DesarrollodeProyectos.Identity
{
    public class SizeModel
    {
        public SizeModel()
        {
            ShirtList = new List<SelectListItem>();
            SweaterList = new List<SelectListItem>();
            CapList = new List<SelectListItem>();
        }

        public Guid Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Nombre de la Talla")]
        public string Name { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime CreationTime { get; set; } = DateTime.Now;

        [Display(Name = "Camisas")]
        public List<SelectListItem> ShirtList { get; set; }

        [Display(Name = "Suéteres")]
        public List<SelectListItem> SweaterList { get; set; }

        [Display(Name = "Gorras")]
        public List<SelectListItem> CapList { get; set; }

        public bool IsActive { get; set; } = true;

      
    }
}
