using Data.Authentication;
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
        ApiResponse<ForgotPasswordEmailResponseDto> SendForgotPasswordEmail(EmailSendDto email);
        byte[] generateRandomKeyNumber();
        byte[] generateRandomIvNumber();
        ApiResponse<UserLogInResponseDto> LogIn(UserLoginDto user);
        ApiResponse<string> VerifyEmail(string token);
        ApiResponse<UserDto> GetCurrentUser(string token);
        ApiResponse<string> ChangePassword(ChangePasswordDto changePassword);
        ApiResponse<IList<UserDto>> GetAllUsersForAdminDashboard(UserForDashboardDto userForDashboard);
        ApiResponse<string> SignUp(UserCreateDto user);
    }
}
