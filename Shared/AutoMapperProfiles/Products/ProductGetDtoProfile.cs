using AutoMapper;
using DB.Model.ProductInfo;
using Shared.Dto.Product;

namespace Shared.AutoMapperProfiles.Products;

public class ProductGetDtoProfile : Profile
{
    public ProductGetDtoProfile()
    {
        CreateProjection<Product, ProductGetDto>()
            .ForMember(dest => dest.SerialNumber, opt => opt.MapFrom(product => product.Detail!.SerialNumber))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(product => product.Detail!.Title))
            .ForMember(dest => dest.DetailId, opt => opt.MapFrom(product => product.Detail!.Id))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(product => product.Id));
    }
}
