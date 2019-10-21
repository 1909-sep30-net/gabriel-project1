using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DoapSoap.DataAccess.Entities
{
    public partial class DoapSoapContext : DbContext
    {
        public DoapSoapContext()
        {
        }

        public DoapSoapContext(DbContextOptions<DoapSoapContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<InventoryItems> InventoryItems { get; set; }
        public virtual DbSet<Locations> Locations { get; set; }
        public virtual DbSet<OrderItems> OrderItems { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<SpiceLevels> SpiceLevels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=DoapSoapDb");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customers>(entity =>
            {
                entity.HasKey(e => e.CustomerId)
                    .HasName("PK__Customer__A4AE64B86AFA165B");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(10);
            });

            modelBuilder.Entity<InventoryItems>(entity =>
            {
                entity.HasKey(e => new { e.LocationId, e.ProductId })
                    .HasName("PK__Inventor__2CBE68198BE2D357");

                entity.ToTable("Inventory_Items");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.InventoryItems)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK__Inventory__Locat__21D600EE");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.InventoryItems)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__Inventory__Produ__22CA2527");
            });

            modelBuilder.Entity<Locations>(entity =>
            {
                entity.HasKey(e => e.LocationId)
                    .HasName("PK__Location__E7FEA4770D072407");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<OrderItems>(entity =>
            {
                entity.HasKey(e => e.OrderItemId)
                    .HasName("PK__Order_It__57ED06A18615D96B");

                entity.ToTable("Order_Items");

                entity.Property(e => e.OrderItemId).HasColumnName("OrderItemID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__Order_Ite__Order__1E05700A");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__Order_Ite__Produ__1EF99443");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK__Orders__C3905BAF2C31EE49");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.TimeConfirmed)
                    .HasColumnName("Time_Confirmed")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__Orders__Customer__1A34DF26");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK__Orders__Location__1B29035F");
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasKey(e => e.ProductId)
                    .HasName("PK__Products__B40CC6ED63FCFF17");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.SpiceId).HasColumnName("SpiceID");

                entity.HasOne(d => d.Spice)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SpiceId)
                    .HasConstraintName("FK__Products__SpiceI__1758727B");
            });

            modelBuilder.Entity<SpiceLevels>(entity =>
            {
                entity.HasKey(e => e.SpiceId)
                    .HasName("PK__Spice_Le__6D1D9D9BF69A2EDC");

                entity.ToTable("Spice_Levels");

                entity.HasIndex(e => e.Name)
                    .HasName("UQ__Spice_Le__737584F6CC1CC2F6")
                    .IsUnique();

                entity.Property(e => e.SpiceId).HasColumnName("SpiceID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
