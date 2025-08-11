using Microsoft.EntityFrameworkCore;
using ShopBook.Data.Models;

namespace ShopBook.Data;

public partial class BookstoreContext : DbContext
{
    public BookstoreContext()
    {
    }

    public BookstoreContext(DbContextOptions<BookstoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookAuthor> BookAuthors { get; set; }

    public virtual DbSet<BookImage> BookImages { get; set; }

    public virtual DbSet<BookSeller> BookSellers { get; set; }

    public virtual DbSet<BookSpecification> BookSpecifications { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<ProductReview> ProductReviews { get; set; }

    public virtual DbSet<Seller> Sellers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=192.168.10.237;Database=bookstore;User ID=Huy;Password=123456;TrustServerCertificate=True");
    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("Server=DESKTOP-EU72BJ2\\SQLEXPRESS;Database=bookstore;User ID=sa;Password=123456;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.AuthorId).HasName("PK__Authors__70DAFC349A56BD7B");

            entity.Property(e => e.AuthorId).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Slug).HasMaxLength(255);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__Books__3DE0C2075C43CF49");

            entity.Property(e => e.BookId).ValueGeneratedOnAdd();
            entity.Property(e => e.ListPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.OriginalPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Category).WithMany(p => p.Books)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Books__CategoryI__267ABA7A");
        });

        modelBuilder.Entity<BookAuthor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_BookAuthors");

           entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Book).WithMany(p => p.BookAuthors)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_BookAuthors_Books");

            entity.HasOne(d => d.Author).WithMany(p => p.BookAuthors)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_BookAuthor_Authors");
        });

        modelBuilder.Entity<BookImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__BookImag__7516F70C403A90B6");

            entity.HasOne(d => d.Book).WithMany(p => p.BookImages)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK__BookImage__BookI__34C8D9D1");
        });

        modelBuilder.Entity<BookSeller>(entity =>
        {
            entity.HasKey(e => new { e.Id }).HasName("PK__BookSell__3A1EFFBFDCB3FE94");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductId).HasMaxLength(50);
            entity.Property(e => e.Sku)
                .HasMaxLength(50)
                .HasColumnName("SKU");

            entity.HasOne(d => d.Book).WithMany(p => p.BookSellers)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__BookSelle__BookI__30F848ED");

            entity.HasOne(d => d.Seller).WithMany(p => p.BookSellers)
                .HasForeignKey(d => d.SellerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__BookSelle__Selle__31EC6D26");
        });

        modelBuilder.Entity<BookSpecification>(entity =>
        {
            entity.HasKey(e => new { e.Id }).HasName("PK__BookSpec__14D3021338D4B78F");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.SpecName).HasMaxLength(255);
            entity.Property(e => e.SpecValue).HasMaxLength(255);

            entity.HasOne(d => d.Book).WithMany(p => p.BookSpecifications)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__BookSpeci__BookI__37A5467C");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Carts__51BCD7B7821E013A");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Carts__UserId__4AB81AF0");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("PK__CartItem__488B0B0AFA2D8462");

            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Book).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK__CartItems__BookI__4E88ABD4");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .HasConstraintName("FK__CartItems__CartI__4D94879B");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0BB3CCCFCC");

            entity.Property(e => e.CategoryId).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCF5DBFA7ED");

            entity.Property(e => e.OrderId).ValueGeneratedOnAdd();

            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod).HasMaxLength(100);
            entity.Property(e => e.ShippingAddress).HasMaxLength(500);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Orders__UserId__52593CB8");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PK__OrderIte__57ED06814F5727D9");

            entity.Property(e => e.OrderItemId).ValueGeneratedOnAdd();

            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Book).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK__OrderItem__BookI__5629CD9C");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderItem__Order__5535A963");
        });

        modelBuilder.Entity<ProductReview>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__ProductR__74BC79CEFC82498C");

            entity.Property(e => e.ReviewId).ValueGeneratedOnAdd();

            entity.Property(e => e.ReviewDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Book).WithMany(p => p.ProductReviews)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK__ProductRe__BookI__5BE2A6F2");

            entity.HasOne(d => d.User).WithMany(p => p.ProductReviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__ProductRe__UserI__5AEE82B9");
        });

        modelBuilder.Entity<Seller>(entity =>
        {
            entity.HasKey(e => e.SellerId).HasName("PK__Sellers__7FE3DB81033DCB04");

            entity.Property(e => e.SellerId).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C9867C321");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105347CA639E4").IsUnique();

            entity.Property(e => e.UserId).ValueGeneratedOnAdd(); 
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.NickName).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasDefaultValue("user");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
