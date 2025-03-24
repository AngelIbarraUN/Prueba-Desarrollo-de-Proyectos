using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesarrollodeProyectos.Identity;


namespace DesarrollodeProyectos.Models
{
    public class UserViewModel
    {
    
    public UserViewModel()
    {

    }

    public string User { get; set; }
            
    public string Email { get; set;}

    public bool Confirmed { get; set; }

    public IList<string> Roles { get; set; }

    }
}