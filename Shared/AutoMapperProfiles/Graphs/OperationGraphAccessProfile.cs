using AutoMapper;
using DB.Model.StorageInfo.Graph;
using Shared.Dto.Graph.Access;

namespace Shared.AutoMapperProfiles.Graphs;

public class OperationGraphAccessProfile : Profile
{
    public OperationGraphAccessProfile()
    {
        CreateProjection<OperationGraphUser, GetAllUserGraphAccessDto>()
            .ForMember(
                dest => dest.FFL,
                opt => opt.MapFrom(gu => gu.User!.FFL))
            .ForMember(
                dest => dest.ProfessionNumber,
                opt => opt.MapFrom(gu => gu.User!.ProfessionNumber))
            .ForMember(
                dest => dest.Subdivision,
                opt => opt.MapFrom(gu => gu.User!.Subdivision.Title))
            .ForMember(
                dest => dest.Access,
                opt => opt.MapFrom(gu => gu.IsReadonly ? "Только на чтение" : "Полный"));
    }
}