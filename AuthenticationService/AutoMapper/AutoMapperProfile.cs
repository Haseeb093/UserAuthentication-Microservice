using AutoMapper;
using Domain.Models;
using Domain.CustomModels;
using Microsoft.AspNetCore.Identity;

namespace Service.AutoMapperr
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<States, StatesDto>();
            CreateMap<Cities, CitiesDto>();
            CreateMap<Countries, CountriesDto>();
            CreateMap<Users, UserDto>();
            CreateMap<IdentityUser, Users>();
        }
    }
}
