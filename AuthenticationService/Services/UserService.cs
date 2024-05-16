using AutoMapper;
using Domain.CustomModels;
using Domain.Enum;
using Domain.Helper;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Service.Interfaces;
using Service.Validation;
using Services.ApplicationContext;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace AuthenticationService
{
    public class UserService : UserHelperService, IUserService
    {
        public UserService(IConfiguration configuration, ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager) : base(configuration, userManager, roleManager, signInManager, dbContext)
        {
        }

        public async Task<ResponseObject<TokenDto>> Login(LoginDto loginParam)
        {
            var response = new ResponseObject<TokenDto>();
            var user = await LoginValidation(loginParam, response);

            if (user != null)
            {
                var Roles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sid, Helper.EncryptString(user.Id)),
                    new Claim(JwtRegisteredClaimNames.Name,Helper.EncryptString(user.UserName)),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                foreach (var userRole in Roles)
                {
                    authClaims.Add(new Claim("Roles", userRole));
                }
                response.Data = GetToken(authClaims);
            }
            return response;
        }

        public async Task<ResponseObject<List<Error>>> Register(UserDto userDto, string insertedByUser)
        {
            var response = new ResponseObject<List<Error>>();

            if (await RegisterValidation(userDto, response))
            {
                var user = new Users()
                {
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    UserName = userDto.UserName,
                    Email = userDto.Email,
                    GenderId = userDto.GenderId,
                    CountryId = userDto.CountryId,
                    StateId = userDto.StateId,
                    CityId = userDto.CityId,
                    PostalCode = userDto.PostalCode,
                    Address1 = userDto.Address1,
                    Address2 = userDto.Address2,
                    DateofBirth = DateTime.Parse(userDto.DateofBirth),
                    PhoneNumber = userDto.PhoneNumber,
                    SecondaryPhoneNumber = userDto.SecondaryPhoneNumber,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    InsertedBy = insertedByUser,
                    UpdatedBy = insertedByUser
                };

                if (IdentityResponseValidation(await _userManager.CreateAsync(user, userDto.Password), response))
                {
                    if (!await _roleManager.RoleExistsAsync(Roles.Admin.ToString()))
                        await _roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));

                    if (!await _roleManager.RoleExistsAsync(Roles.Doctor.ToString()))
                        await _roleManager.CreateAsync(new IdentityRole(Roles.Doctor.ToString()));

                    if (!await _roleManager.RoleExistsAsync(Roles.Patient.ToString()))
                        await _roleManager.CreateAsync(new IdentityRole(Roles.Patient.ToString()));

                    if (!await _roleManager.RoleExistsAsync(Roles.Staff.ToString()))
                        await _roleManager.CreateAsync(new IdentityRole(Roles.Staff.ToString()));

                    switch (userDto.Role)
                    {
                        case "Admin":
                            await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                            break;
                        case "Doctor":
                            await _userManager.AddToRoleAsync(user, Roles.Doctor.ToString());
                            break;
                        case "Patient":
                            await _userManager.AddToRoleAsync(user, Roles.Patient.ToString());
                            break;
                        default:
                            await _userManager.AddToRoleAsync(user, Roles.Staff.ToString());
                            break;
                    }
                }
            }
            return response;
        }

        public async Task<ResponseObject<List<Error>>> RegisterPatient(UserDto userDto)
        {
            var response = new ResponseObject<List<Error>>();

            if (await RegisterValidation(userDto, response, true))
            {
                var user = new Users()
                {
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    UserName = userDto.UserName,
                    Email = userDto.Email,
                    GenderId = userDto.GenderId,
                    DateofBirth = DateTime.Parse(userDto.DateofBirth),
                    PhoneNumber = userDto.PhoneNumber,
                    SecondaryPhoneNumber = userDto.SecondaryPhoneNumber,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    InsertedBy = "Self",
                    UpdatedBy = "Self"
                };

                if (IdentityResponseValidation(await _userManager.CreateAsync(user, userDto.Password), response))
                {
                    if (!await _roleManager.RoleExistsAsync(Roles.Admin.ToString()))
                        await _roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));

                    if (!await _roleManager.RoleExistsAsync(Roles.Doctor.ToString()))
                        await _roleManager.CreateAsync(new IdentityRole(Roles.Doctor.ToString()));

                    if (!await _roleManager.RoleExistsAsync(Roles.Patient.ToString()))
                        await _roleManager.CreateAsync(new IdentityRole(Roles.Patient.ToString()));

                    if (!await _roleManager.RoleExistsAsync(Roles.Staff.ToString()))
                        await _roleManager.CreateAsync(new IdentityRole(Roles.Staff.ToString()));

                    await _userManager.AddToRoleAsync(user, Roles.Patient.ToString());
                }
            }
            return response;
        }

        public async Task<ResponseObject<List<Error>>> UpdateUser(UserDto userDto)
        {
            var response = new ResponseObject<List<Error>>();

            if (await UpdateUserValidation(userDto, response))
            {
                var user = await GetUserById(userDto.Id);
                user.FirstName = userDto.FirstName;
                user.LastName = userDto.LastName;
                user.Email = userDto.Email;
                user.GenderId = userDto.GenderId;
                user.CountryId = userDto.CountryId;
                user.StateId = userDto.StateId;
                user.CityId = userDto.CityId;
                user.PostalCode = userDto.PostalCode;
                user.Address1 = userDto.Address1;
                user.Address2 = userDto.Address2;
                user.DateofBirth = DateTime.Parse(userDto.DateofBirth);
                user.PhoneNumber = userDto.PhoneNumber;
                user.SecondaryPhoneNumber = userDto.SecondaryPhoneNumber;
                user.UpdatedBy = user.UserName;
                user.UpdatedDate = DateTime.Now;
                IdentityResponseValidation(await _userManager.UpdateAsync(user), response);
            }
            return response;
        }

        public async Task<ResponseObject<List<Error>>> ChangeUserPassword(ChangePasswordDto changePasswordDto)
        {
            var response = new ResponseObject<List<Error>>();

            if (ChangePasswordValidation(changePasswordDto, response))
            {
                IdentityResponseValidation(await _userManager.ChangePasswordAsync(await GetUserById(changePasswordDto.UserId), changePasswordDto.OldPassword, changePasswordDto.NewPassword), response);
            }
            return response;
        }

        public async Task<ResponseObject<List<Error>>> LockOutUser(LockOutUserDto lockOutUser)
        {
            var response = new ResponseObject<List<Error>>();
            var user = await LockOutValidation(lockOutUser, response);

            if (user != null)
            {
                if (IdentityResponseValidation(await _userManager.SetLockoutEndDateAsync(user, DateTime.Now.AddYears(10)), response))
                {
                    user.UpdatedDate = DateTime.Now;
                    user.UpdatedBy = lockOutUser.UpdateByUser;
                    await _userManager.UpdateAsync(user);
                }
            }
            return response;
        }

        public async Task<ResponseObject<List<Error>>> UnlockUser(LockOutUserDto lockOutUser)
        {
            var response = new ResponseObject<List<Error>>();
            var user = await LockOutValidation(lockOutUser, response);

            if (user != null)
            {
                if (IdentityResponseValidation(await _userManager.SetLockoutEndDateAsync(user, DateTime.Now.AddMinutes(1)), response))
                {
                    user.UpdatedDate = DateTime.Now;
                    user.UpdatedBy = lockOutUser.UpdateByUser;
                    await _userManager.UpdateAsync(user);
                }
            }
            return response;
        }

    
    }
}
