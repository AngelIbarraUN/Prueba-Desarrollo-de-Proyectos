using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DesarrollodeProyectos.Identity;
using DesarrollodeProyectos.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DesarrollodeProyectos.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = new ProductViewModel
            {
                Shirts = await _context.Shirts
                    .Where(s => s.IsActive)
                    .Select(s => new ShirtModel
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Color = s.Color,
                        Price = s.Price,
                        IsActive = s.IsActive,
                        ImageUrl = s.ImageUrl,
                        Quantity = s.Quantity
                    })
                    .ToListAsync(),

                Sweaters = await _context.Sweaters
                    .Where(s => s.IsActive)
                    .Select(s => new SweaterModel
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Color = s.Color,
                        Price = s.Price,
                        IsActive = s.IsActive,
                        ImageUrl = s.ImageUrl,
                        Quantity = s.Quantity
                    })
                    .ToListAsync(),

                Caps = await _context.Caps
                    .Where(c => c.IsActive)
                    .Select(c => new CapModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Color = c.Color,
                        Price = c.Price,
                        IsActive = c.IsActive,
                        ImageUrl = c.ImageUrl,
                        Quantity = c.Quantity
                    })
                    .ToListAsync()
            };

            return View("~/Views/Home/Products.cshtml", products);
        }

        [HttpPost]
public async Task<IActionResult> AddToCart(Guid productId, string productType, int quantity, decimal price)
{
    // Verificar si el usuario está autenticado
    if (!User.Identity.IsAuthenticated)
    {
        // Redirigir al login si no está autenticado
        return RedirectToAction("Login", "User");
    }

    // Si está logueado, redirigir a la acción AddToCart en el controlador Cart
    // **Mover esta redirección dentro del bloque 'if' para que solo ocurra si el usuario está autenticado**
    return RedirectToAction("AddToCart", "Cart", new { productId, productType, quantity, price });
}   

       [HttpGet]
public async Task<IActionResult> Search(string searchTerm)
{
    // Filtrar los productos según el término de búsqueda
    var products = new ProductViewModel
    {
        Shirts = await _context.Shirts
            .Where(s => s.IsActive && 
                (string.IsNullOrEmpty(searchTerm) || 
                 s.Name.Contains(searchTerm) || 
                 s.Color.Contains(searchTerm)))
            .Select(s => new ShirtModel
            {
                Id = s.Id,
                Name = s.Name,
                Color = s.Color,
                Price = s.Price,
                IsActive = s.IsActive,
                ImageUrl = s.ImageUrl,
                Quantity = s.Quantity
            })
            .ToListAsync(),

        Sweaters = await _context.Sweaters
            .Where(s => s.IsActive && 
                (string.IsNullOrEmpty(searchTerm) || 
                 s.Name.Contains(searchTerm) || 
                 s.Color.Contains(searchTerm)))
            .Select(s => new SweaterModel
            {
                Id = s.Id,
                Name = s.Name,
                Color = s.Color,
                Price = s.Price,
                IsActive = s.IsActive,
                ImageUrl = s.ImageUrl,
                Quantity = s.Quantity
            })
            .ToListAsync(),

        Caps = await _context.Caps
            .Where(c => c.IsActive && 
                (string.IsNullOrEmpty(searchTerm) || 
                 c.Name.Contains(searchTerm) || 
                 c.Color.Contains(searchTerm)))
            .Select(c => new CapModel
            {
                Id = c.Id,
                Name = c.Name,
                Color = c.Color,
                Price = c.Price,
                IsActive = c.IsActive,
                ImageUrl = c.ImageUrl,
                Quantity = c.Quantity
            })
            .ToListAsync()
    };

    return View("~/Views/Home/Products.cshtml", products);
}


    [HttpGet]
public IActionResult ClearFilter()
{
    return RedirectToAction("Search", new { searchTerm = "", color = "", category = "" });
}

        

    }
}