using AutoMapper;
using Domain.CustomModels;
using Domain.Enum;
using Domain.Helper;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Service.Services;
using Service.Validation;
using Services.ApplicationContext;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace AuthenticationService
{
    public class UserService : HelperService, IUserService
    {


        public UserService(IConfiguration configuration, IMapper mapper, ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager) : base(configuration, userManager, roleManager, signInManager, mapper, dbContext)
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
                    DateofBirth = DateOnly.Parse(userDto.DateofBirth),
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
                user.DateofBirth = DateOnly.Parse(userDto.DateofBirth);
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

        public async Task<ResponseObject<List<UserDto>>> GetAllDoctors()
        {
            var response = new ResponseObject<List<UserDto>>();
            response.Data = await (from user in _userManager.Users
                                   join userRoles in _dbContext.UserRoles on user.Id equals userRoles.UserId
                                   join roles in _dbContext.Roles on userRoles.RoleId equals roles.Id
                                   join gender in _dbContext.Genders on (user as Users).GenderId equals gender.GenderId
                                   join department in _dbContext.Departments on (user as Users).DepartmentId equals department.DepartmentId
                                   join country in _dbContext.Countries on (user as Users).CountryId equals country.CountryId
                                   join state in _dbContext.States on (user as Users).StateId equals state.StateId
                                   join city in _dbContext.Cities on (user as Users).CityId equals city.CityId
                                   where roles.Name == Roles.Doctor.ToString()
                                   select new UserDto
                                   {
                                       Id = Helper.EncryptString(user.Id),
                                       FirstName = (user as Users).FirstName,
                                       LastName = (user as Users).LastName,
                                       UserName = user.UserName,
                                       Email = user.Email,
                                       Role = roles.Name,
                                       GenderId = gender.GenderId,
                                       Gender = gender.Name,
                                       DateofBirth = (user as Users).DateofBirth.ToString(),
                                       Address1 = (user as Users).Address1,
                                       Address2 = (user as Users).Address2,
                                       PostalCode = (user as Users).PostalCode,
                                       PhoneNumber = (user as Users).PhoneNumber,
                                       SecondaryPhoneNumber = (user as Users).SecondaryPhoneNumber,
                                       DepartmentId = department.DepartmentId,
                                       Department = department.Name,
                                       CountryId = country.CountryId,
                                       Country = country.Name,
                                       StateId = state.StateId,
                                       State = state.Name,
                                       CityId = city.CityId,
                                       City = city.Name
                                   }).ToListAsync();
            Helper.SetSuccessRespose(response);
            return response;
        }

        public async Task<ResponseObject<List<UserDto>>> GetAllPatients()
        {
            var response = new ResponseObject<List<UserDto>>();
            response.Data = await (from user in _userManager.Users
                                   join userRoles in _dbContext.UserRoles on user.Id equals userRoles.UserId
                                   join roles in _dbContext.Roles on userRoles.RoleId equals roles.Id
                                   join gender in _dbContext.Genders on (user as Users).GenderId equals gender.GenderId
                                   join department in _dbContext.Departments on (user as Users).DepartmentId equals department.DepartmentId
                                   join country in _dbContext.Countries on (user as Users).CountryId equals country.CountryId
                                   join state in _dbContext.States on (user as Users).StateId equals state.StateId
                                   join city in _dbContext.Cities on (user as Users).CityId equals city.CityId
                                   where roles.Name == Roles.Patient.ToString()
                                   select new UserDto
                                   {
                                       Id = Helper.EncryptString(user.Id),
                                       FirstName = (user as Users).FirstName,
                                       LastName = (user as Users).LastName,
                                       UserName = user.UserName,
                                       Email = user.Email,
                                       Role = roles.Name,
                                       GenderId = gender.GenderId,
                                       Gender = gender.Name,
                                       DateofBirth = (user as Users).DateofBirth.ToString(),
                                       Address1 = (user as Users).Address1,
                                       Address2 = (user as Users).Address2,
                                       PostalCode = (user as Users).PostalCode,
                                       PhoneNumber = (user as Users).PhoneNumber,
                                       SecondaryPhoneNumber = (user as Users).SecondaryPhoneNumber,
                                       DepartmentId = department.DepartmentId,
                                       Department = department.Name,
                                       CountryId = country.CountryId,
                                       Country = country.Name,
                                       StateId = state.StateId,
                                       State = state.Name,
                                       CityId = city.CityId,
                                       City = city.Name
                                   }).ToListAsync();
            Helper.SetSuccessRespose(response);
            return response;
        }

        public async Task<ResponseObject<List<CountriesDto>>> GetCountries()
        {
            var response = new ResponseObject<List<CountriesDto>>();
            response.Data = _mapper.Map<List<CountriesDto>>(await _dbContext.Countries.ToListAsync());
            Helper.SetSuccessRespose(response);
            return response;
        }

        public async Task<ResponseObject<List<StatesDto>>> GetCountryStates(int countryId)
        {
            var response = new ResponseObject<List<StatesDto>>();
            response.Data = _mapper.Map<List<StatesDto>>(await _dbContext.States.Where(s => s.Countries.CountryId == countryId).ToListAsync());
            Helper.SetSuccessRespose(response);
            return response;
        }

        public async Task<ResponseObject<List<CitiesDto>>> GetStateCities(int stateId)
        {
            var response = new ResponseObject<List<CitiesDto>>();
            response.Data = _mapper.Map<List<CitiesDto>>(await _dbContext.Cities.Where(s => s.States.StateId == stateId).ToListAsync());
            Helper.SetSuccessRespose(response);
            return response;
        }

    }
}
