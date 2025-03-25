using System;
using DesarrollodeProyectos.Identity;

namespace DesarrollodeProyectos.Models
{
    public class OrderDetail
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }

        public string ProductType { get; set; }  // "Shirt", "Cap" o "Sweater"

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal TotalPrice => Quantity * Price;

        public Order Order { get; set; }

        public Shirt? Shirt { get; set; }
        public Cap? Cap { get; set; }
        public Sweater? Sweater { get; set; }
        public string? ProductName { get; internal set; }

        public string ImageUrl { get; set; }
    }
}
