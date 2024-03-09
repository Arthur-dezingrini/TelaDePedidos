using Tela_de_pedidos.Enums;

namespace Tela_de_pedidos.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
        public OriginProduct OriginProduct { get; set; }
        public List<OrderProductModel> OrderProducts { get; set; } = new List<OrderProductModel>();
    }
}
