using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DesarrollodeProyectos.Models
{
    public class OrderModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "La fecha de la orden es requerida")]
        [Display(Name = "Fecha de Orden")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public string UserId { get; set; }

        [Required(ErrorMessage = "El monto total es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto total debe ser mayor que 0")]
        public decimal TotalAmount { get; set; }

        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "El estado del pedido es requerido")]
        public OrderStatus Status { get; set; }

        public List<OrderDetailModel> OrderDetails { get; set; } = new List<OrderDetailModel>();
    }

    
}
