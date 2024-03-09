using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using Tela_de_pedidos.Data;
using Tela_de_pedidos.Enums;

namespace Tela_de_pedidos.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Selecione o cliente")]
        public ClientModel? Client { get; set; }
        [Required(ErrorMessage = "Selecione o tipo de transporte")]
        public TypeTransport TypeTransport { get; set; }
        [Required(ErrorMessage = "Selecione a Transportadora")]
        public CarrierModel? Carrier { get; set; }
        [Required(ErrorMessage = "Selecione o vendedor")]
        public SellerModel? Seller { get; set; }
        [Required(ErrorMessage = "Selecione o tipo de pagamento")]
        public PaymentType PaymentType { get; set; }
        public List<OrderProductModel> OrderProducts { get; set; } =  new List<OrderProductModel>();
        [Required(ErrorMessage = "Selecione o valor do frete")]
        public double Frete { get; set; }
        [Required(ErrorMessage = "Selecione o valor do desconto")]
        public double Descont { get; set; }
        public string? Obs { get; set; }
        public DateTime Date { get; set; } = DateTime.Now.Date;
    }
}

