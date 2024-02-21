using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Domain.Helper;
using Service.Services;
using Domain.CustomModels;
using Domain.Models;
using System.IdentityModel.Tokens.Jwt;

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
        public async Task<ResponseObject<TokenDto>> Login(JObject requestObject)
        {
            var responseObj = new ResponseObject<TokenDto>();
            try
            {
                LoginDto loginUser = JsonConvert.DeserializeObject<LoginDto>(requestObject["data"].ToString());
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
                UserDto userDto = JsonConvert.DeserializeObject<UserDto>(requestObject["data"].ToString());
                responseObj = await userService.Register(userDto, Helper.GetUserFromToken(HttpContext.User));
            }
            catch (Exception ex)
            {
                Helper.SetFailuerRespose(responseObj, ex);
            }
            return responseObj;
        }

        [HttpPut]
        [Route("UpdateUser")]
        public async Task<ResponseObject<List<Error>>> UpdateUser(JObject requestObject)
        {
            var responseObj = new ResponseObject<List<Error>>();
            try
            {
                UserDto userDto = JsonConvert.DeserializeObject<UserDto>(requestObject["data"].ToString());
                userDto.Id = Helper.GetUserIdFromToken(HttpContext.User);
                responseObj = await userService.UpdateUser(userDto);
            }
            catch (Exception ex)
            {
                Helper.SetFailuerRespose(responseObj, ex);
            }
            return responseObj;
        }

        [HttpPut]
        [Route("ChangePassword")]
        public async Task<ResponseObject<List<Error>>> ChangePassword(JObject requestObject)
        {
            var responseObj = new ResponseObject<List<Error>>();
            try
            {
                ChangePasswordDto passwordDto = JsonConvert.DeserializeObject<ChangePasswordDto>(requestObject["data"].ToString());
                passwordDto.UserId = Helper.GetUserIdFromToken(HttpContext.User);
                responseObj = await userService.ChangeUserPassword(passwordDto);
            }
            catch (Exception ex)
            {
                Helper.SetFailuerRespose(responseObj, ex);
            }
            return responseObj;
        }

        [HttpPut]
        [Route("LockOutUser")]
        public async Task<ResponseObject<List<Error>>> LockOutUser(JObject requestObject)
        {
            var responseObj = new ResponseObject<List<Error>>();
            try
            {
                LockOutUserDto lockOutUser = JsonConvert.DeserializeObject<LockOutUserDto>(requestObject["data"].ToString());
                lockOutUser.UpdateByUser = Helper.GetUserFromToken(HttpContext.User);
                responseObj = await userService.LockOutUser(lockOutUser);
            }
            catch (Exception ex)
            {
                Helper.SetFailuerRespose(responseObj, ex);
            }
            return responseObj;
        }

        [HttpPut]
        [Route("UnlockUser")]
        public async Task<ResponseObject<List<Error>>> UnlockUser(JObject requestObject)
        {
            var responseObj = new ResponseObject<List<Error>>();
            try
            {
                LockOutUserDto lockOutUser = JsonConvert.DeserializeObject<LockOutUserDto>(requestObject["data"].ToString());
                lockOutUser.UpdateByUser = Helper.GetUserFromToken(HttpContext.User);
                responseObj = await userService.UnlockUser(lockOutUser);
            }
            catch (Exception ex)
            {
                Helper.SetFailuerRespose(responseObj, ex);
            }
            return responseObj;
        }

        [HttpGet]
        [Route("GetAllPatients")]
        public async Task<ResponseObject<List<UserDto>>> GetAllPatients()
        {
            var responseObj = new ResponseObject<List<UserDto>>();
            try
            {
                responseObj = await userService.GetAllPatients();
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
                responseObj = await userService.GetAllDoctors();
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
        public async Task<ResponseObject<List<StatesDto>>> GetStates(JObject requestObject)
        {
            var responseObj = new ResponseObject<List<StatesDto>>();
            try
            {
                int countryId = JsonConvert.DeserializeObject<int>(requestObject["data"]["countryId"].ToString());
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
        public async Task<ResponseObject<List<CitiesDto>>> GetCities(JObject requestObject)
        {
            var responseObj = new ResponseObject<List<CitiesDto>>();
            try
            {
                int stateId = JsonConvert.DeserializeObject<int>(requestObject["data"]["stateId"].ToString());
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
