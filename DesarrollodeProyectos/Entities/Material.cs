using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace DesarrollodeProyectos.Identity
{
    public class Material
    {
        public Material()
        {
            Name = string.Empty;
            Description = string.Empty;
            Shirts = new List<Shirt>();
            Sweaters = new List<Sweater>();
            Caps = new List<Cap>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<Shirt> Shirts { get; set; }
        public List<Sweater> Sweaters { get; set; }
        public List<Cap> Caps { get; set; }
    }
}
