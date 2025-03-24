using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DesarrollodeProyectos.Models
{
    public class Cart
    {
        public Guid Id { get; set; }

    [Required(ErrorMessage = "El usuario es obligatorio.")]
    public string UserId { get; set; }  // Relación con el usuario (Identity Framework)

    public List<CartItem> CartItems { get; set; }  // Productos dentro del carrito
    }
}
