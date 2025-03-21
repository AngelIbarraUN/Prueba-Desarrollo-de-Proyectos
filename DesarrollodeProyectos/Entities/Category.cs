using System;
using System.Collections.Generic;

namespace DesarrollodeProyectos.Identity
{
    public class Category
    {
        public Category()
        {
            Name = string.Empty;
            Description = string.Empty;
            Shirts = new List<Shirt>();
            Caps = new List<Cap>();
            Sweaters = new List<Sweater>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        // Relación con los productos
        public List<Shirt> Shirts { get; set; }
        public List<Cap> Caps { get; set; }
        public List<Sweater> Sweaters { get; set; }
    }
}
