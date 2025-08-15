using AutoMapper;
using ShopBook.Data.Dto;
using ShopBook.Data.Models;
using ShopBook.Data.ViewModels;
using ShopBook.Model.Dtos;

namespace ShopBook.API.Infrastructure.Extentsions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, RegisterDto>().ReverseMap();
            CreateMap<User, UserDto>();
            CreateMap<User, UserLoginDto>().ReverseMap();
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
            CreateMap<CartItem, CartItemViewModels>()
            .ReverseMap();
            CreateMap<Book, BookViewModels>()
            .ReverseMap();
        }
    }
}
