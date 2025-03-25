using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DesarrollodeProyectos.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

public class OrderController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public OrderController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> History()
    {
        if (User.IsInRole("ADMIN"))
        {
            // Mostrar todos los pedidos para administradores
            var allOrders = await _context.Orders
                .Include(o => o.OrderDetails)
                .ToListAsync();

            return View("~/Views/Home/History.cshtml", allOrders);
        }
        else
        {
            // Mostrar solo los pedidos del usuario actual
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized();
            }

            var userOrders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderDetails)
                .ToListAsync();

            return View("~/Views/Home/History.cshtml", userOrders);
        }
    }

    public async Task<IActionResult> Details(Guid orderId)
    {
        var order = await _context.Orders
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
        {
            return NotFound();
        }

        if (!User.IsInRole("ADMIN") && order.UserId != _userManager.GetUserId(User))
        {
            return Unauthorized();
        }

        // Cargar los nombres de los productos y las URLs de las imágenes
        foreach (var detail in order.OrderDetails)
        {
            if (detail.ProductType == "Shirt")
            {
                var shirt = await _context.Shirts.FindAsync(detail.ProductId);
                detail.ProductName = shirt?.Name;
                detail.ImageUrl = shirt?.ImageUrl; // Asigna la URL de la imagen
            }
            else if (detail.ProductType == "Sweater")
            {
                var sweater = await _context.Sweaters.FindAsync(detail.ProductId);
                detail.ProductName = sweater?.Name;
                detail.ImageUrl = sweater?.ImageUrl; // Asigna la URL de la imagen
            }
            else if (detail.ProductType == "Cap")
            {
                var cap = await _context.Caps.FindAsync(detail.ProductId);
                detail.ProductName = cap?.Name;
                detail.ImageUrl = cap?.ImageUrl; // Asigna la URL de la imagen
            }
        }

        // Obtener el correo electrónico del usuario
        var user = await _userManager.FindByIdAsync(order.UserId);
        if (user != null)
        {
            order.UserEmail = user.Email;
        }

        return View("~/Views/Home/Details.cshtml", order);
    }

    public async Task<IActionResult> MyOrders()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized();
        }

        var userOrders = await _context.Orders
            .Where(o => o.UserId == userId)
            .ToListAsync();

        return View("~/Views/Home/MyOrders.cshtml", userOrders);
    }

    [HttpPost]
    public async Task<IActionResult> CancelOrder(Guid orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
        {
            return NotFound();
        }

        if (order.UserId != _userManager.GetUserId(User))
        {
            return Unauthorized();
        }

        // Permitir la cancelación solo si el pedido está en estado "Pendiente"
        if (order.Status != OrderStatus.Pendiente)
        {
            return BadRequest("El pedido solo se puede cancelar si está en estado pendiente.");
        }

        order.Status = OrderStatus.Cancelado;
        await _context.SaveChangesAsync();

        return RedirectToAction("Details", new { orderId = orderId });
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> EditStatus(Guid orderId, OrderStatus newStatus)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
        {
            return NotFound();
        }

        order.Status = newStatus;
        await _context.SaveChangesAsync();

        return RedirectToAction("History");
    }
}