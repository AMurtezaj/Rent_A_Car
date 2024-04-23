using Data.DTOs.UserDtos;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.UserRepositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {

        public UserRepository(AppDbContext context) : base(context)
        { 
        
        }

        public User GetUserById(string id)
        {
            return Context.Set<User>().Include(u => u.Role).Where(u => u.Id == id).FirstOrDefault();
        }

        public User GetUserByEmail(string email)
        {
            return Context.Set<User>().Include(u => u.Role).FirstOrDefault(u => u.Email == email);
        }

        public User GetUserByVerificationToken(string token)
        {
            return Context.Set<User>().FirstOrDefault(x => x.AccountVerificationToken == token);
        }

        public User GetUserByEmailVerified(string email)
        {
            return Context.Set<User>().Include(x => x.Role).Where(x => x.IsEmailVerified == true && x.Email == email).FirstOrDefault();
        }

        public IList<UserDto> GetAllUsersForAdminDashboard()
        {
            return Context.Set<User>().Include(x => x.Role).Select(x => new UserDto()
            {
                Id = x.Id,
                Name = x.Name,
                Surname = x.Surname,
                Email = x.Email,
                Password = x.Password,
                Role = x.Role.Name,
                IsEmailVerified = x.IsEmailVerified
            }).ToList();
        }

        public UserUpdateDto GetUserByIdForUpdate(string id)
        {
            return Context.Set<User>().Include(x => x.Role).Select(x => new UserUpdateDto()
            {
                Id = x.Id,
                Name = x.Name,
                Surname = x.Surname,
                Email = x.Email,
                RoleId = x.RoleId,
                IsEmailVerified = x.IsEmailVerified
            }).FirstOrDefault(x => x.Id == id);
        }

        public UserDto GetUserByIdIncludeRole(string id)
        {
            return Context.Set<User>().Include(x => x.Role)
                .Select(x => new UserDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Surname = x.Surname, 
                    Role = x.Role.Name,
                    IsEmailVerified = x.IsEmailVerified
                }).FirstOrDefault(x=>x.Id == id);

        }

        public UserForDashboardDto GetUserStatistics()
        {

            var verifiedUsers = Context.Set<User>().Where(x => x.IsEmailVerified == true).Count();
            var unverifiedUsers = Context.Set<User>().Where(x => x.IsEmailVerified == false).Count();

            var totalUsers = Context.Set<User>().Count();

            return new UserForDashboardDto()
            {
                VerifiedUsers = verifiedUsers,
                UnverifiedUsers = unverifiedUsers,
                TotalUsers = totalUsers
            };
        
        }


        
    }
}
