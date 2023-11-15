using AutoMapper;
using DB.Model.DetailInfo;
using Shared.Dto.Detail;

namespace Shared.AutoMapperProfiles.Details;

public class GetDetailDtoProfile : Profile
{
    public GetDetailDtoProfile()
    {
        CreateProjection<Detail, GetDetailInfoWithIdDto>();

        CreateProjection<Detail, GetDetailDto>()
            .ForMember(dest => dest.DetailType, opt => opt.MapFrom(detail => detail.DetailType!.Title))
            .ForMember(dest => dest.IsComposite, opt => opt.MapFrom(detail => detail.DetailsChildren!.Count > 0));

        CreateProjection<Detail, DetailBaseInfoForProductsTreeDto>()
            .ForMember(dest => dest.Unit, opt => opt.MapFrom(detail => detail.Unit!.Title));
    }
}
