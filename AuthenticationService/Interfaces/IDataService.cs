using Domain.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IDataService
    {
        public Task<ResponseObject<List<UserDto>>> GetAllDoctors();
        public Task<ResponseObject<List<UserDto>>> GetAllPatients();
        public Task<ResponseObject<List<CountriesDto>>> GetCountries();
        public Task<ResponseObject<List<CitiesDto>>> GetStateCities(int stateId);
        public Task<ResponseObject<List<StatesDto>>> GetCountryStates(int countryId);
    }
}
