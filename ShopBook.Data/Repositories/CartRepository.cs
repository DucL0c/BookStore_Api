using Microsoft.EntityFrameworkCore;
using ShopBook.Data.Dto;
using ShopBook.Data.Infrastructure;
using ShopBook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ShopBook.Data.Repositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<List<CartDetailDto>> GetById(int Id);
        Task<List<CartDetailDto>> GetByUserIdAsync(int userId);
        Task<List<CartDetailDto>> GetAllByKeyWord(string keyWord);
        Task<List<CartDetailDto>> GetAllAsync();

        Task<Cart> AddToCartAsync(int userId, int bookId, int quantity);

    }
    public class CartRepository : RepositoryBase <Cart> , ICartRepository
    {
        private readonly BookstoreContext _context;
        public CartRepository(BookstoreContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<CartDetailDto>> GetByUserIdAsync(int userId)
        {
            var query = _context.Carts
                .Where(c => c.UserId == userId)
                .Include(c => c.User)
                .Include(c => c.CartItems).ThenInclude(ci => ci.Book).ThenInclude(b => b.BookSellers).ThenInclude(bs => bs.Seller)
                .Include(c => c.CartItems).ThenInclude(ci => ci.Book).ThenInclude(b => b.BookImages);


            var carts = await query.ToListAsync();

            var result = carts.Select(c => new CartDetailDto
            {
                CartId = c.CartId,
                UserId = c.UserId,
                CreatedAt = c.CreatedAt,
                CartItems = c.CartItems.Select(ct => new CartItemsDto
                {
                    CartItemId = ct.CartItemId,
                    BookId = ct.BookId,
                    Quantity = ct.Quantity,
                    Price = ct.Price,
                    Book = ct.Book == null ? null : new BookCartDto
                    {
                        BookId = ct.Book.BookId,
                        Name = ct.Book.Name,
                        Price = ct.Book.ListPrice,
                        BookSellers = ct.Book.BookSellers.Select(bs => new BookSellerCartDto
                        {
                            Id = bs.Id,
                            Price = bs.Price,
                            IsBestStore = bs.IsBestStore,
                            Seller = new SellerCartDto
                            {
                                SellerId = bs.Seller.SellerId,
                                Name = bs.Seller.Name,
                                Link = bs.Seller.Link,
                                Logo = bs.Seller.Logo
                            }
                        }).ToList(),
                        BookImages = ct.Book.BookImages.Select(bi => new BookImageCartDto
                        {
                            ImageId = bi.ImageId,
                            BaseUrl = bi.BaseUrl,
                            SmallUrl = bi.SmallUrl,
                            MediumUrl = bi.MediumUrl,
                            LargeUrl = bi.LargeUrl,
                            ThumbnailUrl = bi.ThumbnailUrl,
                            IsGallery = bi.IsGallery
                        }).ToList(),
                    }
                }).ToList(),
                User = c.User == null ? null : new UserCartDto
                {
                    UserId = c.User.UserId,
                    Name = c.User.Name,
                    Phone = c.User.Phone,
                    Address = c.User.Address
                } 

            }).ToList();
            return result;
        }
        public async Task<List<CartDetailDto>> GetAllByKeyWord(string keyWord)
        {
            var query = _context.Carts
                .Where(c => string.IsNullOrEmpty(keyWord) || c.User.Name.ToString().Contains(keyWord) || c.User.Email.ToString().Contains(keyWord))
                .Include(c => c.User)
                .Include(c => c.CartItems).ThenInclude(ci => ci.Book).ThenInclude(b => b.BookSellers).ThenInclude(bs => bs.Seller)
                .Include(c => c.CartItems).ThenInclude(ci => ci.Book).ThenInclude(b => b.BookImages);

            var carts = await query.ToListAsync();

            var result = carts.Select(c => new CartDetailDto
            {
                CartId = c.CartId,
                UserId = c.UserId,
                CreatedAt = c.CreatedAt,
                CartItems = c.CartItems.Select(ct => new CartItemsDto
                {
                    CartItemId = ct.CartItemId,
                    BookId = ct.BookId,
                    Quantity = ct.Quantity,
                    Price = ct.Price,
                    Book = ct.Book == null ? null : new BookCartDto
                    {
                        BookId = ct.Book.BookId,
                        Name = ct.Book.Name,
                        Price = ct.Book.ListPrice,
                        BookSellers = ct.Book.BookSellers.Select(bs => new BookSellerCartDto
                        {
                            Id = bs.Id,
                            Price = bs.Price,
                            IsBestStore = bs.IsBestStore,
                            Seller = new SellerCartDto
                            {
                                SellerId = bs.Seller.SellerId,
                                Name = bs.Seller.Name,
                                Link = bs.Seller.Link,
                                Logo = bs.Seller.Logo
                            }
                        }).ToList(),
                        BookImages = ct.Book.BookImages.Select(bi => new BookImageCartDto
                        {
                            ImageId = bi.ImageId,
                            BaseUrl = bi.BaseUrl,
                            SmallUrl = bi.SmallUrl,
                            MediumUrl = bi.MediumUrl,
                            LargeUrl = bi.LargeUrl,
                            ThumbnailUrl = bi.ThumbnailUrl,
                            IsGallery = bi.IsGallery
                        }).ToList(),
                    }
                }).ToList(),
                User = c.User == null ? null : new UserCartDto
                {
                    UserId = c.User.UserId,
                    Name = c.User.Name,
                    Phone = c.User.Phone,
                    Address = c.User.Address
                }

            }).ToList();
            return result;
        }

        public async Task<List<CartDetailDto>> GetById(int Id)
        {
            var query = _context.Carts
                .Where(c => c.CartId == Id)
                .Include(c => c.User)
                .Include(c => c.CartItems).ThenInclude(ci => ci.Book).ThenInclude(b => b.BookSellers).ThenInclude(bs => bs.Seller)
                .Include(c => c.CartItems).ThenInclude(ci => ci.Book).ThenInclude(b => b.BookImages);

            var carts = await query.ToListAsync();

            var result = carts.Select(c => new CartDetailDto
            {
                CartId = c.CartId,
                UserId = c.UserId,
                CreatedAt = c.CreatedAt,
                CartItems = c.CartItems.Select(ct => new CartItemsDto
                {
                    CartItemId = ct.CartItemId,
                    BookId = ct.BookId,
                    Quantity = ct.Quantity,
                    Price = ct.Price,
                    Book = ct.Book == null ? null : new BookCartDto
                    {
                        BookId = ct.Book.BookId,
                        Name = ct.Book.Name,
                        Price = ct.Book.ListPrice,
                        BookSellers = ct.Book.BookSellers.Select(bs => new BookSellerCartDto
                        {
                            Id = bs.Id,
                            Price = bs.Price,
                            IsBestStore = bs.IsBestStore,
                            Seller = new SellerCartDto
                            {
                                SellerId = bs.Seller.SellerId,
                                Name = bs.Seller.Name,
                                Link = bs.Seller.Link,
                                Logo = bs.Seller.Logo
                            }
                        }).ToList(),
                        BookImages = ct.Book.BookImages.Select(bi => new BookImageCartDto
                        {
                            ImageId = bi.ImageId,
                            BaseUrl = bi.BaseUrl,
                            SmallUrl = bi.SmallUrl,
                            MediumUrl = bi.MediumUrl,
                            LargeUrl = bi.LargeUrl,
                            ThumbnailUrl = bi.ThumbnailUrl,
                            IsGallery = bi.IsGallery
                        }).ToList(),
                    }
                }).ToList(),
                User = c.User == null ? null : new UserCartDto
                {
                    UserId = c.User.UserId,
                    Name = c.User.Name,
                    Phone = c.User.Phone,
                    Address = c.User.Address
                }

            }).ToList();
            return result;
        }

        public async Task<List<CartDetailDto>> GetAllAsync()
        {
            var query = _context.Carts
               .Include(c => c.User)
               .Include(c => c.CartItems).ThenInclude(ci => ci.Book).ThenInclude(b => b.BookSellers).ThenInclude(bs => bs.Seller)
               .Include(c => c.CartItems).ThenInclude(ci => ci.Book).ThenInclude(b => b.BookImages);

            var carts = await query.ToListAsync();

            var result = carts.Select(c => new CartDetailDto
            {
                CartId = c.CartId,
                UserId = c.UserId,
                CreatedAt = c.CreatedAt,
                CartItems = c.CartItems.Select(ct => new CartItemsDto
                {
                    CartItemId = ct.CartItemId,
                    BookId = ct.BookId,
                    Quantity = ct.Quantity,
                    Price = ct.Price,
                    Book = ct.Book == null ? null : new BookCartDto
                    {
                        BookId = ct.Book.BookId,
                        Name = ct.Book.Name,
                        Price = ct.Book.ListPrice,
                        BookSellers = ct.Book.BookSellers.Select(bs => new BookSellerCartDto
                        {
                            Id = bs.Id,
                            Price = bs.Price,
                            IsBestStore = bs.IsBestStore,
                            Seller = new SellerCartDto
                            {
                                SellerId = bs.Seller.SellerId,
                                Name = bs.Seller.Name,
                                Link = bs.Seller.Link,
                                Logo = bs.Seller.Logo
                            }
                        }).ToList(),
                        BookImages = ct.Book.BookImages.Select(bi => new BookImageCartDto
                        {
                            ImageId = bi.ImageId,
                            BaseUrl = bi.BaseUrl,
                            SmallUrl = bi.SmallUrl,
                            MediumUrl = bi.MediumUrl,
                            LargeUrl = bi.LargeUrl,
                            ThumbnailUrl = bi.ThumbnailUrl,
                            IsGallery = bi.IsGallery
                        }).ToList(),
                    }
                }).ToList(),
                User = c.User == null ? null : new UserCartDto
                {
                    UserId = c.User.UserId,
                    Name = c.User.Name,
                    Phone = c.User.Phone,
                    Address = c.User.Address
                }

            }).ToList();
            return result;
        }

        public async Task<Cart> AddToCartAsync(int userId, int bookId, int quantity)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null) throw new Exception("Book not found");

            // Retrieve the user's cart
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Carts.Add(cart);
            }

            // Check if the item already exists
            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.BookId == bookId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.Price = existingItem.Quantity * book.ListPrice;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    BookId = bookId,
                    Quantity = quantity,
                    Price = book.ListPrice * quantity
                });
            }

            await _context.SaveChangesAsync(); // Save changes to the database
            return cart; // Return the updated cart
        }
    }
}
