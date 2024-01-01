using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Domain.Helper;
using Service.Services;
using Domain.CustomModels;
using Domain.Models;

namespace UserAuthentication.UserControllers
{
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService _userService)
        {
            userService = _userService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ResponseObject<LoginResponse>> Login(JObject requestObject)
        {
            var responseObj = new ResponseObject<LoginResponse>();
            try
            {
                LoginParam loginUser = JsonConvert.DeserializeObject<LoginParam>(requestObject["data"].ToString());
                responseObj = await userService.Login(loginUser);
            }
            catch (Exception ex)
            {
                Helper.SetFailuerRespose(responseObj, ex);
            }
            return responseObj;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ResponseObject<List<Error>>> Register(JObject requestObject)
        {
            var responseObj = new ResponseObject<List<Error>>();
            try
            {
                RegisterParam registerParam = JsonConvert.DeserializeObject<RegisterParam>(requestObject["data"].ToString());
                responseObj = await userService.Register(registerParam);
            }
            catch (Exception ex)
            {
                Helper.SetFailuerRespose(responseObj, ex);
            }
            return responseObj;
        }

        [HttpGet]
        [Route("GetCountries")]
        public async Task<ResponseObject<List<Countries>>> GetCountries()
        {
            var responseObj = new ResponseObject<List<Countries>>();
            try
            {
                responseObj = await userService.GetCountries();
            }
            catch (Exception ex)
            {
                Helper.SetFailuerRespose(responseObj, ex);
            }
            return responseObj;
        }

        [HttpGet]
        [Route("GetStates")]
        public async Task<ResponseObject<List<States>>> GetStates(JObject requestObject)
        {
            var responseObj = new ResponseObject<List<States>>();
            try
            {
                int countryId = JsonConvert.DeserializeObject<int>(requestObject["data"].ToString());
                responseObj = await userService.GetCountryStates(countryId);
            }
            catch (Exception ex)
            {
                Helper.SetFailuerRespose(responseObj, ex);
            }
            return responseObj;
        }

        [HttpGet]
        [Route("GetCities")]
        public async Task<ResponseObject<List<Cities>>> GetCities(JObject requestObject)
        {
            var responseObj = new ResponseObject<List<Cities>>();
            try
            {
                int stateId = JsonConvert.DeserializeObject<int>(requestObject["data"].ToString());
                responseObj = await userService.GetStateCities(stateId);
            }
            catch (Exception ex)
            {
                Helper.SetFailuerRespose(responseObj, ex);
            }
            return responseObj;
        }

    }
}
