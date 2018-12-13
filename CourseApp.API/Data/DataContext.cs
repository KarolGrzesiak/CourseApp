using CourseApp.API.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.API.Data
{
    public class DataContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>
    , IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }


    }
}