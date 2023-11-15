using AutoMapper;
using DB.Model.DetailInfo;
using Shared.Dto.Detail.DetailChild;

namespace Shared.AutoMapperProfiles.Details;

public class GetDetailChildDtoProfile : Profile
{
    public GetDetailChildDtoProfile()
    {
        CreateProjection<DetailsChild, GetDetailChildDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(child => child.ChildId))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(child => child.Child!.Title))
            .ForMember(dest => dest.SerialNumber, opt => opt.MapFrom(child => child.Child!.SerialNumber))
            .ForMember(dest => dest.Unit, opt => opt.MapFrom(child => child.Child!.Unit!.Title))
            .ForMember(dest => dest.IsComposite, opt => opt.MapFrom(child => child.Child!.DetailsChildren!.Count > 0));
    }
}
