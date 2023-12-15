using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService
{
    public interface IUserService
    {
        public Task<ResponseObject<LoginResponse>> Login(LoginParam loginParam);
        public Task<ResponseObject<List<Error>>> Register(RegisterParam registerParam);
    }
}
