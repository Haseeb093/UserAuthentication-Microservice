using Domain.CustomModels;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public interface IUserService
    {
        public Task<ResponseObject<List<UserDto>>> GetAllDoctors();
        public Task<ResponseObject<List<UserDto>>> GetAllPatients();
        public Task<ResponseObject<List<CountriesDto>>> GetCountries();
        public Task<ResponseObject<TokenDto>> Login(LoginDto loginParam);
        public Task<ResponseObject<List<CitiesDto>>> GetStateCities(int stateId);
        public Task<ResponseObject<List<StatesDto>>> GetCountryStates(int countryId);
        public Task<ResponseObject<List<Error>>> UnlockUser(LockOutUserDto lockOutUser);
        public Task<ResponseObject<List<Error>>> LockOutUser(LockOutUserDto lockOutUser);
        public Task<ResponseObject<List<Error>>> Register(UserDto registerParam, string userName);
        public Task<ResponseObject<List<Error>>> ChangeUserPassword(ChangePasswordDto changePasswordDto);
       

    }
}
