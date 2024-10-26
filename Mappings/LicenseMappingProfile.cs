using AutoMapper;
using FirmTracker_Server.Entities;
using FirmTracker_Server.Models;
using NHibernate.Type;
using NuGet.Packaging.Licenses;
using System.ComponentModel;

namespace FirmTracker_Server.Mappings
{
    public class LicenseMappingProfile : Profile
    {
        public LicenseMappingProfile()
        {
           // CreateMap<License, LicenseDto>();
           // CreateMap<LicenseDto, License>();
          //  CreateMap<CreateLicenseDto, License>();
          //  CreateMap<LicType, LicTypeDto>();
          //  CreateMap<LicTypeDto, LicType>();
            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>().ForSourceMember(x => x.Password, y => y.DoNotValidate());
        }
    }
}
