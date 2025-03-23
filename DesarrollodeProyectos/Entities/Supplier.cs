using System;
using System.Collections.Generic;

namespace DesarrollodeProyectos.Identity
{
    public class Supplier
    {
        public Supplier()
        {
            Name = string.Empty;
            PhoneNumber = string.Empty;
            Materials = new List<Material>(); 
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        /* Este atributo sirve para marcar como inactivo un registro si se desea borrar */
        public bool IsActive { get; set; } = true;

        public DateTime CreationTime { get; set; } = DateTime.Now;

        public List<Material> Materials { get; set; }
    }
}
