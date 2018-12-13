using Microsoft.AspNetCore.Identity;

namespace CourseApp.API.Model
{
    public class UserRole : IdentityUserRole<int>
    {
        public User User { get; set; }
        public Role Role { get; set; }
    }
}