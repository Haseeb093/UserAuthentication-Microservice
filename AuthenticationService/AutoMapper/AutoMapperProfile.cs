using AutoMapper;
using Domain.Models;
using Domain.CustomModels;

namespace Service.AutoMapperr
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<States, StatesDto>();
            CreateMap<Cities, CitiesDto>();
            CreateMap<Countries, CountriesDto>();
        }
    }
}
