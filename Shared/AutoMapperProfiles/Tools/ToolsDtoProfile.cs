using AutoMapper;
using DB.Model.ToolInfo;
using Shared.Dto.Tools;

namespace Shared.AutoMapperProfiles.Tools;

public class ToolsDtoProfile : Profile
{
    public ToolsDtoProfile()
    {
        CreateMap<AddToolDto, Tool>();
        CreateMap<Tool, GetToolDto>();
    }
}