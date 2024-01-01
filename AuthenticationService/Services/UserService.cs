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
        private readonly IMapper mapper;
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(IConfiguration configuration, IMapper _mapper, ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : base(configuration, userManager)
        {
            mapper = _mapper;
            _dbContext = dbContext;
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
                    new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim("Roles", userRole));
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

                    if (!await _roleManager.RoleExistsAsync(UserRoles.Doctor.ToString()))
                        await _roleManager.CreateAsync(new IdentityRole(UserRoles.Doctor.ToString()));

                    if (!await _roleManager.RoleExistsAsync(UserRoles.Patient.ToString()))
                        await _roleManager.CreateAsync(new IdentityRole(UserRoles.Patient.ToString()));

                    if (!await _roleManager.RoleExistsAsync(UserRoles.User.ToString()))
                        await _roleManager.CreateAsync(new IdentityRole(UserRoles.User.ToString()));

                    switch (registerParam.Role)
                    {
                        case "Admin":
                            await _userManager.AddToRoleAsync(user, UserRoles.Admin.ToString());
                            break;
                        case "Doctor":
                            await _userManager.AddToRoleAsync(user, UserRoles.Doctor.ToString());
                            break;
                        case "Patient":
                            await _userManager.AddToRoleAsync(user, UserRoles.Patient.ToString());
                            break;
                        default:
                            await _userManager.AddToRoleAsync(user, UserRoles.User.ToString());
                            break;
                    }

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

        public async Task<ResponseObject<List<CountriesDto>>> GetCountries()
        {
            var response = new ResponseObject<List<CountriesDto>>();
            response.Data = mapper.Map<List<CountriesDto>>(await _dbContext.Countries.ToListAsync());
            Helper.SetSuccessRespose(response);
            return response;
        }

        public async Task<ResponseObject<List<CitiesDto>>> GetStateCities(int stateId)
        {
            var response = new ResponseObject<List<CitiesDto>>();
            response.Data = mapper.Map<List<CitiesDto>>(await _dbContext.Cities.Where(s => s.States.StateId == stateId).ToListAsync());
            Helper.SetSuccessRespose(response);
            return response;
        }

        public async Task<ResponseObject<List<StatesDto>>> GetCountryStates(int countryId)
        {
            var response = new ResponseObject<List<StatesDto>>();
            response.Data = mapper.Map<List<StatesDto>>(await _dbContext.States.Where(s => s.Countries.CountryId == countryId).ToListAsync());
            Helper.SetSuccessRespose(response);
            return response;
        }
    }
}
