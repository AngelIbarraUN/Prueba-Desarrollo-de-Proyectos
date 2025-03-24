using DesarrollodeProyectos.Identity;
using DesarrollodeProyectos.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Cap> Caps { get; set; } 
          public DbSet<Shirt> Shirts { get; set; } 
          public DbSet<Sweater> Sweaters { get; set; } 
          public DbSet<Material> Materials { get; set; }
          public DbSet<Category> Categories { get; set; }
          
          public DbSet<Size> Sizes { get; set; }

          public DbSet<Supplier> Suppliers { get; set; }

           public DbSet<Order> Orders { get; set; }

           public DbSet<OrderDetail> OrderDetails { get; set; }

            public DbSet<Cart> Carts { get; set; }  
            public DbSet<CartItem> CartItems { get; set; } 

          
}