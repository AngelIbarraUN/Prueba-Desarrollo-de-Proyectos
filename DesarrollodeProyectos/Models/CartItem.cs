using System;
using System.ComponentModel.DataAnnotations;
using DesarrollodeProyectos.Identity;

namespace DesarrollodeProyectos.Models
{
    public class CartItem
    {
        [Key]
        public Guid Id { get; set; }

        public string UserId { get; set; } // Relación con el usuario (Identity)

        [Required(ErrorMessage = "El tipo de producto es requerido")]
        public string ProductType { get; set; }  // "Shirt", "Cap", "Sweater"

        [Required(ErrorMessage = "El ID del producto es obligatorio")]
        public Guid ProductId { get; set; } // ID del producto (Shirt, Cap, Sweater)

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que 0")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0")]
        public decimal Price { get; set; }

        public decimal TotalPrice => Quantity * Price;

        
        public Shirt? Shirt { get; set; }

        public Cap? Cap { get; set; }

        public Sweater? Sweater { get; set; }

        // Relación con el carrito
        public Guid CartId { get; set; } // Clave foránea para el carrito

    
        public Cart Cart { get; set; } // Relación con la entidad Cart
    }
}
