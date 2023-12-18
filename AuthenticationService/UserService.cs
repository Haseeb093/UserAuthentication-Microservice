﻿using Domain.Enum;
using Domain.Helper;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.Validation;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace AuthenticationService
{
    public class UserService : HelperService, IUserService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(IConfiguration configuration, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : base(configuration, userManager)
        {
            _roleManager = roleManager;
        }

        public async Task<ResponseObject<LoginResponse>> Login(LoginParam loginParam)
        {
            var response = new ResponseObject<LoginResponse>();

            if (await LoginValidation(loginParam, response))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                response.Data = GetToken(authClaims);
                Helper.SetSuccessRespose(response);
            }
            else
            {
                Helper.SetFailuerRespose(response);
            }
            return response;
        }

        public async Task<ResponseObject<List<Error>>> Register(RegisterParam registerParam)
        {
            var response = new ResponseObject<List<Error>>();

            if (await RegisterValidation(registerParam, response))
            {
                IdentityUser user = new()
                {
                    UserName = registerParam.Username,
                    Email = registerParam.Email,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                if (RegisterResponseValidation(await _userManager.CreateAsync(user, registerParam.Password), response))
                {
                    if (!await _roleManager.RoleExistsAsync(UserRoles.Admin.ToString()))
                        await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin.ToString()));

                    if (!await _roleManager.RoleExistsAsync(UserRoles.User.ToString()))
                        await _roleManager.CreateAsync(new IdentityRole(UserRoles.User.ToString()));

                    if (registerParam.IsAdmin && await _roleManager.RoleExistsAsync(UserRoles.Admin.ToString()))
                        await _userManager.AddToRoleAsync(user, UserRoles.Admin.ToString());

                    if (await _roleManager.RoleExistsAsync(UserRoles.User.ToString()))
                        await _userManager.AddToRoleAsync(user, UserRoles.User.ToString());

                    Helper.SetSuccessRespose(response);
                }
                else
                {
                    Helper.SetFailuerRespose(response);
                }
            }
            else
            {
                Helper.SetFailuerRespose(response);
            }
            return response;
        }
    }
}
