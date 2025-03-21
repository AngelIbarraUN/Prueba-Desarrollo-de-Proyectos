using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DesarrollodeProyectos.Identity;

namespace DesarrollodeProyectos.Identity
{
    public class Size
    {
        public Size()
        {
            Name = string.Empty;
            Shirts = new List<Shirt>();
            Sweaters = new List<Sweater>();
            Caps = new List<Cap>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<Shirt> Shirts { get; set; }
        public List<Sweater> Sweaters { get; set; }
        public List<Cap> Caps { get; set; }
    }
}
