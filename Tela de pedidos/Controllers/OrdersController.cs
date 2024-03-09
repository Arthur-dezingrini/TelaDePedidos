using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Numerics;
using Tela_de_pedidos.Data;
using Tela_de_pedidos.Models;
using Tela_de_pedidos.Models.ViewModel;


namespace Tela_de_pedidos.Controllers
{
	public class OrdersController : Controller
	{
		private readonly ApplicationDbContext _db;


		public OrdersController(ApplicationDbContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			var orders = (from order in _db.Pedidos
						  join client in _db.Client on order.Client.Id equals client.Id
						  join carrier in _db.Carrier on order.Carrier.Id equals carrier.Id
						  join seller in _db.Seller on order.Seller.Id equals seller.Id
						  select new OrderViewModel
						  {
							  OrderId = order.Id,
							  ClientName = client.Name,
							  CarrierName = carrier.Name,
							  SellerName = seller.Name,
							  PaymentType = order.PaymentType,
							  TypeTransport = order.TypeTransport,
							  Date = order.Date,
							  Obs = order.Obs,
						  }).ToList();

			return View(orders);
		}
		[HttpGet]
		public IActionResult AddOrder()
		{
			ViewBag.Clientes = _db.Client.ToList();
			ViewBag.Transportadoras = _db.Carrier.ToList();
			ViewBag.Vendedores = _db.Seller.ToList();
			ViewBag.Produtos = _db.Products.ToList();
			return View();
		}

		[HttpGet]
		public IActionResult AddSeller()
		{
			return View();
		}

		[HttpGet]
		public IActionResult AddCarrier()
		{
			return View();
		}

		[HttpGet]
		public IActionResult AddClient()
		{
			return View();
		}

		[HttpGet]
		public IActionResult AddProduct()
		{
			return View();
		}

		[HttpGet]
		public IActionResult DashBoard()
		{
			var orderProducts = _db.ProductsOfOrder;
			double TotalSell = 0;
			foreach (var product in orderProducts)
			{
				TotalSell += product.TotalPrice();
			}

			var orderProduct = _db.ProductsOfOrder.Include(p => p.Product).ToList();

			var totalSalesByProduct = orderProduct.GroupBy(p => p.Product.Name)
												   .Select(g => new { ProductName = g.Key, TotalSales = g.Sum(p => p.TotalPrice()) })
												   .ToList();


			var ordersProducts = _db.ProductsOfOrder.Include(p => p.Order).ThenInclude(o => o.Client).ToList();

			var totalSalesByClient = ordersProducts.GroupBy(p => p.Order.Client.Name)
												  .Select(g => new { ClientName = g.Key, TotalSales = g.Sum(p => p.TotalPrice()) })
												  .ToList();

			var totalQuantityByProduct = _db.ProductsOfOrder.Include(p => p.Product)
															.GroupBy(p => p.Product.Name)
															.Select(g => new { ProductName = g.Key, TotalQuantity = g.Sum(p => p.Quantity) })
															.ToList();

			var totalQuantityByClient = _db.ProductsOfOrder.Include(p => p.Order).ThenInclude(o => o.Client)
														   .GroupBy(p => p.Order.Client.Name)
														   .Select(g => new { ClientName = g.Key, TotalQuantity = g.Sum(p => p.Quantity) })
														   .ToList();

			int totalItemsSold = _db.ProductsOfOrder.Sum(p => p.Quantity);


			ViewBag.TotalItemsSold = totalItemsSold;
			ViewBag.TotalQuantityByProduct = totalQuantityByProduct;
			ViewBag.TotalQuantityByClient = totalQuantityByClient;
			ViewBag.TotalSalesByClient = totalSalesByClient;
			ViewBag.TotalSalesByProduct = totalSalesByProduct;
			ViewBag.TotalSell = TotalSell;

			return View();
		}

		[HttpGet]
		public IActionResult AddProductToOrder(int orderId)
		{
			OrderModel order = _db.Pedidos.Find(orderId);

			double total = 0;
			foreach (var product in _db.ProductsOfOrder.Where(p => p.OrderId == orderId))
			{
				total += product.TotalPrice();
			}

			total += order.Frete - order.Descont;

			var orderProducts = _db.ProductsOfOrder
							.Include(p => p.Product)
							.Where(p => p.OrderId == orderId)
							.ToList();

			ViewBag.OrderProducts = orderProducts;
			ViewBag.OrderId = orderId;
			ViewBag.Produtos = _db.Products.ToList();
			ViewBag.Total = total;

			return View();
		}

		[HttpGet]
		public IActionResult Edit(int id)
		{
			ViewBag.Clientes = _db.Client.ToList();
			ViewBag.Transportadoras = _db.Carrier.ToList();
			ViewBag.Vendedores = _db.Seller.ToList();
			ViewBag.Produtos = _db.Products.ToList();
			if (id == 0 || id == null)
			{
				return NotFound();
			}

			OrderModel order = _db.Pedidos.FirstOrDefault(x => x.Id == id);

			if (order == null)
			{
				return NotFound();
			}

			return View(order);
		}

		[HttpPost]
		public IActionResult AddSeller(SellerModel seller)
		{
			if (ModelState.IsValid)
			{
				_db.Seller.Add(seller);
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult AddOrder(OrderModel order)
		{
			OrderModel orderModel = new OrderModel
			{
				Client = _db.Client.Find(order.Client.Id),
				Seller = _db.Seller.Find(order.Seller.Id),
				Carrier = _db.Carrier.Find(order.Carrier.Id),
				TypeTransport = order.TypeTransport,
				PaymentType = order.PaymentType,
				Descont = order.Descont,
				Frete = order.Frete,
				Date = order.Date,
				Obs = order.Obs,
			};

			_db.Pedidos.Add(orderModel);
			_db.SaveChanges();


			return RedirectToAction("Index");

		}

		[HttpPost]
		public IActionResult AddCarrier(CarrierModel carrier)
		{
			if (ModelState.IsValid)
			{
				_db.Carrier.Add(carrier);
				_db.SaveChanges();
			}

			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult AddClient(ClientModel client)
		{
			if (ModelState.IsValid)
			{
				_db.Client.Add(client);
				_db.SaveChanges();
			}

			return RedirectToAction("Index");
		}
		[HttpPost]
		public IActionResult AddProductToDb(ProductModel product)
		{
			if (ModelState.IsValid)
			{
				_db.Products.Add(product);
				_db.SaveChanges();
			}
			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult AddProductToOrder(int orderId, int productId, int quantity)
		{
			var order = _db.Pedidos.Include(o => o.OrderProducts).FirstOrDefault(o => o.Id == orderId);
			var product = _db.Products.Find(productId);

			if (order != null && product != null)
			{
				var existingProduct = _db.ProductsOfOrder.FirstOrDefault(p => p.ProductId == productId && p.OrderId == orderId);
				if (existingProduct != null)
				{
					existingProduct.Quantity += quantity;
				}
				else
				{
					var orderProduct = new OrderProductModel
					{
						Order = order,
						Product = product,
						Quantity = quantity,
						UnitPrice = product.Price,
					};

					_db.ProductsOfOrder.Add(orderProduct);
					order.OrderProducts.Add(orderProduct);
				}

				_db.SaveChanges();
			}

			double total = 0;
			foreach (var productOrder in _db.ProductsOfOrder.Where(p => p.OrderId == orderId))
			{
				total += productOrder.TotalPrice();
			}

			total += order.Frete - order.Descont;

			var orderProducts = _db.ProductsOfOrder
							 .Include(p => p.Product)
							 .Where(p => p.OrderId == orderId)
							 .ToList();

			ViewBag.OrderProducts = orderProducts;
			ViewBag.OrderId = orderId;
			ViewBag.Total = total;
			return RedirectToAction("AddProductToOrder", new { orderId = orderId });
		}

		[HttpPost]
		public IActionResult Excluir(int id)
		{
			OrderModel order = _db.Pedidos.FirstOrDefault(c => c.Id == id);

			if (order != null)
			{
				_db.Pedidos.Remove(order);
				_db.SaveChanges();

			}
			return RedirectToAction("Index");
		}


		[HttpPost]
		public IActionResult Edit(OrderModel order)
		{
			var existingOrder = _db.Pedidos.Find(order.Id);

			if (existingOrder != null)
			{
				existingOrder.Client = _db.Client.Find(order.Client.Id);
				existingOrder.Seller = _db.Seller.Find(order.Seller.Id);
				existingOrder.Carrier = _db.Carrier.Find(order.Carrier.Id);
				existingOrder.TypeTransport = order.TypeTransport;
				existingOrder.PaymentType = order.PaymentType;
				existingOrder.Date = order.Date;
				existingOrder.Obs = order.Obs;
				existingOrder.Frete = order.Frete;
				existingOrder.Descont = order.Descont;

				_db.Pedidos.Update(existingOrder);
				_db.SaveChanges();

			}

			return RedirectToAction("Index");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
