﻿using AutoMapper;
using Azure.Storage.Blobs.Models;
using Business.Services.AuthenticationServices;
using Business.Services.Mailing;
using Business.Services.Token;
using Data.Authentication;
using Data.DTOs.UserDtos;
using Data.Entities;
using Repositories.Repositories.UserRepositories;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.UserServices
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IMailService _mailService;
        private readonly IAuthenticationService _authenticationService;
        

        public UserService(IUserRepository userRepository, IMapper mapper, ITokenService tokenService, IMailService mailService, IAuthenticationService authenticationService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenService = tokenService;
            _mailService = mailService;
            _authenticationService = authenticationService;
        }

        public ApiResponse<IList<UserDto>> GetAll()
        {
            try
            {
                var users = _userRepository.GetAll();
                return new ApiResponse<IList<UserDto>>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = _mapper.Map<IList<UserDto>>(users)
                };
            }
            catch (Exception e)
            {
                Log.Error(e.Message, "An error occurred: {ErrorMessage}", e.Message);
                return new ApiResponse<IList<UserDto>>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<string> { "An error occurred while processing your request. Please try again later." }
                };

            }   
        }

        public ApiResponse<UserDto> GetUser(string id)
        {
            try
            {
                var user = _userRepository.GetUserById(id);
                if (user == null)
                {
                    return new ApiResponse<UserDto>()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Errors = new List<string>() { "The user doesn't exist" }
                    };
                
                }
                return new ApiResponse<UserDto>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = _mapper.Map<UserDto>(user)
                };
            }
            catch (Exception e)
            {
                Log.Error(e.Message, "An error occurred: {ErrorMessage}", e.Message);
                return new ApiResponse<UserDto>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<string> { "An error occurred while processing your request. Please try again later." }
                };
            }

        }

        public ApiResponse<UserDto> EditUser(UserUpdateDto user)
        {
            try
            {
                var userInDb = _userRepository.GetUserById(user.Id);
                if (userInDb == null)
                {
                    return new ApiResponse<UserDto>()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Errors = new List<string>() { "The user doesn't exist" }
                    };

                }

                _mapper.Map(user, userInDb);
                if (_userRepository.Update(userInDb))
                {
                    return new ApiResponse<UserDto>()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Data = _mapper.Map<UserDto>(userInDb)
                    };
                
                }
                return new ApiResponse<UserDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = new List<string>() { "Something went wrong while updating the user" }
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, "An error ocurred: {ErrorMessage}", ex.Message);
                return new ApiResponse<UserDto>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<string> { "An error occurred while processing your request. Please try again" }
                };
            }
        
        }

        public ApiResponse<UserUpdateDto> GetUserByIdForUpdate(string id)
        {
            try
            {
                var user = _userRepository.GetUserByIdForUpdate(id);
                if (user == null)
                {
                    return new ApiResponse<UserUpdateDto>()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Errors = new List<string>() { "The user doesn't exist" }
                    };
                }
                return new ApiResponse<UserUpdateDto>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = user
                };

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, "An error occurred: {ErrorMessage}", ex.Message);
                return new ApiResponse<UserUpdateDto>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<string> { "An error occurred while processing your request" }
                };
            }
        
        }

        public ApiResponse<string> DeleteUser(string id)
        {
            try
            {
                var user = _userRepository.GetUserById(id);
                if (user == null)
                {
                    return new ApiResponse<string>()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Errors = new List<string>() { " The user doesn't exist" }
                    };
                
                }

                if (_userRepository.Remove(user))
                {

                    return new ApiResponse<string>()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "The user was deleted successfully"
                    };
                }

                return new ApiResponse<string>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = new List<string>() { "Something went wrong while deleting the user" }
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, "An error occurred: {ErrorMessage}", ex.Message);
                return new ApiResponse<string>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<string> { "An error occurred while processing your request. Please try again" }
                };
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                //Convert the password string to a byte array
                byte[] bytes = Encoding.UTF8.GetBytes(password);

                //Compute the hash of the byte array
                byte[] hash = sha256Hash.ComputeHash(bytes);

                //Convert the hash byte array to a string
                StringBuilder sb = new StringBuilder();
                for(int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }


        public ApiResponse<UserForDashboardDto> GetUserStatisticsForDashboard()
        {

            try
            {
                return new ApiResponse<UserForDashboardDto>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = _userRepository.GetUserStatistics()
                };   
            }
            catch (Exception e)
            {
                Log.Error(e.Message, "An error occurred: {ErrorMessage}", e.Message);
                return new ApiResponse<UserForDashboardDto>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<string> { "An error occurred while processing your request" }
                };
            }

        }

        
        public ApiResponse<string> SignUp(UserCreateDto user)
        {
            try
            {
                var mappedUser = _mapper.Map<User>(user);
                mappedUser.Password = HashPassword(mappedUser.Password);
                mappedUser.AccountVerificationToken = _tokenService.CreateVerifyAccountToken(user);
                user.AccountVerificationToken = mappedUser.AccountVerificationToken;
                //mappedUser.RoleId = _rolesRepository.FindDefaultCustomerRole();
                if (_userRepository.Add(mappedUser))
                {
                    //_cartRepository.CreateCart(mappedUser.Id);
                    _mailService.SendVerifyAccountEmail(user);

                    return new ApiResponse<string>()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Data = mappedUser.AccountVerificationToken
                    };
                }
                return new ApiResponse<string>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = new List<string>() { "Something went wrong while registering the user" }
                };
            }
            catch (Exception e)
            {
                Log.Error(e.Message, "An error occurred: {ErrorMessage}", e.Message);
                return new ApiResponse<string>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<string> { "An error occurred while processing your request. Please try again" }
                };


            }
        } 

        public ApiResponse<string> ForgotPassword(ForgetPasswordDto forgetPassword)
        {
            try
            {
                var userInDb = _userRepository.GetUserById(forgetPassword.UserId);
                if (userInDb == null)
                {
                    return new ApiResponse<string>()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Errors = new List<string>() { "The user doesn't exist" }
                    };
                }

                if (forgetPassword.NewPassword == forgetPassword.RepeatPassword)
                { 
                    userInDb.Password = HashPassword(forgetPassword.NewPassword);
                    if (_userRepository.Update(userInDb))
                    {
                        return new ApiResponse<string>()
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = "The password was changed successfully"
                        };
                    
                    }
                    return new ApiResponse<string>()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "Something went wrong while updating the data"
                    };

                }

                return new ApiResponse<string>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = new List<string>() { "The password doesn't match" }
                };

            }
            catch (Exception e)
            {
                Log.Error(e.Message, "An error occurred: {ErrorMessage}", e.Message);
                return new ApiResponse<string>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<string> { "An error occurred while processing your request. Please try again" }
                };
            
            }
        
        }

        public ApiResponse<ForgotPasswordEmailResponseDto> SendForgotPasswordEmail(EmailSendDto email)
        {
            try
            {
                var userInDb = _userRepository.GetUserByEmail(email.Email);


                if (userInDb != null)
                {
                    var token = _tokenService.CreatePasswordToken(userInDb.Email);
                    var key = generateRandomKeyNumber();
                    var iv = generateRandomIvNumber();
                    var encryptedToken = _authenticationService.EncryptString(token, key, iv);
                    _mailService.SendForgotPasswordEmail(userInDb, token);

                    return new ApiResponse<ForgotPasswordEmailResponseDto>()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Data = new ForgotPasswordEmailResponseDto()
                        {
                            EncryptedToken = encryptedToken,
                            Key = key,
                            Iv = iv,
                            UserId = userInDb.Id
                        }
                    };
                }
                return new ApiResponse<ForgotPasswordEmailResponseDto>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = new List<string> { "Something went wrong while sending the email " }
                };
           
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message, "An error occurred: {ErrorMessage}", ex.Message);
                return new ApiResponse<ForgotPasswordEmailResponseDto>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<string> { "An error occurred while processing your request. Please try again later." }
                };

            }
        
        }

        private static byte[] generateRandomKeyNumber()
        { 
            byte[] key = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(key);
            }
            return key;
        }

        private static byte[] generateRandomIvNumber()
        {
            byte[] iv = new byte[16];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(iv);
            
            }
            return iv;
        }


        public ApiResponse<UserLogInResponseDto> LogIn(UserLoginDto user)
        {
            try
            {
                var userExists = _userRepository.GetUserByEmailVerified(user.Email);
                if (userExists != null && userExists.Password.Equals(HashPassword(user.Password))) 
                {
                    return new ApiResponse<UserLogInResponseDto>()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Data = new UserLogInResponseDto
                        {
                            Id = userExists.Id,
                            Role = userExists.Role.Name,
                            Email = userExists.Email,
                            Token = _tokenService.CreateToken(userExists),
                        }
                    };
                
                }
                return new ApiResponse<UserLogInResponseDto>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = new List<string>() { "The credentials are incorrect" }
                };

            }
            catch (Exception e)
            {
                Log.Information("This is an informational message");
                Log.Error(e.Message, "An error occurred: {ErrorMessage}", e.Message);

                return new ApiResponse<UserLogInResponseDto>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<string> { "An error occurred while processing your request. Please try again" }
                };
            }


        }

        public ApiResponse<string> VerifyEmail(string token)
        {
            try
            {
                var userInDb = _userRepository.GetUserByVerificationToken(token);
                if (userInDb != null)
                {
                    userInDb.IsEmailVerified = true;
                    userInDb.AccountVerificationToken = "";
                    if (_userRepository.Update(userInDb))
                    {
                        return new ApiResponse<string>()
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = "The email was verified successfully"
                        };

                    }

                    else
                    {
                        return new ApiResponse<string>()
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            Errors = new List<string>() { "The account was not verified" }
                        };

                    }
                }
                return new ApiResponse<string>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = new List<string>() { "The account was not verified" }
                };

            }
            catch (Exception ex)
            {
                Log.Information("This is an informational message");
                Log.Error(ex.Message, "An error occurred: {ErrorMessage}", ex.Message);
                return new ApiResponse<string>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<string> { "An error occurred while processing your request. Please try again" }

                };
            }
        }

        public ApiResponse<UserDto> GetCurrentUser(string token)
        {
            try
            {
                var userId = _tokenService.DecodeToken(token);
                var userInDb = _userRepository.GetUserByIdIncludeRole(userId);
                if (userInDb == null)
                {
                    return new ApiResponse<UserDto>()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Errors = new List<string>() { "Please log in first" }
                    };

                }
                return new ApiResponse<UserDto>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = userInDb
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while processing the ChangePassword request.");

                return new ApiResponse<UserDto>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<string>() { "An error occurred while processing your request. Please try again later." }
                };

            }
        
        }

        public ApiResponse<string> ChangePassword(ChangePasswordDto changePassword)
        {
            try
            {
                var userInDb = _userRepository.GetUserById(changePassword.UserId);

                if (userInDb == null)
                {
                    return new ApiResponse<string>()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Errors = new List<string>() { "Something went wrong" }
                    };
                
                }
                if (changePassword.NewPassword != changePassword.RepeatPassword)
                {
                    return new ApiResponse<string>()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Errors = new List<string>() { "The passwords do not match" }
                    };
                }
                if (userInDb.Password != HashPassword(changePassword.CurrentPassword))
                {
                    return new ApiResponse<string>()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Errors = new List<string>() { "The curremt password is wrong" }
                    };
                }

                userInDb.Password = HashPassword(changePassword.NewPassword);
                if (_userRepository.Update(userInDb))
                {
                    return new ApiResponse<string>()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "The password was changed successfully"
                    };

                }

                return new ApiResponse<string>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = new List<string>() { "The password was not verified" }
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while processing the ChangePassword request.");

                return new ApiResponse<string>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<string>() { "An error occurred while processing your request. Please try again later." }
                };

            }
        }

        public ApiResponse<IList<UserDto>> GetAllUsersForAdminDashboard(UserForDashboardDto userForDashboard)
        {
            try
            {
                var users = _userRepository.GetAllUsersForAdminDashboard();
                return new ApiResponse<IList<UserDto>>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = users
                };


            }
            catch(Exception ex)
            {
                Log.Error(ex, "An error occurred while processing the GetAllUsersForAdminDashboardDisplay request.");

                return new ApiResponse<IList<UserDto>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<string>() { "An error occurred while processing your request. Please try again later." }
                };
            }
        }



    }
}
