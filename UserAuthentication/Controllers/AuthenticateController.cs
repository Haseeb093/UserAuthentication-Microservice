﻿using AuthenticationService;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Domain.Helper;

namespace UserAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IUserService userService;

        public AuthenticateController(IUserService _userService)
        {
            userService = _userService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ResponseObject<LoginResponse>> Login(JObject requestObject)
        {
            var responseObj = new ResponseObject<LoginResponse>();
            try
            {
                LoginParam loginUser = JsonConvert.DeserializeObject<LoginParam>(requestObject["data"].ToString());

                if (loginUser != null)
                {
                    responseObj = await userService.Login(loginUser);
                }
                else
                {
                    Helper.SetFailuerRespose(responseObj);
                }
            }
            catch (Exception ex)
            {
                Helper.SetFailuerRespose(responseObj, ex);
            }
            return responseObj;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ResponseObject<List<Error>>> Register(JObject requestObject)
        {
            var responseObj = new ResponseObject<List<Error>>();
            try
            {
                RegisterParam registerParam = JsonConvert.DeserializeObject<RegisterParam>(requestObject["data"].ToString());

                if (registerParam != null)
                {
                    responseObj = await userService.Register(registerParam);
                }
                else
                {
                    responseObj.Data = null;
                    Helper.SetFailuerRespose(responseObj);
                }
            }
            catch (Exception ex)
            {
                Helper.SetFailuerRespose(responseObj, ex);
            }
            return responseObj;
        }

    }
}
