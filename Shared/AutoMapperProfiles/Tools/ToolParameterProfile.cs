using AutoMapper;
using DB.Model.ToolInfo;
using Shared.Dto.Tools;

namespace Shared.AutoMapperProfiles.Tools;

public class ToolParameterProfile : Profile
{
    public ToolParameterProfile()
    {
        CreateMap<ToolParameter, GetToolParameterDto>();
        CreateMap<Tool, ToolParameterConcrete>();


        CreateProjection<ToolParameterConcrete, GetToolParametersDto>()
            .ForMember(
                tcc => tcc.Value,
                opt => opt.MapFrom(parameter => parameter.Value))
            .ForMember(
                tcc => tcc.Id,
                opt => opt.MapFrom(parameter => parameter.ToolParameterId))
            .ForMember(
                tcc => tcc.Title,
                opt => opt.MapFrom(parameter => parameter.ToolParameter.Title));
    }
}