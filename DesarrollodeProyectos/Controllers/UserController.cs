using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DesarrollodeProyectos.Models;
using Microsoft.AspNetCore.Authentication;
using DesarrollodeProyectos.Services;
using Microsoft.EntityFrameworkCore;



namespace DesarrollodeProyectos.Controllers
{
    public class UserController : Controller

    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext context)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
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
            if (message is not null) {
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

        public IActionResult List(string msg=null)
        {
         var userList = _context.Users.Select(x => new UserViewModel
         {
        User = x.UserName, 
        Email = x.Email,   
        Confirmed = x.EmailConfirmed
         }).ToList();

         var userListViewModel = new UserListViewModel();

        userListViewModel.UserList = userList;
        userListViewModel.Message=msg;
        return View(userListViewModel);
        
        }
         [HttpPost]
        public  async Task<IActionResult> HacerAdmin(string email)
        {
            var usuario = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario is null)
            {
                return NotFound();
            }

            await _userManager.AddToRoleAsync(usuario, Constantss.RolAdmin);

            return RedirectToAction("List", 
                routeValues: new { msg = "Rol asignado correctamente a " + email });
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

            return RedirectToAction("List",
                routeValues: new { msg = "Rol removido correctamente a " + email });
        }


    }
}