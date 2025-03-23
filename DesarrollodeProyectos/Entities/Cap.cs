using System;
using System.Collections.Generic;

namespace DesarrollodeProyectos.Identity
{
    public class Cap
    {
        public Cap()
        {
            Name = string.Empty;
            Color = string.Empty;
            ImageUrl = string.Empty;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        public Guid? SizeId { get; set; }

        public Size? Size { get; set; }

        public decimal Price { get; set; }

        public Guid? MaterialId { get; set; }

        public Material? Material { get; set; }

        public int Quantity { get; set; }

        public Guid? CategoryId { get; set; }

        public Category? Category { get; set; }

        public string? ImageUrl { get; set; }

        /*Este atributo sirve para marcar como inactivo un registro si se desea borrar*/
        public bool IsActive { get; set; } = true;

        public DateTime CreationTime { get; set; } = DateTime.Now;

    }
}
