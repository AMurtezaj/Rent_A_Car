using Data.DTOs.UserDtos;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.UserServices
{
    public interface IUserService
    {
        ApiResponse<string> DeleteUser(string id);
        ApiResponse<UserUpdateDto> GetUserByIdForUpdate(string id);
        ApiResponse<UserDto> EditUser(UserUpdateDto user);
        ApiResponse<UserDto> GetUser(string id);
        ApiResponse<IList<UserDto>> GetAll();
        ApiResponse<string> ForgotPassword(ForgetPasswordDto forgetPassword);

    }
}
