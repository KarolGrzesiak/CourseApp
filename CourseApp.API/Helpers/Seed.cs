using System.Collections.Generic;
using System.IO;
using System.Linq;
using CourseApp.API.Model;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace CourseApp.API.Helpers
{
    public class Seed
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public Seed(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;

        }

        public void SeedUsersAndRoles()
        {
            if (!_userManager.Users.Any())
            {
                var userData = File.ReadAllText("Helpers/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                var roles = new List<Role>{
                    new Role{Name=Constants.StudentRole},
                    new Role{Name=Constants.TeacherRole},
                    new Role{Name=Constants.AdminRole},
                };

                foreach (var role in roles)
                {
                    _roleManager.CreateAsync(role).Wait();
                }

                for (int i = 0; i < users.Count; i++)
                {
                    _userManager.CreateAsync(users[i], Constants.Password).Wait();
                    if (i < 3)
                        _userManager.AddToRoleAsync(users[i], Constants.TeacherRole).Wait();
                    else
                        _userManager.AddToRoleAsync(users[i], Constants.StudentRole).Wait();
                }

                var adminUser = new User
                {
                    UserName = "Admin"
                };

                IdentityResult result = _userManager.CreateAsync(adminUser, Constants.Password).Result;

                if (result.Succeeded)
                {
                    var admin = _userManager.FindByNameAsync("Admin").Result;
                    _userManager.AddToRoleAsync(admin, Constants.AdminRole).Wait();
                }

            }
        }
    }
}