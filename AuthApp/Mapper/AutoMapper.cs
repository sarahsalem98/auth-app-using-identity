
using AuthAppBusiness.Models;
using AuthAppDataAccess.Models;
using AutoMapper;

namespace AuthApp.Mapper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<User, UserMV>().ForMember(dest => dest.Roles, opt =>
            opt.MapFrom(src => src.UserRoles.Select(ur => new RolesMV
            {
                Id = ur.Role.Id,
                Type = ur.Role.Type ?? 0,
                Name = ur.Role.Name
            })));
            CreateMap<UserMV, User>()
     .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src =>
         src.Roles.Select(role => new UserRole
         {
             RoleId = role.Id,  // Assuming UserRole has RoleId as a foreign key
             Role = new Role
             {
                 Id = role.Id,
                 Type = role.Type,
                 Name = role.Name
             }
         })));
            CreateMap<Role, AuthAppBusiness.Models.RolesMV>().ReverseMap();
            CreateMap<DoctorMV, Doctor>().ReverseMap();
        }
    }
}
