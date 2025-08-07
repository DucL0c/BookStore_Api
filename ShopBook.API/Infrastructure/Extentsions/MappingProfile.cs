using AutoMapper;
using ShopBook.Data.Models;
using ShopBook.Data.ViewModels;

namespace ShopBook.API.Infrastructure.Extentsions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserViewModels>()
             .ReverseMap();
            CreateMap<Author, AuthorViewModels>()
             .ReverseMap();
            CreateMap<BookImage, BookImageViewModels>()
             .ReverseMap();
            CreateMap<BookSeller, BookSellerViewModels>()
             .ReverseMap();
            CreateMap<BookAuthor, BookAuthorViewModels>()
             .ReverseMap();
            CreateMap<Seller, SellerViewModels>()
             .ReverseMap();
            CreateMap<Category, CategoryViewModels>()
             .ReverseMap();
            CreateMap<BookSpecification, BookSpecificationViewModels>()
             .ReverseMap();
            CreateMap<ProductReview, ProductReviewViewModels>()
            .ReverseMap();
            CreateMap<Order, OrderViewModels>()
            .ReverseMap();
            CreateMap<OrderItem, OrderItemViewModels>()
            .ReverseMap();
            CreateMap<Cart, CartViewModels>()
            .ReverseMap();
        }
    }
}
