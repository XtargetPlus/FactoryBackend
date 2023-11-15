using AutoMapper;
using DB.Model.AccessoryInfo;
using DB.Model.DetailInfo;
using DB.Model.ProductInfo;
using DB.Model.StatusInfo;
using DB.Model.SubdivisionInfo;
using DB.Model.TechnologicalProcessInfo.TechnologicalProcessDataInfo;
using DB.Model.UserInfo;
using DB.Model.UserInfo.RoleInfo;
using Shared.Dto;

namespace Shared.AutoMapperProfiles;

public class BaseDtoProfile : Profile
{
    public BaseDtoProfile()
    {
        CreateProjection<Subdivision, BaseDto>();
        CreateProjection<Unit, BaseDto>();
        CreateProjection<DetailType, BaseDto>();
        CreateProjection<Profession, BaseDto>();
        CreateProjection<BlankType, BaseDto>();
        CreateProjection<Material, BaseDto>();
        CreateProjection<Status, BaseDto>();
        CreateProjection<Role, BaseDto>();
        CreateProjection<OutsideOrganization, BaseDto>();
        CreateProjection<Client, BaseDto>();
    }
}
