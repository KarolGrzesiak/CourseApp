using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CourseApp.API.Model
{
    public class Role : IdentityRole<int>
    {
        public ICollection<UserRole> UserRoles { get; set; }


    }
}