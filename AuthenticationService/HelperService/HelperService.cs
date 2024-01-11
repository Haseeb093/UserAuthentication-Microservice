﻿using Azure;
using Domain.CustomModels;
using Domain.Helper;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Service.Validation
{
    public class HelperService
    {
        protected IdentityUser user = null;
        private readonly IConfiguration _configuration;
        protected readonly UserManager<IdentityUser> _userManager;
        protected readonly SignInManager<IdentityUser> _signInManager;

        public HelperService(IConfiguration configuration, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        protected async Task<bool> LoginValidation(LoginDto loginDto, ResponseObject<TokenDto> responseObject)
        {
            if (loginDto != null && !string.IsNullOrEmpty(loginDto.Username) && loginDto.Username.Trim() != ""
                                      && !string.IsNullOrEmpty(loginDto.Password) && loginDto.Password.Trim() != "")
            {
                user = await GetUserByName(loginDto.Username);

                if (user != null)
                {
                    var login = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: true);

                    if (!login.Succeeded)
                    {
                        if (login.IsLockedOut)
                            responseObject.Message = "User Locked Out !";
                        else
                            responseObject.Message = "User Name or Password not Matched !";
                    }
                    else
                    {
                        Helper.SetSuccessRespose(responseObject);
                        return true;
                    }
                }
                else
                {
                    responseObject.Message = "User Name or Password not Matched !";
                }
            }
            else
            {
                responseObject.Message = "Please Provided User Name or Password !";
            }
            return false;
        }

        protected async Task<bool> RegisterValidation(UserDto registerParam, ResponseObject<List<Error>> response)
        {
            bool isValid = true;
            response.Data = new List<Error>();

            if (string.IsNullOrEmpty(registerParam.FirstName) || string.IsNullOrEmpty(registerParam.LastName) || registerParam.FirstName.Trim() == "" || registerParam.LastName.Trim() == "")
            {
                isValid = false;
                Error error = new Error();
                error.Code = "FirstNameNotValid";
                error.Message = "First or Last Name is Missing !";
                response.Data.Add(error);
            }

            if (string.IsNullOrEmpty(registerParam.Username) || registerParam.Username.Trim() == "")
            {
                isValid = false;
                Error error = new Error();
                error.Code = "UsernameNotValid";
                error.Message = "Username is Missing !";
                response.Data.Add(error);
            }
            else
            {
                var regUser = await GetUserByName(registerParam.Username);

                if (regUser != null)
                {
                    isValid = false;
                    Error error = new Error();
                    error.Code = "UserExist";
                    error.Message = "User Name Already Exist !";
                    response.Data.Add(error);
                }
            }

            if (string.IsNullOrEmpty(registerParam.Email) || registerParam.Email.Trim() == "" | !ValidateEmail(registerParam.Email))
            {
                isValid = false;
                Error error = new Error();
                error.Code = "EmailNotValid";
                error.Message = "Enter Valid Email Address !";
                response.Data.Add(error);
            }
            else
            {
                var regEmail = await GetUserByEmail(registerParam.Email);

                if (regEmail != null)
                {
                    isValid = false;
                    Error error = new Error();
                    error.Code = "EmailExist";
                    error.Message = "User Email Already Exist !";
                    response.Data.Add(error);
                }
            }

            if (string.IsNullOrEmpty(registerParam.PhoneNumber) || registerParam.PhoneNumber.Trim() == "")
            {
                isValid = false;
                Error error = new Error();
                error.Code = "PhoneNumberNotValid";
                error.Message = "Phone Number is Missing !";
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

            if (registerParam.CountryId <= 0)
            {
                isValid = false;
                Error error = new Error();
                error.Code = "CountryIdInvalid";
                error.Message = "Please Select Country";
                response.Data.Add(error);
            }

            if (registerParam.StateId <= 0)
            {
                isValid = false;
                Error error = new Error();
                error.Code = "StateIdInvalid";
                error.Message = "Please Select State";
                response.Data.Add(error);
            }

            if (registerParam.CityId <= 0)
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

        protected async Task<IdentityUser> GetUserByName(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        protected async Task<IdentityUser> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        protected TokenDto GetToken(List<Claim> authClaims)
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
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }

        #region Private Method
        private bool ValidateEmail(string email)
        {
            string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";
            return Regex.IsMatch(email, regex);
        }

        #endregion
    }
}
