using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesarrollodeProyectos.Identity;


namespace DesarrollodeProyectos.Models
{
    public class UserListViewModel
    {
        
       public UserListViewModel()
       {
        ListadeRoles = new List<string>();  

       }
       
        public List<UserViewModel> UserList { get; set; }

        public string Message { get; set; }

         public List<string> ListadeRoles { get; set; }
    }
}