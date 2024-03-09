using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Numerics;
using Tela_de_pedidos.Data;

namespace Tela_de_pedidos.Models
{
    public class OrderProductModel
    {
        public int Id { get; set; }
        public OrderModel Order { get; set; }
        public int OrderId { get; set; }
        public ProductModel Product { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }


        public double TotalPrice()
        {
            return UnitPrice * Quantity;
        }


    }
}
