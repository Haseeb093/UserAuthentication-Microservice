using AuthenticationService;
using Domain.CustomModels;
using Domain.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Service.Interfaces;

namespace UserAuthentication.Controllers
{
    [Route("api")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IDataService dataService;

        public DataController(IDataService _dataService)
        {
            dataService = _dataService;
        }

        [HttpGet]
        [Route("GetAllPatients")]
        public async Task<ResponseObject<List<UserDto>>> GetAllPatients()
        {
            var responseObj = new ResponseObject<List<UserDto>>();
            try
            {
                responseObj = await dataService.GetAllPatients();
            }
            catch (Exception ex)
            {
                Helper.SetFailuerRespose(responseObj, ex);
            }
            return responseObj;
        }

        [HttpGet]
        [Route("GetAllDoctors")]
        public async Task<ResponseObject<List<UserDto>>> GetAllDoctors()
        {
            var responseObj = new ResponseObject<List<UserDto>>();
            try
            {
                responseObj = await dataService.GetAllDoctors();
            }
            catch (Exception ex)
            {
                Helper.SetFailuerRespose(responseObj, ex);
            }
            return responseObj;
        }

        [HttpGet]
        [Route("GetCountries")]
        public async Task<ResponseObject<List<CountriesDto>>> GetCountries()
        {
            var responseObj = new ResponseObject<List<CountriesDto>>();
            try
            {
                responseObj = await dataService.GetCountries();
            }
            catch (Exception ex)
            {
                Helper.SetFailuerRespose(responseObj, ex);
            }
            return responseObj;
        }

        [HttpGet]
        [Route("GetStates")]
        public async Task<ResponseObject<List<StatesDto>>> GetStates(JObject requestObject)
        {
            var responseObj = new ResponseObject<List<StatesDto>>();
            try
            {
                int countryId = JsonConvert.DeserializeObject<int>(requestObject["data"]["countryId"].ToString());
                responseObj = await dataService.GetCountryStates(countryId);
            }
            catch (Exception ex)
            {
                Helper.SetFailuerRespose(responseObj, ex);
            }
            return responseObj;
        }

        [HttpGet]
        [Route("GetCities")]
        public async Task<ResponseObject<List<CitiesDto>>> GetCities(JObject requestObject)
        {
            var responseObj = new ResponseObject<List<CitiesDto>>();
            try
            {
                int stateId = JsonConvert.DeserializeObject<int>(requestObject["data"]["stateId"].ToString());
                responseObj = await dataService.GetStateCities(stateId);
            }
            catch (Exception ex)
            {
                Helper.SetFailuerRespose(responseObj, ex);
            }
            return responseObj;
        }
    }
}
