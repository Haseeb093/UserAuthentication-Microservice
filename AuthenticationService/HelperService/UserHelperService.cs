﻿using AutoMapper;
using Domain.CustomModels;
using Domain.Enum;
using Domain.Helper;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.ApplicationContext;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace Service.Validation
{
    public class UserHelperService
    {
        private readonly IConfiguration _configuration;
        protected readonly ApplicationDbContext _dbContext;
        protected readonly RoleManager<IdentityRole> _roleManager;
        protected readonly UserManager<IdentityUser> _userManager;
        protected readonly SignInManager<IdentityUser> _signInManager;

        public UserHelperService(IConfiguration configuration, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        protected async Task<Users> LoginValidation(LoginDto loginDto, ResponseObject<TokenDto> responseObject)
        {
            if (loginDto != null && !string.IsNullOrEmpty(loginDto.UserName) && loginDto.UserName.Trim() != ""
                                      && !string.IsNullOrEmpty(loginDto.Password) && loginDto.Password.Trim() != "")
            {
                var user = await GetUserByName(loginDto.UserName);

                if (user != null)
                {
                    var login = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: true);

                    if (!login.Succeeded)
                    {
                        if (login.IsLockedOut)
                            responseObject.Message = "User Locked Out";
                        else
                            responseObject.Message = "User Name or Password not Matched";
                    }
                    else
                    {
                        Helper.SetSuccessRespose(responseObject);
                        return user;
                    }
                }
                else
                {
                    responseObject.Message = "User Name or Password not Matched";
                }
            }
            else
            {
                responseObject.Message = "Please Provided User Name or Password";
            }
            return null;
        }

        protected async Task<bool> RegisterValidation(UserDto registerParam, ResponseObject<List<Error>> response, bool isPatient = false)
        {
            bool isValid = true;
            response.Data = new List<Error>();

            if (string.IsNullOrEmpty(registerParam.FirstName) || string.IsNullOrEmpty(registerParam.LastName) || registerParam.FirstName.Trim() == "" || registerParam.LastName.Trim() == "")
            {
                isValid = false;
                Error error = new Error();
                error.Code = "FirstNameNotValid";
                error.Message = "First or Last Name is Missing";
                response.Data.Add(error);
            }

            if (string.IsNullOrEmpty(registerParam.UserName) || registerParam.UserName.Trim() == "")
            {
                isValid = false;
                Error error = new Error();
                error.Code = "UsernameNotValid";
                error.Message = "Username is Missing";
                response.Data.Add(error);
            }
            else
            {
                if (await IsUserExist(registerParam.UserName))
                {
                    isValid = false;
                    Error error = new Error();
                    error.Code = "UserExist";
                    error.Message = "User Name Already Exist";
                    response.Data.Add(error);
                }
            }

            if (string.IsNullOrEmpty(registerParam.Email) || registerParam.Email.Trim() == "" | !ValidateEmail(registerParam.Email))
            {
                isValid = false;
                Error error = new Error();
                error.Code = "EmailNotValid";
                error.Message = "Enter Valid Email Address";
                response.Data.Add(error);
            }
            else
            {
                if (await IsEmailExist(registerParam.Email))
                {
                    isValid = false;
                    Error error = new Error();
                    error.Code = "EmailExist";
                    error.Message = "User Email Already Exist";
                    response.Data.Add(error);
                }
            }

            if (string.IsNullOrEmpty(registerParam.PhoneNumber) || registerParam.PhoneNumber.Trim() == "")
            {
                isValid = false;
                Error error = new Error();
                error.Code = "PhoneNumberNotValid";
                error.Message = "Phone Number is Missing";
                response.Data.Add(error);
            }

            if (registerParam.GenderId <= 0)
            {
                isValid = false;
                Error error = new Error();
                error.Code = "GenderInValid";
                error.Message = "Please Select Gender";
                response.Data.Add(error);
            }

            if ((registerParam.Role == Roles.Doctor.ToString() || registerParam.Role == Roles.Staff.ToString()) && registerParam.DepartmentId <= 0)
            {
                isValid = false;
                Error error = new Error();
                error.Code = "DepartmentNotValid";
                error.Message = "Please Select Department";
                response.Data.Add(error);
            }

            if ((registerParam.DateofBirth == null || DateOnly.Parse(registerParam.DateofBirth) == default(DateOnly)))
            {
                isValid = false;
                Error error = new Error();
                error.Code = "DateofBirthInValid";
                error.Message = "Please Select Date of Birth";
                response.Data.Add(error);
            }

            if (registerParam.CountryId <= 0 && !isPatient)
            {
                isValid = false;
                Error error = new Error();
                error.Code = "CountryIdInvalid";
                error.Message = "Please Select Country";
                response.Data.Add(error);
            }

            if (registerParam.StateId <= 0 && !isPatient)
            {
                isValid = false;
                Error error = new Error();
                error.Code = "StateIdInvalid";
                error.Message = "Please Select State";
                response.Data.Add(error);
            }

            if (registerParam.CityId <= 0 && !isPatient)
            {
                isValid = false;
                Error error = new Error();
                error.Code = "CityIdInvalid";
                error.Message = "Please Select City";
                response.Data.Add(error);
            }

            if (!isValid)
                Helper.SetFailuerRespose(response);

            return isValid;
        }

        protected async Task<bool> UpdateUserValidation(UserDto registerParam, ResponseObject<List<Error>> response)
        {
            bool isValid = true;
            response.Data = new List<Error>();

            if (string.IsNullOrEmpty(registerParam.FirstName) || string.IsNullOrEmpty(registerParam.LastName) || registerParam.FirstName.Trim() == "" || registerParam.LastName.Trim() == "")
            {
                isValid = false;
                Error error = new Error();
                error.Code = "FirstNameNotValid";
                error.Message = "First or Last Name is Missing";
                response.Data.Add(error);
            }

            if (string.IsNullOrEmpty(registerParam.Email) || registerParam.Email.Trim() == "" | !ValidateEmail(registerParam.Email))
            {
                isValid = false;
                Error error = new Error();
                error.Code = "EmailNotValid";
                error.Message = "Enter Valid Email Address";
                response.Data.Add(error);
            }
            else
            {
                if (await IsEmailExist(registerParam.Email, registerParam.Id))
                {
                    isValid = false;
                    Error error = new Error();
                    error.Code = "EmailAlreadyExists";
                    error.Message = "User Email Already Exists";
                    response.Data.Add(error);
                }
            }

            if (string.IsNullOrEmpty(registerParam.PhoneNumber) || registerParam.PhoneNumber.Trim() == "")
            {
                isValid = false;
                Error error = new Error();
                error.Code = "PhoneNumberNotValid";
                error.Message = "Phone Number is Missing";
                response.Data.Add(error);
            }

            if (registerParam.GenderId <= 0)
            {
                isValid = false;
                Error error = new Error();
                error.Code = "GenderInValid";
                error.Message = "Please Select Gender";
                response.Data.Add(error);
            }

            if (registerParam.DateofBirth == null || DateOnly.Parse(registerParam.DateofBirth) == default(DateOnly))
            {
                isValid = false;
                Error error = new Error();
                error.Code = "DateofBirthInValid";
                error.Message = "Please Select Date of Birth";
                response.Data.Add(error);
            }

            if (registerParam.CountryId <= 0 && registerParam.Role != Roles.Patient.ToString())
            {
                isValid = false;
                Error error = new Error();
                error.Code = "CountryIdInvalid";
                error.Message = "Please Select Country";
                response.Data.Add(error);
            }

            if (registerParam.StateId <= 0 && registerParam.Role != Roles.Patient.ToString())
            {
                isValid = false;
                Error error = new Error();
                error.Code = "StateIdInvalid";
                error.Message = "Please Select State";
                response.Data.Add(error);
            }

            if (registerParam.CityId <= 0 && registerParam.Role != Roles.Patient.ToString())
            {
                isValid = false;
                Error error = new Error();
                error.Code = "CityIdInvalid";
                error.Message = "Please Select City";
                response.Data.Add(error);
            }

            if (!isValid)
                Helper.SetFailuerRespose(response);

            return isValid;
        }

        protected bool IdentityResponseValidation(IdentityResult identityResult, ResponseObject<List<Error>> responseObject)
        {
            if (responseObject.Data == null)
                responseObject.Data = new List<Error>();

            if (!identityResult.Succeeded)
            {
                foreach (var err in identityResult.Errors)
                {
                    Error error = new Error();
                    error.Code = err.Code;
                    error.Message = err.Description;
                    responseObject.Data.Add(error);
                }
                Helper.SetFailuerRespose(responseObject);
            }
            else
            {
                Helper.SetSuccessRespose(responseObject);
            }
            return identityResult.Succeeded;
        }

        protected bool ChangePasswordValidation(ChangePasswordDto passwordDto, ResponseObject<List<Error>> responseObject)
        {

            if (passwordDto == null || string.IsNullOrEmpty(passwordDto.OldPassword) || passwordDto.OldPassword.Trim() == ""
                                    || string.IsNullOrEmpty(passwordDto.NewPassword) || passwordDto.NewPassword.Trim() == "")
            {
                responseObject.Message = "Old or New Password Not Provided !";
                return false;
            }

            return true;
        }

        protected async Task<Users> LockOutValidation(LockOutUserDto lockOutUser, ResponseObject<List<Error>> responseObject)
        {
            if (lockOutUser != null && !string.IsNullOrEmpty(lockOutUser.UserId))
            {
                var user = await GetUserById(Helper.DecryptString(lockOutUser.UserId));

                if (user == null)
                    responseObject.Message = "User not Found";
                else
                    return user;
            }
            else
            {
                responseObject.Message = "Please Provided User Id";
            }
            return null;
        }

        protected async Task<Users> GetUserById(string userId)
        {
            return await _userManager.FindByIdAsync(userId) as Users;
        }

        protected async Task<Users> GetUserByName(string userName)
        {
            return await _userManager.FindByNameAsync(userName) as Users;
        }

        protected TokenDto GetData(List<Claim> authClaims, Users user, IList<string> roles)
        {
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])), SecurityAlgorithms.HmacSha256)
                );
            return new TokenDto()
            {
                ValidTo = token.ValidTo,
                Roles = roles,
                UserName = user.UserName,
                FullName = user.FirstName + " " + user.LastName,
                Email = user.Email,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }

        #region Private Method
        private bool ValidateEmail(string email)
        {
            string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";
            return Regex.IsMatch(email, regex);
        }
        private async Task<bool> IsUserExist(string userName)
        {
            return await _userManager.Users.AnyAsync(s => s.UserName == userName);
        }

        private async Task<bool> IsEmailExist(string email, string userId = "")
        {
            if (userId == "")
                return await _userManager.Users.AnyAsync(s => s.Email == email);
            else
                return await _userManager.Users.AnyAsync(s => s.Email == email && s.Id != userId);
        }

        #endregion
    }
}
