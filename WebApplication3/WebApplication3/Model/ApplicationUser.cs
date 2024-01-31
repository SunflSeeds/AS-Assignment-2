using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace WebApplication3.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string NRIC { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Resume { get; set; } = string.Empty;
        public string WhoAmI { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty;
        //public string Password1 { get; set; } = string.Empty;
        //public string Password2 { get; set; } = string.Empty;
    }
}