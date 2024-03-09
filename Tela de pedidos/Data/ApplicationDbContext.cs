using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Reflection.Metadata.Ecma335;
using Tela_de_pedidos.Models;

namespace Tela_de_pedidos.Data;
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        }

    public DbSet<OrderModel> Pedidos { get; set; }
	public DbSet<ClientModel> Client { get; set; }
    public DbSet<SellerModel> Seller {  get; set; }
    public DbSet<CarrierModel> Carrier { get; set; }
    public DbSet<ProductModel> Products { get; set; }
    public DbSet<OrderProductModel> ProductsOfOrder { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderProductModel>()
            .HasKey(op => new { op.OrderId, op.ProductId });

        modelBuilder.Entity<OrderProductModel>()
            .HasOne(op => op.Order)
            .WithMany(o => o.OrderProducts)
            .HasForeignKey(op => op.OrderId);

        modelBuilder.Entity<OrderProductModel>()
            .HasOne(op => op.Product)
            .WithMany(p => p.OrderProducts)
            .HasForeignKey(op => op.ProductId);
    }

}

