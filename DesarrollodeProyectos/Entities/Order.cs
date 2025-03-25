using System;
using System.Collections.Generic;

namespace DesarrollodeProyectos.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        public string UserId { get; set; } // Relación con la tabla de usuarios (Identity)

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public decimal TotalAmount { get; set; }

         public string UserEmail { get; set; }

        public bool IsActive { get; set; } = true;

        public OrderStatus Status { get; set; }

        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }

    public enum OrderStatus
    {
        Pendiente,
        Procesando,
        Enviado,
        Entregado,
        Cancelado
    }
}
