using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CuaHangVHT.Data;

public partial class TuanStoreContext : DbContext
{
    public TuanStoreContext()
    {
    }

    public TuanStoreContext(DbContextOptions<TuanStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BlogPost> BlogPosts { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderDetailPromotion> OrderDetailPromotions { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductSize> ProductSizes { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<StockIn> StockIns { get; set; }

    public virtual DbSet<StockOut> StockOuts { get; set; }

    public virtual DbSet<TransactionLog> TransactionLogs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=MSI\\SQLEXPRESS;Initial Catalog=TuanStore;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BlogPost>(entity =>
        {
            entity.HasKey(e => e.IdBlogPost).HasName("PK__BlogPost__85B7BB19B2E76EB3");

            entity.Property(e => e.IdBlogPost).HasColumnName("Id_BlogPost");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.User).WithMany(p => p.BlogPosts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BlogPosts__UserI__76619304");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__2EF52A272A7B5A67");

            entity.ToTable("Cart");

            entity.Property(e => e.CartId).HasColumnName("cart_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cart__user_id__66603565");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("PK__CartItem__5D9A6C6EAF8CA6E6");

            entity.Property(e => e.CartItemId).HasColumnName("cart_item_id");
            entity.Property(e => e.CartId).HasColumnName("cart_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CartItems__cart___693CA210");

            entity.HasOne(d => d.Product).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CartItems__produ__6A30C649");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__D54EE9B47F98DD62");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__C52E0BA8F0A415F9");

            entity.HasIndex(e => e.PhoneNumber, "UQ__Employee__A1936A6B6A160980").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Employee__AB6E616494FE6A98").IsUnique();

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .HasColumnName("phone_number");
            entity.Property(e => e.PositionId).HasColumnName("position_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Position).WithMany(p => p.Employees)
                .HasForeignKey(d => d.PositionId)
                .HasConstraintName("FK_Employees_Position");

            entity.HasOne(d => d.User).WithMany(p => p.Employees)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Employees_Users");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__46596229BB6438FC");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.CachThanhToan)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.CachVanChuyen)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DiaChi).HasMaxLength(100);
            entity.Property(e => e.DienThoai).HasMaxLength(20);
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.GhiChu).HasMaxLength(100);
            entity.Property(e => e.Hoten).HasMaxLength(50);
            entity.Property(e => e.MaTrangThai)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("pending")
                .HasColumnName("status");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_price");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Employee).WithMany(p => p.Orders)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_Orders_Employees");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__user_id__5EBF139D");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__3C5A408053CB795B");

            entity.Property(e => e.OrderDetailId).HasColumnName("order_detail_id");
            entity.Property(e => e.Discount)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("discount");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.ProductSizeId).HasColumnName("product_size_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__order__619B8048");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__produ__628FA481");

            entity.HasOne(d => d.ProductSize).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductSizeId)
                .HasConstraintName("FK_OrderDetails_ProductSizes");
        });

        modelBuilder.Entity<OrderDetailPromotion>(entity =>
        {
            entity.HasKey(e => e.OrderDetailPromotionId).HasName("PK__OrderDet__D688B29B0BA9C67B");

            entity.Property(e => e.OrderDetailPromotionId).HasColumnName("order_detail_promotion_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.PromotionId).HasColumnName("promotion_id");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetailPromotions)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_OrderDetailPromotions_Products");

            entity.HasOne(d => d.Promotion).WithMany(p => p.OrderDetailPromotions)
                .HasForeignKey(d => d.PromotionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__promo__1F98B2C1");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__ED1FC9EA6FCB55D5");

            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("payment_date");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(20)
                .HasColumnName("payment_method");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(20)
                .HasDefaultValue("pending")
                .HasColumnName("payment_status");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__order___6EF57B66");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.PositionId).HasName("PK__Position__99A0E7A4C85157FA");

            entity.ToTable("Position");

            entity.HasIndex(e => e.PositionName, "UQ__Position__0030EAD73022923A").IsUnique();

            entity.Property(e => e.PositionId).HasColumnName("position_id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.PositionName)
                .HasMaxLength(50)
                .HasColumnName("position_name");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__47027DF5BEC69F46");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("image_url");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Stock).HasColumnName("stock");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Products__catego__4222D4EF");
        });

        modelBuilder.Entity<ProductSize>(entity =>
        {
            entity.HasKey(e => e.ProductSizeId).HasName("PK__ProductS__062A9A680AB8A493");

            entity.Property(e => e.ProductSizeId).HasColumnName("product_size_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.SizeName)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("size_name");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductSizes)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductSi__produ__2B0A656D");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.PromotionId).HasName("PK__Promotio__2CB9556B6959313D");

            entity.Property(e => e.PromotionId).HasColumnName("promotion_id");
            entity.Property(e => e.DiscountPercent)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("discount_percent");
            entity.Property(e => e.EndPrDate)
                .HasColumnType("datetime")
                .HasColumnName("endPr_date");
            entity.Property(e => e.PromotionName)
                .HasMaxLength(100)
                .HasColumnName("promotion_name");
            entity.Property(e => e.StartPrDate)
                .HasColumnType("datetime")
                .HasColumnName("startPr_date");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("status");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Reviews__60883D9083708E3B");

            entity.Property(e => e.ReviewId).HasColumnName("review_id");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reviews__product__73BA3083");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reviews__user_id__74AE54BC");
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.StockId).HasName("PK__Stock__E8666862E06F57E8");

            entity.ToTable("Stock");

            entity.Property(e => e.StockId).HasColumnName("stock_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Stock__product_i__49C3F6B7");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Stock__warehouse__4AB81AF0");
        });

        modelBuilder.Entity<StockIn>(entity =>
        {
            entity.HasKey(e => e.StockInId).HasName("PK__StockIn__F657737D5397F835");

            entity.ToTable("StockIn");

            entity.Property(e => e.StockInId).HasColumnName("stock_in_id");
            entity.Property(e => e.DateIn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("date_in");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Supplier)
                .HasMaxLength(100)
                .HasColumnName("supplier");
            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");

            entity.HasOne(d => d.Product).WithMany(p => p.StockIns)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockIn__product__4F7CD00D");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.StockIns)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockIn__warehou__4E88ABD4");
        });

        modelBuilder.Entity<StockOut>(entity =>
        {
            entity.HasKey(e => e.StockOutId).HasName("PK__StockOut__19F07083C00FE100");

            entity.ToTable("StockOut");

            entity.Property(e => e.StockOutId).HasColumnName("stock_out_id");
            entity.Property(e => e.DateOut)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("date_out");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Reason)
                .HasMaxLength(255)
                .HasColumnName("reason");
            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");

            entity.HasOne(d => d.Product).WithMany(p => p.StockOuts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockOut__produc__5441852A");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.StockOuts)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockOut__wareho__534D60F1");
        });

        modelBuilder.Entity<TransactionLog>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__85C600AF6D46D444");

            entity.ToTable("TransactionLog");

            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(10)
                .HasColumnName("transaction_type");
            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");

            entity.HasOne(d => d.Product).WithMany(p => p.TransactionLogs)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__produ__5812160E");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.TransactionLogs)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__wareh__59063A47");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__B9BE370FC5AD1171");

            entity.HasIndex(e => e.Email, "UQ__Users__AB6E6164C4E51A9C").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__Users__F3DBC5720AFBEAF0").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("full_name");
            entity.Property(e => e.ImgAvater)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("IMG_avater");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .HasColumnName("phone_number");
            entity.Property(e => e.RandomKey)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("randomKey");
            entity.Property(e => e.ResetPasswordToken).HasMaxLength(50);
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasDefaultValue("customer")
                .HasColumnName("role");
            entity.Property(e => e.TokenExpiration).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.WarehouseId).HasName("PK__Warehous__734FE6BF1BC5389B");

            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .HasColumnName("location");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
