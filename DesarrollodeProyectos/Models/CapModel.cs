using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using DesarrollodeProyectos.Identity;

namespace DesarrollodeProyectos.Identity
{
    public class CapModel
    {
        public CapModel()
        {
            SizeList = new List<SelectListItem>();
            MaterialList = new List<SelectListItem>();
            CategoryList = new List<SelectListItem>();
        }

        public Guid Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Color")]
        public string Color { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Talla")]
        public Guid? SizeId { get; set; }

        public Size? Size { get; set; }
        public string? SizeName { get; set; }
        public List<SelectListItem> SizeList { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Precio")]
        [Range(1, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Material")]
        public Guid? MaterialId { get; set; }

        public Material? Material { get; set; }
        public string? MaterialName { get; set; }
        public List<SelectListItem> MaterialList { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Cantidad")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que 0")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Categoría")]
        public Guid? CategoryId { get; set; }

        public Category? Category { get; set; }
        public string? CategoryName { get; set; }
        public List<SelectListItem> CategoryList { get; set; }

        
        [Display(Name = "Imagen del producto")]
        public IFormFile? Image { get; set; }

        public string? ImageUrl { get; set; } 

        // Campo para marcar si el producto está activo o inactivo
        [Display(Name = "Activo")]
        public bool IsActive { get; set; } = true; 

        [Display(Name = "Fecha de creación")]
        public DateTime CreationTime { get; set; } = DateTime.Now;

    }
}
