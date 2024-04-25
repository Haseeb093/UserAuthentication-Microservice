using AutoMapper;
using Domain.CustomModels;
using Domain.Enum;
using Domain.Helper;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using Services.ApplicationContext;

namespace Service.Services
{
    public class DataService : IDataService
    {
        protected readonly IMapper _mapper;
        protected readonly ApplicationDbContext _dbContext;
        protected readonly UserManager<IdentityUser> _userManager;

        public DataService(IMapper mapper, ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _mapper= mapper;
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<ResponseObject<List<UserDto>>> GetAllDoctors()
        {
            var response = new ResponseObject<List<UserDto>>();
            response.Data = await (from user in _userManager.Users
                                   join userRoles in _dbContext.UserRoles on user.Id equals userRoles.UserId
                                   join roles in _dbContext.Roles on userRoles.RoleId equals roles.Id
                                   join gender in _dbContext.Genders on (user as Users).GenderId equals gender.GenderId
                                   join department in _dbContext.Departments on (user as Users).DepartmentId equals department.DepartmentId
                                   join country in _dbContext.Countries on (user as Users).CountryId equals country.CountryId
                                   join state in _dbContext.States on (user as Users).StateId equals state.StateId
                                   join city in _dbContext.Cities on (user as Users).CityId equals city.CityId
                                   where roles.Name == Roles.Doctor.ToString()
                                   select new UserDto
                                   {
                                       Id = Helper.EncryptString(user.Id),
                                       FirstName = (user as Users).FirstName,
                                       LastName = (user as Users).LastName,
                                       UserName = user.UserName,
                                       Email = user.Email,
                                       Role = roles.Name,
                                       GenderId = gender.GenderId,
                                       Gender = gender.Name,
                                       DateofBirth = (user as Users).DateofBirth.ToString(),
                                       Address1 = (user as Users).Address1,
                                       Address2 = (user as Users).Address2,
                                       PostalCode = (user as Users).PostalCode,
                                       PhoneNumber = (user as Users).PhoneNumber,
                                       SecondaryPhoneNumber = (user as Users).SecondaryPhoneNumber,
                                       DepartmentId = department.DepartmentId,
                                       Department = department.Name,
                                       CountryId = country.CountryId,
                                       Country = country.Name,
                                       StateId = state.StateId,
                                       State = state.Name,
                                       CityId = city.CityId,
                                       City = city.Name
                                   }).ToListAsync();
            Helper.SetSuccessRespose(response);
            return response;
        }

        public async Task<ResponseObject<List<UserDto>>> GetAllPatients()
        {
            var response = new ResponseObject<List<UserDto>>();
            response.Data = await (from user in _userManager.Users
                                   join userRoles in _dbContext.UserRoles on user.Id equals userRoles.UserId
                                   join roles in _dbContext.Roles on userRoles.RoleId equals roles.Id
                                   join gender in _dbContext.Genders on (user as Users).GenderId equals gender.GenderId
                                   join department in _dbContext.Departments on (user as Users).DepartmentId equals department.DepartmentId
                                   join country in _dbContext.Countries on (user as Users).CountryId equals country.CountryId
                                   join state in _dbContext.States on (user as Users).StateId equals state.StateId
                                   join city in _dbContext.Cities on (user as Users).CityId equals city.CityId
                                   where roles.Name == Roles.Patient.ToString()
                                   select new UserDto
                                   {
                                       Id = Helper.EncryptString(user.Id),
                                       FirstName = (user as Users).FirstName,
                                       LastName = (user as Users).LastName,
                                       UserName = user.UserName,
                                       Email = user.Email,
                                       Role = roles.Name,
                                       GenderId = gender.GenderId,
                                       Gender = gender.Name,
                                       DateofBirth = (user as Users).DateofBirth.ToString(),
                                       Address1 = (user as Users).Address1,
                                       Address2 = (user as Users).Address2,
                                       PostalCode = (user as Users).PostalCode,
                                       PhoneNumber = (user as Users).PhoneNumber,
                                       SecondaryPhoneNumber = (user as Users).SecondaryPhoneNumber,
                                       DepartmentId = department.DepartmentId,
                                       Department = department.Name,
                                       CountryId = country.CountryId,
                                       Country = country.Name,
                                       StateId = state.StateId,
                                       State = state.Name,
                                       CityId = city.CityId,
                                       City = city.Name
                                   }).ToListAsync();
            Helper.SetSuccessRespose(response);
            return response;
        }

        public async Task<ResponseObject<List<CountriesDto>>> GetCountries()
        {
            var response = new ResponseObject<List<CountriesDto>>();
            response.Data = _mapper.Map<List<CountriesDto>>(await _dbContext.Countries.ToListAsync());
            Helper.SetSuccessRespose(response);
            return response;
        }

        public async Task<ResponseObject<List<StatesDto>>> GetCountryStates(int countryId)
        {
            var response = new ResponseObject<List<StatesDto>>();
            response.Data = _mapper.Map<List<StatesDto>>(await _dbContext.States.Where(s => s.Countries.CountryId == countryId).ToListAsync());
            Helper.SetSuccessRespose(response);
            return response;
        }

        public async Task<ResponseObject<List<CitiesDto>>> GetStateCities(int stateId)
        {
            var response = new ResponseObject<List<CitiesDto>>();
            response.Data = _mapper.Map<List<CitiesDto>>(await _dbContext.Cities.Where(s => s.States.StateId == stateId).ToListAsync());
            Helper.SetSuccessRespose(response);
            return response;
        }
    }
}
