using Microsoft.AspNetCore.Identity;

namespace New_Ston_Request.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
