using Microsoft.EntityFrameworkCore;
using Tienda.Api.Models;

namespace Tienda.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<Producto> Productos => Set<Producto>();
        public DbSet<Pedido> Pedidos => Set<Pedido>();
        public DbSet<ItemPedido> ItemsPedido => Set<ItemPedido>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Producto
            builder.Entity<Producto>(e =>
            {
                e.Property(p => p.Nombre).IsRequired();
                e.Property(p => p.Precio)
                 .IsRequired()
                 .HasPrecision(18, 2);
                e.Property(p => p.Stock).IsRequired();

                // Relación con ItemsPedido
                e.HasMany(p => p.Items)
                 .WithOne(i => i.Producto)
                 .HasForeignKey(i => i.ProductoId)
                 .OnDelete(DeleteBehavior.Cascade); // al eliminar Producto, se eliminan ItemsPedido
            });

            // Pedido
            builder.Entity<Pedido>(e =>
            {
                e.Property(p => p.NombreCliente).IsRequired();
                e.Property(p => p.CreadoEn)
                 .HasDefaultValueSql("GETDATE()");

                e.HasMany(p => p.Items)
                 .WithOne(i => i.Pedido)
                 .HasForeignKey(i => i.PedidoId)
                 .OnDelete(DeleteBehavior.Cascade); // al eliminar Pedido, se eliminan ItemsPedido
            });

            // ItemPedido
            builder.Entity<ItemPedido>(e =>
            {
                e.Property(i => i.PrecioUnitario)
                 .HasPrecision(18, 2);
            });
        }
    }
}
