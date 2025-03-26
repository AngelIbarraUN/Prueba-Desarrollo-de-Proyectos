using DesarrollodeProyectos.Models;
using DesarrollodeProyectos.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DesarrollodeProyectos.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
            this._context = context;
        }

        [AllowAnonymous]
        public IActionResult Registry()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registry(RegistryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new IdentityUser()
            {
                Email = model.Email,
                UserName = model.Email,
            };

            var result = await _userManager.CreateAsync(user, password: model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: true);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }
        }

        [AllowAnonymous]
        public IActionResult Login(string message = null)
        {
            if (message is not null)
            {
                ViewData["message"] = message;
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var resultado = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.Remember, lockoutOnFailure: false);
            if (resultado.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "El Nombre de usuario o password son incorrectos");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> List(string msg = null)
        {
            var users = await _userManager.Users.ToListAsync();

            var userList = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new UserViewModel
                {
                    User = user.UserName,
                    Email = user.Email,
                    Confirmed = user.EmailConfirmed,
                    Roles = roles.ToList()
                });
            }

            var allRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

            var userListViewModel = new UserListViewModel
            {
                UserList = userList,
                ListadeRoles = allRoles,
                Message = msg
            };

            return View(userListViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> HacerAdmin(string email)
        {
            var usuario = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario is null)
            {
                return NotFound();
            }

            await _userManager.AddToRoleAsync(usuario, Constantss.RolAdmin);

            return RedirectToAction("List", new { msg = "Rol asignado correctamente a " + email });
        }

        [HttpPost]
        public async Task<IActionResult> RemoverAdmin(string email)
        {
            var usuario = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario is null)
            {
                return NotFound();
            }

            await _userManager.RemoveFromRoleAsync(usuario, Constantss.RolAdmin);

            return RedirectToAction("List", new { msg = "Rol removido correctamente a " + email });
        }

        [HttpPost]
        public async Task<IActionResult> AsignarRol(string email, string roleToAssign)
        {
            var usuario = await _userManager.FindByEmailAsync(email);

            if (usuario == null)
            {
                return NotFound();
            }

            var result = await _userManager.AddToRoleAsync(usuario, roleToAssign);

            if (result.Succeeded)
            {
                return RedirectToAction("List", new { msg = "Rol asignado correctamente." });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return RedirectToAction("List");
        }

        [HttpPost]
public async Task<IActionResult> QuitarRol(string email, string rol)
{
    var user = await _userManager.FindByEmailAsync(email);
    if (user == null)
    {
        return NotFound();
    }

    // Verificar si se intenta eliminar el rol "Admin"
    if (rol == "ADMIN")
    {
        var admins = await _userManager.GetUsersInRoleAsync("ADMIN");

        // Si solo hay un administrador, no permitir la eliminación
        if (admins.Count == 1)
        {
            TempData["ErrorMessage"] = "⚠️No puedes eliminar el último administrador del sistema.";
            return RedirectToAction("List");
        }
    }

    var result = await _userManager.RemoveFromRoleAsync(user, rol);
    if (result.Succeeded)
    {
        TempData["Message"] = $"Rol {rol} eliminado correctamente.";
    }
    else
    {
        TempData["Message"] = "Hubo un problema al eliminar el rol.";
    }

    return RedirectToAction("List");
}
         
    } 
}
