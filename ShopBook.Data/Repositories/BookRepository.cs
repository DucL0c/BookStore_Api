using Microsoft.EntityFrameworkCore;
using ShopBook.Data.Dto;
using ShopBook.Data.Infrastructure;
using ShopBook.Data.Models;

namespace ShopBook.Data.Repositories
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<List<BookDto>> GetAllByKeyWord(string keyWord, int? categoryId);
        Task<List<BookDto>> GetAll();
        Task<BookDto?> GetBookByIdAsync(int bookId);
    }
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        private readonly BookstoreContext _context;
        public BookRepository(BookstoreContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<BookDto>> GetAll()
        {
            var books = await _context.Books
                .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
                .Include(b => b.BookSpecifications)
                .Include(b => b.BookImages)
                .Include(b => b.BookSellers).ThenInclude(bs => bs.Seller)
                .Include(b => b.ProductReviews).ThenInclude(pr => pr.User)
                .Include(b => b.Category)
                .Include(b => b.OrderItems)
                .AsNoTracking()
                .ToListAsync();

            return books.Select(b =>
            {
                var quantitySold = b.OrderItems?.Sum(oi => oi.Quantity) ?? 0;

                return new BookDto
                {
                    BookId = b.BookId,
                    Name = b.Name,
                    Description = b.Description,
                    ShortDescription = b.ShortDescription,
                    OriginalPrice = b.OriginalPrice,
                    ListPrice = b.ListPrice,
                    RatingAverage = b.RatingAverage,

                    // ✅ Gán QuantitySold object
                    QuantitySold = new QuantitySoldDto
                    {
                        Value = quantitySold,
                        Text = quantitySold >= 1000 ? "Đã bán hơn 1000+" : $"Đã bán {quantitySold}"
                    },

                    BookAuthors = b.BookAuthors.Select(ba => new BookAuthorDto
                    {
                        BookId = ba.BookId,
                        AuthorId = ba.AuthorId,
                        Author = new AuthorDto
                        {
                            AuthorId = ba.Author.AuthorId,
                            Name = ba.Author.Name,
                            Slug = ba.Author.Slug
                        }
                    }).ToList(),

                    BookSpecifications = b.BookSpecifications.Select(s => new BookSpecificationDto
                    {
                        Id = s.Id,
                        SpecName = s.SpecName,
                        SpecValue = s.SpecValue,
                        SpecCode = s.SpecCode
                    }).ToList(),

                    BookImages = b.BookImages.Select(i => new BookImageDto
                    {
                        ImageId = i.ImageId,
                        BaseUrl = i.BaseUrl,
                        SmallUrl = i.SmallUrl,
                        MediumUrl = i.MediumUrl,
                        LargeUrl = i.LargeUrl,
                        ThumbnailUrl = i.ThumbnailUrl,
                        IsGallery = i.IsGallery
                    }).ToList(),

                    BookSellers = b.BookSellers.Select(bs => new BookSellerDto
                    {
                        Id = bs.Id,
                        Price = bs.Price,
                        IsBestStore = bs.IsBestStore,
                        Sku = bs.Sku,
                        StoreId = bs.StoreId,
                        ProductId = bs.ProductId,
                        Seller = new SellerDto
                        {
                            SellerId = bs.Seller.SellerId,
                            Name = bs.Seller.Name,
                            Link = bs.Seller.Link,
                            Logo = bs.Seller.Logo
                        }
                    }).ToList(),

                    ProductReviews = b.ProductReviews.Select(r => new ProductReviewDto
                    {
                        ReviewId = r.ReviewId,
                        Comment = r.Comment,
                        Rating = r.Rating,
                        ReviewDate = r.ReviewDate,
                        User = new ReviewUserDto
                        {
                            UserId = r.User.UserId,
                            FullName = r.User.FullName,
                            NickName = r.User.NickName,
                            Email = r.User.Email,
                            Phone = r.User.Phone,
                            Address = r.User.Address,
                            Gender = r.User.Gender,
                            BirthDay = r.User.BirthDay
                        }
                    }).ToList(),

                    Category = new CategoryDto
                    {
                        CategoryId = b.Category.CategoryId,
                        Name = b.Category.Name,
                        IsLeaf = b.Category.IsLeaf,
                    }
                };
            }).ToList();
        }

        public async Task<List<BookDto>> GetAllByKeyWord(string keyWord, int? categoryId)
        {
            var books = await _context.Books
                .Where(b =>
                    (string.IsNullOrEmpty(keyWord) ||
                     b.Name.Contains(keyWord) ||
                     b.Description.Contains(keyWord) ||
                     b.ShortDescription.Contains(keyWord))
                    &&
                    (!categoryId.HasValue || b.CategoryId == categoryId.Value)
                )
                .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
                .Include(b => b.BookSpecifications)
                .Include(b => b.BookImages)
                .Include(b => b.BookSellers).ThenInclude(bs => bs.Seller)
                .Include(b => b.ProductReviews).ThenInclude(pr => pr.User)
                .Include(b => b.Category)
                .Include(b => b.OrderItems)
                .AsNoTracking()
                .ToListAsync();

            return books.Select(b =>
            {
                var quantitySold = b.OrderItems?.Sum(oi => oi.Quantity) ?? 0;

                return new BookDto
                {
                    BookId = b.BookId,
                    Name = b.Name,
                    Description = b.Description,
                    ShortDescription = b.ShortDescription,
                    OriginalPrice = b.OriginalPrice,
                    ListPrice = b.ListPrice,
                    RatingAverage = b.RatingAverage,

                    QuantitySold = new QuantitySoldDto
                    {
                        Value = quantitySold,
                        Text = quantitySold >= 1000 ? "Đã bán hơn 1000+" : $"Đã bán {quantitySold}"
                    },

                    BookAuthors = b.BookAuthors.Select(ba => new BookAuthorDto
                    {
                        BookId = ba.BookId,
                        AuthorId = ba.AuthorId,
                        Author = new AuthorDto
                        {
                            AuthorId = ba.Author.AuthorId,
                            Name = ba.Author.Name,
                            Slug = ba.Author.Slug
                        }
                    }).ToList(),

                    BookSpecifications = b.BookSpecifications.Select(s => new BookSpecificationDto
                    {
                        Id = s.Id,
                        SpecName = s.SpecName,
                        SpecValue = s.SpecValue,
                        SpecCode = s.SpecCode
                    }).ToList(),

                    BookImages = b.BookImages.Select(i => new BookImageDto
                    {
                        ImageId = i.ImageId,
                        BaseUrl = i.BaseUrl,
                        SmallUrl = i.SmallUrl,
                        MediumUrl = i.MediumUrl,
                        LargeUrl = i.LargeUrl,
                        ThumbnailUrl = i.ThumbnailUrl,
                        IsGallery = i.IsGallery
                    }).ToList(),

                    BookSellers = b.BookSellers.Select(bs => new BookSellerDto
                    {
                        Id = bs.Id,
                        Price = bs.Price,
                        IsBestStore = bs.IsBestStore,
                        Sku = bs.Sku,
                        StoreId = bs.StoreId,
                        ProductId = bs.ProductId,
                        Seller = new SellerDto
                        {
                            SellerId = bs.Seller.SellerId,
                            Name = bs.Seller.Name,
                            Link = bs.Seller.Link,
                            Logo = bs.Seller.Logo
                        }
                    }).ToList(),

                    ProductReviews = b.ProductReviews.Select(r => new ProductReviewDto
                    {
                        ReviewId = r.ReviewId,
                        Comment = r.Comment,
                        Rating = r.Rating,
                        ReviewDate = r.ReviewDate,
                        User = new ReviewUserDto
                        {
                            UserId = r.User.UserId,
                            FullName = r.User.FullName,
                            NickName = r.User.NickName,
                            Email = r.User.Email,
                            Phone = r.User.Phone,
                            Address = r.User.Address,
                            Gender = r.User.Gender,
                            BirthDay = r.User.BirthDay
                        }
                    }).ToList(),

                    Category = new CategoryDto
                    {
                        CategoryId = b.Category.CategoryId,
                        Name = b.Category.Name,
                        IsLeaf = b.Category.IsLeaf,
                    }
                };
            }).ToList();
        }


        public async Task<BookDto?> GetBookByIdAsync(int bookId)
        {
            var book = await _context.Books
            .Where(b => b.BookId == bookId)
            .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
            .Include(b => b.BookSpecifications)
            .Include(b => b.BookImages)
            .Include(b => b.BookSellers).ThenInclude(bs => bs.Seller)
            .Include(b => b.ProductReviews).ThenInclude(pr => pr.User)
            .Include(b => b.Category)
            .Include(b => b.OrderItems)
            .AsNoTracking()
            .FirstOrDefaultAsync();

            if (book == null) return null;

            var quantitySold = book.OrderItems?.Sum(oi => oi.Quantity) ?? 0;

            return new BookDto
            {
                BookId = book.BookId,
                Name = book.Name,
                Description = book.Description,
                ShortDescription = book.ShortDescription,
                OriginalPrice = book.OriginalPrice,
                ListPrice = book.ListPrice,
                RatingAverage = book.RatingAverage,

                QuantitySold = new QuantitySoldDto
                {
                    Value = quantitySold,
                    Text = quantitySold >= 1000 ? "Đã bán hơn 1000+" : $"Đã bán {quantitySold}"
                },

                BookAuthors = book.BookAuthors.Select(ba => new BookAuthorDto
                {
                    BookId = ba.BookId,
                    AuthorId = ba.AuthorId,
                    Author = new AuthorDto
                    {
                        AuthorId = ba.Author.AuthorId,
                        Name = ba.Author.Name,
                        Slug = ba.Author.Slug
                    }
                }).ToList(),

                BookSpecifications = book.BookSpecifications.Select(s => new BookSpecificationDto
                {
                    Id = s.Id,
                    SpecName = s.SpecName,
                    SpecValue = s.SpecValue,
                    SpecCode = s.SpecCode
                }).ToList(),

                BookImages = book.BookImages.Select(i => new BookImageDto
                {
                    ImageId = i.ImageId,
                    BaseUrl = i.BaseUrl,
                    SmallUrl = i.SmallUrl,
                    MediumUrl = i.MediumUrl,
                    LargeUrl = i.LargeUrl,
                    ThumbnailUrl = i.ThumbnailUrl,
                    IsGallery = i.IsGallery
                }).ToList(),

                BookSellers = book.BookSellers.Select(bs => new BookSellerDto
                {
                    Id = bs.Id,
                    Price = bs.Price,
                    IsBestStore = bs.IsBestStore,
                    Sku = bs.Sku,
                    StoreId = bs.StoreId,
                    ProductId = bs.ProductId,
                    Seller = new SellerDto
                    {
                        SellerId = bs.Seller.SellerId,
                        Name = bs.Seller.Name,
                        Link = bs.Seller.Link,
                        Logo = bs.Seller.Logo
                    }
                }).ToList(),

                ProductReviews = book.ProductReviews.Select(r => new ProductReviewDto
                {
                    ReviewId = r.ReviewId,
                    Comment = r.Comment,
                    Rating = r.Rating,
                    ReviewDate = r.ReviewDate,
                    User = new ReviewUserDto
                    {
                        UserId = r.User.UserId,
                        FullName = r.User.FullName,
                        NickName = r.User.NickName,
                        Email = r.User.Email,
                        Phone = r.User.Phone,
                        Address = r.User.Address,
                        Gender = r.User.Gender,
                        BirthDay = r.User.BirthDay
                    }
                }).ToList(),

                Category = new CategoryDto
                {
                    CategoryId = book.Category.CategoryId,
                    Name = book.Category.Name,
                    IsLeaf = book.Category.IsLeaf
                }
            };
        }
    }
}
