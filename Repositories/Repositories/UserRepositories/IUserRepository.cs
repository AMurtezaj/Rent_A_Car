using Data.DTOs.UserDtos;
using Data.Entities;
using Repositories.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.UserRepositories
{
    public interface IUserRepository :IRepository<User>
    {
        User GetUserById(string id);
        User GetUserByEmail(string email);
        User GetUserByVerificationToken(string token);
        User GetUserByEmailVerified(string email);
        IList<UserDto> GetAllUsersForAdminDashboard();
        UserUpdateDto GetUserByIdForUpdate(string id);
        UserDto GetUserByIdIncludeRole(string id);
        UserForDashboardDto GetUserStatistics();

    }
}
