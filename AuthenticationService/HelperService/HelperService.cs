using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Validation
{
    public class HelperService
    {
        protected IdentityUser user = null;
        private readonly IConfiguration _configuration;
        protected readonly UserManager<IdentityUser> _userManager;

        public HelperService(IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        protected async Task<bool> LoginValidation(LoginParam loginParam, ResponseObject<LoginResponse> responseObject)
        {
            if (loginParam != null && !string.IsNullOrEmpty(loginParam.Username) && loginParam.Username.Trim() != "")
            {
                user = await GetUserByName(loginParam.Username);

                if (user != null && !string.IsNullOrEmpty(loginParam.Password) && loginParam.Password.Trim() != "" && await _userManager.CheckPasswordAsync(user, loginParam.Password))
                {
                    return true;
                }
            }
            responseObject.Message = "User Name or Password not Matched !";
            return false;
        }

        protected async Task<bool> RegisterValidation(RegisterParam registerParam, ResponseObject<List<Error>> response)
        {
            bool isValid = true;
            response.Data = new List<Error>();

            if (registerParam != null && !string.IsNullOrEmpty(registerParam.Username) && !string.IsNullOrEmpty(registerParam.Email)
                                               && !string.IsNullOrEmpty(registerParam.Password) && registerParam.Username.Trim() != "" &&
                                               registerParam.Email.Trim() != "" && registerParam.Password.Trim() != "")
            {
                var regUser = await GetUserByName(registerParam.Username);

                if (regUser != null)
                {
                    isValid = false;
                    Error error = new Error();
                    error.Code = "User Exist";
                    error.Message = "User Name Already Exist !";
                    response.Data.Add(error);
                }

                var regEmail = await GetUserByEmail(registerParam.Email);

                if (regEmail != null)
                {
                    isValid = false;
                    Error error = new Error();
                    error.Code = "Email Exist";
                    error.Message = "User Email Already Exist !";
                    response.Data.Add(error);
                }
            }
            else
            {
                isValid = false;
                response.Message = "Field Required !";
            }
            return isValid;
        }

        protected bool RegisterResponseValidation(IdentityResult identityResult, ResponseObject<List<Error>> responseObject)
        {
            if (!identityResult.Succeeded)
            {
                foreach (var err in identityResult.Errors)
                {
                    Error error = new Error();
                    error.Code = err.Code;
                    error.Message = err.Description;
                    responseObject.Data.Add(error);
                }
            }
            else
            {
                responseObject.Message = "User Registered Successfully";
            }
            return identityResult.Succeeded;
        }

        protected async Task<IdentityUser> GetUserByName(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        protected async Task<IdentityUser> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        protected LoginResponse GetToken(List<Claim> authClaims)
        {
            var tokenResponse = new LoginResponse();
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])), SecurityAlgorithms.HmacSha256)
                );
            tokenResponse.UserName = authClaims?.FirstOrDefault(s => s.Type.Contains(ClaimTypes.Name))?.Value;
            tokenResponse.Token = new JwtSecurityTokenHandler().WriteToken(token);
            tokenResponse.ValidTo = token.ValidTo;
            return tokenResponse;
        }
    }
}
