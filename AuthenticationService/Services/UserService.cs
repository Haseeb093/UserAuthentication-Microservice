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
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace AuthenticationService
{
    public class UserService : HelperService, IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(IConfiguration configuration, IMapper mapper, ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager) : base(configuration, userManager, signInManager, mapper)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
        }

        public async Task<ResponseObject<TokenDto>> Login(LoginDto loginParam)
        {
            var response = new ResponseObject<TokenDto>();

            if (await LoginValidation(loginParam, response))
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

        public async Task<ResponseObject<List<Error>>> Register(UserDto userDto, string userName)
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
                    InsertedBy = userName,
                    UpdatedBy = userName
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

        public async Task<ResponseObject<List<Error>>> ChangeUserPassword(ChangePasswordDto changePasswordDto)
        {
            var response = new ResponseObject<List<Error>>();

            if (ChangePasswordValidation(changePasswordDto, response))
            {
                var changeResponse = await _userManager.ChangePasswordAsync(await GetUserByName(changePasswordDto.UserName), changePasswordDto.OldPassword, changePasswordDto.NewPassword);
                IdentityResponseValidation(changeResponse, response);
            }
            return response;
        }

        public async Task<ResponseObject<List<Error>>> LockOutUser(LockOutUserDto lockOutUser)
        {
            var response = new ResponseObject<List<Error>>();
            if (await LockOutValidation(lockOutUser, response))
            {
                if (IdentityResponseValidation(await _userManager.SetLockoutEndDateAsync(user, DateTime.Now.AddYears(10)), response))
                {
                    user.UpdatedBy = lockOutUser.UserName;
                    await _userManager.UpdateAsync(user);
                }
            }
            return response;
        }

        public async Task<ResponseObject<List<Error>>> UnlockUser(LockOutUserDto lockOutUser)
        {
            var response = new ResponseObject<List<Error>>();
            if (await LockOutValidation(lockOutUser, response))
            {
                if (IdentityResponseValidation(await _userManager.SetLockoutEndDateAsync(user, DateTime.Now.AddMinutes(1)), response))
                {
                    user.UpdatedBy = lockOutUser.UserName;
                    await _userManager.UpdateAsync(user);
                }
            }
            return response;
        }

        public async Task<ResponseObject<List<UserDto>>> GetAllDoctors()
        {
            var response = new ResponseObject<List<UserDto>>();
            //var ss1 = await _userManager.Users.ToListAsync();
            //var x = from U in Users
            //        join R in Roles on equals userRole.UserId
            //        join role in t.Roles on userRole.RoleId equals role.Id into roles
            //        select new
            //        {
            //            User = usr,
            //            Roles = roles
            //        };

            response.Data = _mapper.Map<List<UserDto>>(await _userManager.Users.ToListAsync());
            Helper.SetSuccessRespose(response);
            return response;
        }

        public async Task<ResponseObject<List<UserDto>>> GetAllPatients()
        {
            var response = new ResponseObject<List<UserDto>>();
            response.Data = _mapper.Map<List<UserDto>>(await _dbContext.Users.ToListAsync());
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
