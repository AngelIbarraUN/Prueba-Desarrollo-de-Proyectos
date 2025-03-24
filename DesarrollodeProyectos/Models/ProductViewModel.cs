    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DesarrollodeProyectos.Models
{
   public class ProductViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string ProductType { get; set; }  // "Shirt", "Sweater", etc.
}

}