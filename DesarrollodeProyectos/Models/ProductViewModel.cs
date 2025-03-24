    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DesarrollodeProyectos.Identity;

namespace DesarrollodeProyectos.Models
{
    public class ProductViewModel
{
    public List<ShirtModel> Shirts { get; set; }
    public List<SweaterModel> Sweaters { get; set; }
    public List<CapModel> Caps { get; set; }

    }
}