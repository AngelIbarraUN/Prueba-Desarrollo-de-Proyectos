using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DesarrollodeProyectos.Identity
{
    public class CategoryModel
    {
        public CategoryModel()
        {
            ShirtList = new List<SelectListItem>();
            SweaterList = new List<SelectListItem>();
            CapList = new List<SelectListItem>();
        }

        public Guid Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        // Relacionado con los productos (camisas, suéteres, gorras) de esta categoría
        [Display(Name = "Camisas")]
        public List<SelectListItem> ShirtList { get; set; }

        [Display(Name = "Suéteres")]
        public List<SelectListItem> SweaterList { get; set; }

        [Display(Name = "Gorras")]
        public List<SelectListItem> CapList { get; set; }
    }
}
