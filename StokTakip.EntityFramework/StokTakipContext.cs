using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using StokTakip.Model;

#nullable disable

namespace StokTakip.EntityFramework
{
    public partial class StokTakipContext : DbContext
    {
        public StokTakipContext()
        {
        }

        public StokTakipContext(DbContextOptions<StokTakipContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CampaigUsedHistory> CampaigUsedHistories { get; set; }
        public virtual DbSet<Campaign> Campaigns { get; set; }
        public virtual DbSet<CampaignRequirement> CampaignRequirements { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CurrencyType> CurrencyTypes { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<DiscountCode> DiscountCodes { get; set; }
        public virtual DbSet<DiscountCodeRequirement> DiscountCodeRequirements { get; set; }
        public virtual DbSet<DiscountCodeUsedHistory> DiscountCodeUsedHistories { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<GiftCard> GiftCards { get; set; }
        public virtual DbSet<GiftCardHistory> GiftCardHistories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductAttribute> ProductAttributes { get; set; }
        public virtual DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }
        public virtual DbSet<ProductPrice> ProductPrices { get; set; }
        public virtual DbSet<SalesOrder> SalesOrders { get; set; }
        public virtual DbSet<SalesOrderDetail> SalesOrderDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(local); Initial Catalog=StokTakip; user id=sa; password=2430155a");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<CampaigUsedHistory>(entity =>
            {
                entity.HasKey(e => new { e.CampaignId, e.SalesOrderId })
                    .HasName("pk_CampaigUsedHistory");

                entity.ToTable("CampaigUsedHistory");

                entity.Property(e => e.UsedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.CampaigUsedHistories)
                    .HasForeignKey(d => d.CampaignId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_CampaigUsedHistory_Campaign");

                entity.HasOne(d => d.SalesOrder)
                    .WithMany(p => p.CampaigUsedHistories)
                    .HasForeignKey(d => d.SalesOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_CampaigUsedHistory_SalesOrder");
            });

            modelBuilder.Entity<Campaign>(entity =>
            {
                entity.ToTable("Campaign");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Value).HasColumnType("decimal(18, 5)");

                entity.HasOne(d => d.CurrencyType)
                    .WithMany(p => p.Campaigns)
                    .HasForeignKey(d => d.CurrencyTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Campaign_CurrencyType");
            });

            modelBuilder.Entity<CampaignRequirement>(entity =>
            {
                entity.ToTable("CampaignRequirement");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 5)");

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.CampaignRequirements)
                    .HasForeignKey(d => d.CampaignId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_CampaignRequirement_Campaing");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.CampaignRequirements)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("fk_CampaignRequirement_Category");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CampaignRequirements)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("fk_CampaignRequirement_Product");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("fk_Category_Category");
            });

            modelBuilder.Entity<CurrencyType>(entity =>
            {
                entity.ToTable("CurrencyType");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SortName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Symbol).HasMaxLength(1);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.No)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DiscountCode>(entity =>
            {
                entity.ToTable("DiscountCode");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Value).HasColumnType("decimal(18, 5)");

                entity.HasOne(d => d.CurrencyType)
                    .WithMany(p => p.DiscountCodes)
                    .HasForeignKey(d => d.CurrencyTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_DiscountCode_CurrencyType");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.DiscountCodes)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("fk_DiscountCode_Customer");
            });

            modelBuilder.Entity<DiscountCodeRequirement>(entity =>
            {
                entity.ToTable("DiscountCodeRequirement");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 5)");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.DiscountCodeRequirements)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("fk_DiscountCodeRequirement_Category");

                entity.HasOne(d => d.DiscountCode)
                    .WithMany(p => p.DiscountCodeRequirements)
                    .HasForeignKey(d => d.DiscountCodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_DiscountCodeRequirement_DiscountCode");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.DiscountCodeRequirements)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("fk_DiscountCodeRequirement_Product");
            });

            modelBuilder.Entity<DiscountCodeUsedHistory>(entity =>
            {
                entity.HasKey(e => new { e.DiscountCodeId, e.SalesOrderId })
                    .HasName("pk_DiscountCodeUsedHistory");

                entity.ToTable("DiscountCodeUsedHistory");

                entity.Property(e => e.UsedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.DiscountCode)
                    .WithMany(p => p.DiscountCodeUsedHistories)
                    .HasForeignKey(d => d.DiscountCodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_DiscountCodeUsedHistory_DiscountCode");

                entity.HasOne(d => d.SalesOrder)
                    .WithMany(p => p.DiscountCodeUsedHistories)
                    .HasForeignKey(d => d.SalesOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_DiscountCodeUsedHistory_SalesOrder");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GiftCard>(entity =>
            {
                entity.ToTable("GiftCard");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Value).HasColumnType("decimal(18, 5)");

                entity.HasOne(d => d.CurrencyType)
                    .WithMany(p => p.GiftCards)
                    .HasForeignKey(d => d.CurrencyTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Campaign_CurrecyType");
            });

            modelBuilder.Entity<GiftCardHistory>(entity =>
            {
                entity.HasKey(e => new { e.GiftCardId, e.SalesOrderId })
                    .HasName("pk_GiftCardHistory");

                entity.ToTable("GiftCardHistory");

                entity.Property(e => e.UsedAmount).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.UsedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.GiftCard)
                    .WithMany(p => p.GiftCardHistories)
                    .HasForeignKey(d => d.GiftCardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_GiftCardHistory_GiftCard");

                entity.HasOne(d => d.SalesOrder)
                    .WithMany(p => p.GiftCardHistories)
                    .HasForeignKey(d => d.SalesOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_GiftCardHistory_SalesOrder");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Barcode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TaxRate).HasColumnType("decimal(10, 5)");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("fk_Product_Category");
            });

            modelBuilder.Entity<ProductAttribute>(entity =>
            {
                entity.ToTable("ProductAttribute");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Categoty)
                    .WithMany(p => p.ProductAttributes)
                    .HasForeignKey(d => d.CategotyId)
                    .HasConstraintName("fk_ProductAttribute_Categoty");
            });

            modelBuilder.Entity<ProductAttributeValue>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.ProductAttributeId })
                    .HasName("pk_ProductAttributeValue");

                entity.ToTable("ProductAttributeValue");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.ProductAttribute)
                    .WithMany(p => p.ProductAttributeValues)
                    .HasForeignKey(d => d.ProductAttributeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ProductAttributeValue_ProductAttribute");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductAttributeValues)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ProductAttributeValue_Product");
            });

            modelBuilder.Entity<ProductPrice>(entity =>
            {
                entity.ToTable("ProductPrice");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 5)");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Tax).HasColumnType("decimal(18, 5)");

                entity.HasOne(d => d.CurrencyType)
                    .WithMany(p => p.ProductPrices)
                    .HasForeignKey(d => d.CurrencyTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ProductPrice_CurrencyType");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.ProductPrices)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("fk_ProductPrice_Customer");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductPrices)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ProductPrice_Product");
            });

            modelBuilder.Entity<SalesOrder>(entity =>
            {
                entity.ToTable("SalesOrder");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.OrderNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.CurrencyType)
                    .WithMany(p => p.SalesOrders)
                    .HasForeignKey(d => d.CurrencyTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SalesOrder_CurrencyType");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.SalesOrders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SalesOrder_Customer");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.SalesOrders)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("fk_SalesOrder_Employee");
            });

            modelBuilder.Entity<SalesOrderDetail>(entity =>
            {
                entity.ToTable("SalesOrderDetail");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 5)");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.SalesOrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SalesOrderDetail_Product");

                entity.HasOne(d => d.SalesOrder)
                    .WithMany(p => p.SalesOrderDetails)
                    .HasForeignKey(d => d.SalesOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SalesOrderDetail_SalesOrder");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
