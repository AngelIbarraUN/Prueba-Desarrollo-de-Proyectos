using System;
using System.Collections.Generic;
using DesarrollodeProyectos.Models;

namespace DesarrollodeProyectos.Identity
{
    public class Shirt
    {
        public Shirt()
        {
            Name = string.Empty;
            Color = string.Empty;
            ImageUrl = string.Empty;
            CartItems = new List<CartItem>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        /*Este atributo sirve para marcar como inactivo un registro si se desea borrar*/
        public bool IsActive { get; set; } = true;

        public DateTime CreationTime { get; set; } = DateTime.Now;


        public Guid? SizeId { get; set; }

        public Size? Size { get; set; }

        public decimal Price { get; set; }

        public Guid? MaterialId { get; set; }

        public Material? Material { get; set; }

        public int Quantity { get; set; }

        public Guid? CategoryId { get; set; }

        public Category? Category { get; set; }

        public string? ImageUrl { get; set; }

        public List<CartItem> CartItems { get; set; }

    }
}
