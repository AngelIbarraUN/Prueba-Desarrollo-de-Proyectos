using System;
using System.ComponentModel.DataAnnotations;

namespace DesarrollodeProyectos.Models
{
    public class OrderDetailModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    public Guid OrderId { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    public Guid ProductId { get; set; }

    [Required(ErrorMessage = "El tipo de producto es requerido")]
    [Display(Name = "Tipo de Producto")]
    public string ProductType { get; set; }  // "Shirt", "Cap" o "Sweater"

    [Required(ErrorMessage = "La cantidad es requerida")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que 0")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "El precio es requerido")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0")]
    public decimal Price { get; set; }

    public decimal TotalPrice => Quantity * Price;
    public string ProductName { get; set; }
    }

}
