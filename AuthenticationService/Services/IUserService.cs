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
        public Task<ResponseObject<List<Countries>>> GetCountries();
        public Task<ResponseObject<List<Cities>>> GetStateCities(int stateId);
        public Task<ResponseObject<List<States>>> GetCountryStates(int countryId);
        public Task<ResponseObject<LoginResponse>> Login(LoginParam loginParam);
        public Task<ResponseObject<List<Error>>> Register(RegisterParam registerParam);

    }
}
