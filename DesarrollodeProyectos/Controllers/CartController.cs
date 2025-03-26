using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DesarrollodeProyectos.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DesarrollodeProyectos.Identity;
using System.Collections.Generic;

public class CartController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public CartController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(Guid productId, string productType, int quantity, decimal price)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return RedirectToAction("Login", "User");
        }

        if (quantity < 1)
        {
            return BadRequest("La cantidad debe ser al menos 1.");
        }

        var cart = await _context.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            cart = new Cart { UserId = userId, CartItems = new List<CartItem>() };
            _context.Carts.Add(cart);
        }

        if (cart.CartItems == null)
        {
            cart.CartItems = new List<CartItem>();
        }

        var cartItem = cart.CartItems.FirstOrDefault(item => item.ProductId == productId && item.ProductType == productType);
        if (cartItem != null)
        {
            cartItem.Quantity += quantity;
        }
        else
        {
            cart.CartItems.Add(new CartItem
            {
                ProductId = productId,
                ProductType = productType,
                Quantity = quantity,
                Price = price,
                UserId = userId,
                CartId = cart.Id
            });
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized();
        }

        var cart = await _context.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            return View("~/Views/Home/Cart.cshtml", new Cart { CartItems = new List<CartItem>() });
        }

        // Carga de productos relacionados directamente desde la base de datos
        foreach (var item in cart.CartItems)
        {
            if (item.ProductType == "Shirt")
            {
                item.Shirt = await _context.Shirts.FindAsync(item.ProductId);
            }
            else if (item.ProductType == "Cap")
            {
                item.Cap = await _context.Caps.FindAsync(item.ProductId);
            }
            else if (item.ProductType == "Sweater")
            {
                item.Sweater = await _context.Sweaters.FindAsync(item.ProductId);
            }
        }

        return View("~/Views/Home/Cart.cshtml", cart);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateCartItem(Guid cartItemId, int quantity)
    {
        var cartItem = await _context.CartItems.FindAsync(cartItemId);
        if (cartItem == null)
        {
            return NotFound();
        }

        if (quantity < 1)
        {
            return BadRequest("La cantidad debe ser al menos 1.");
        }

        cartItem.Quantity = quantity;
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveCartItem(Guid cartItemId)
    {
        var cartItem = await _context.CartItems.FindAsync(cartItemId);
        if (cartItem == null)
        {
            return NotFound();
        }

        _context.CartItems.Remove(cartItem);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    [HttpPost]
public async Task<IActionResult> Checkout()
{
    var userId = _userManager.GetUserId(User);
    if (userId == null)
    {
        return Unauthorized("Usuario no autenticado.");
    }

    var cart = await _context.Carts
        .Include(c => c.CartItems)
        .FirstOrDefaultAsync(c => c.UserId == userId);

    if (cart == null || !cart.CartItems.Any())
    {
        return BadRequest("El carrito está vacío.");
    }

    var user = await _userManager.FindByIdAsync(userId);
    if (user == null)
    {
        return NotFound("Usuario no encontrado.");
    }

    var orderDetails = new List<OrderDetail>();
    foreach (var item in cart.CartItems)
    {
        string imageUrl = null;
        switch (item.ProductType)
        {
            case "Shirt":
                var shirt = await _context.Shirts.FindAsync(item.ProductId);
                if (shirt != null) imageUrl = shirt.ImageUrl;
                break;
            case "Cap":
                var cap = await _context.Caps.FindAsync(item.ProductId);
                if (cap != null) imageUrl = cap.ImageUrl;
                break;
            case "Sweater":
                var sweater = await _context.Sweaters.FindAsync(item.ProductId);
                if (sweater != null) imageUrl = sweater.ImageUrl;
                break;
            default:
                // Manejo de tipo de producto desconocido (opcional)
                break;
        }

        orderDetails.Add(new OrderDetail
        {
            ProductId = item.ProductId,
            ProductType = item.ProductType,
            Quantity = item.Quantity,
            Price = item.Price,
            ImageUrl = imageUrl
        });
    }

    var order = new Order
    {
        UserId = userId,
        UserEmail = user.Email,
        OrderDate = DateTime.Now,
        TotalAmount = cart.CartItems.Sum(item => item.TotalPrice),
        Status = OrderStatus.Pendiente,
        OrderDetails = orderDetails
    };

    foreach (var item in cart.CartItems)
    {
        switch (item.ProductType)
        {
            case "Shirt":
                var shirt = await _context.Shirts.FindAsync(item.ProductId);
                if (shirt != null) shirt.Quantity -= item.Quantity;
                break;
            case "Cap":
                var cap = await _context.Caps.FindAsync(item.ProductId);
                if (cap != null) cap.Quantity -= item.Quantity;
                break;
            case "Sweater":
                var sweater = await _context.Sweaters.FindAsync(item.ProductId);
                if (sweater != null) sweater.Quantity -= item.Quantity;
                break;
        }
    }

    using (var transaction = _context.Database.BeginTransaction())
    {
        try
        {
            _context.Orders.Add(order);
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            // Registra el error (opcional)
            Console.WriteLine($"Error al procesar el pedido: {ex.Message}");
            return StatusCode(500, "Error al procesar el pedido.");
        }
    }

    return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
}

    public IActionResult OrderConfirmation(Guid orderId)
    {
        ViewBag.OrderId = orderId;
        return View("~/Views/Home/OrderConfirmation.cshtml");
    }
}