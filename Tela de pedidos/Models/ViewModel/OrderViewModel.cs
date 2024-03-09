using Tela_de_pedidos.Enums;

namespace Tela_de_pedidos.Models.ViewModel
{
	public class OrderViewModel
	{
		public int OrderId { get; set; }
		public string ClientName { get; set; }
		public string CarrierName { get; set; }
		public string SellerName { get; set; }
        public double TotalPrice { get; set; }
        public PaymentType PaymentType { get; set; }
        public TypeTransport TypeTransport { get; set; }
		public DateTime Date { get; set; } 
        public string Obs { get; set; }
    }


}
