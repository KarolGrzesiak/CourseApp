using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseApp.API.Helpers;
using CourseApp.API.IRepositories;
using CourseApp.API.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.API.Data
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context) : base(context)
        {
           
            _context = context;

        }

        public async Task<User> GetUserAsync(int userId)
        {
            return await _context.Users.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<PagedList<User>> GetUsersAsync(UserParams userParams)
        {
            var users = _context.Users.Include(u => u.Photos).OrderByDescending(u => u.LastActive).AsQueryable();
            var roleId = _context.Roles.Where(r => r.NormalizedName == userParams.Role.ToUpper()).Select(r => r.Id);
            var usersInRole = _context.UserRoles.Where(ur => roleId.Contains(ur.RoleId)).Select(ur => ur.User);

            users = users.Where(u => u.Id != userParams.UserId);

            users = users.Where(u => usersInRole.Contains(u));
            if (userParams.MinAge != Constants.MinAge || userParams.MaxAge != Constants.MaxAge)
            {
                var minDateOfBirth = DateTime.Today.AddYears(-userParams.MaxAge - 1);
                var maxDateOfBirth = DateTime.Today.AddYears(-userParams.MinAge);
                users = users.Where(u => u.DateOfBirth >= minDateOfBirth && u.DateOfBirth <= maxDateOfBirth);
            }
            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }


            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }


    }

    public class UserManager
    {
    }
}