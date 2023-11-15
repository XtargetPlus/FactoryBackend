using AutoMapper;
using DB.Model.DetailInfo;
using DB.Model.ProductInfo;
using DB.Model.SubdivisionInfo.EquipmentInfo;
using Shared.Dto.Detail;

namespace Shared.AutoMapperProfiles;

public class BaseIdSerialTitleDtoProfile : Profile
{
    public BaseIdSerialTitleDtoProfile()
    {
        CreateProjection<Product, BaseIdSerialTitleDto>()
            .ForMember(dest => dest.DetailId, opt => opt.MapFrom(product => product.Detail!.Id))
            .ForMember(dest => dest.SerialNumber, opt => opt.MapFrom(product => product.Detail!.SerialNumber))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(product => product.Detail!.Title));

        CreateProjection<Detail, BaseIdSerialTitleDto>()
            .ForMember(dest => dest.DetailId, opt => opt.MapFrom(detail => detail.Id));
        CreateProjection<Equipment, BaseIdSerialTitleDto>()
            .ForMember(dest => dest.DetailId, opt => opt.MapFrom(equipment => equipment.Id));
        CreateProjection<EquipmentDetail, BaseIdSerialTitleDto>()
            .ForMember(dest => dest.DetailId, opt => opt.MapFrom(equipmentDetail => equipmentDetail.Id));
        CreateProjection<DetailReplaceability, BaseIdSerialTitleDto>()
            .ForMember(dest => dest.DetailId, opt => opt.MapFrom(detail => detail.SecondDetailId))
            .ForMember(dest => dest.SerialNumber, opt => opt.MapFrom(detail => detail.SecondDetail.SerialNumber))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(detail => detail.SecondDetail.Title));
    }
}
