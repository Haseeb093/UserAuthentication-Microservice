using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Domain.Helper;
using Domain.CustomModels;
using Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using Service.Interfaces;

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

    }
}
