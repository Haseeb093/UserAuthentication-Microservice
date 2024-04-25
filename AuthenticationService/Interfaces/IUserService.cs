using Domain.CustomModels;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IUserService
    {
        public Task<ResponseObject<TokenDto>> Login(LoginDto loginParam);
        public Task<ResponseObject<List<Error>>> UpdateUser(UserDto userParams);
        public Task<ResponseObject<List<Error>>> UnlockUser(LockOutUserDto lockOutUser);
        public Task<ResponseObject<List<Error>>> LockOutUser(LockOutUserDto lockOutUser);
        public Task<ResponseObject<List<Error>>> Register(UserDto registerParam, string userName);
        public Task<ResponseObject<List<Error>>> ChangeUserPassword(ChangePasswordDto changePasswordDto);

    }
}
