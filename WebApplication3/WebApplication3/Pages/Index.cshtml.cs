using ASAssignment2222256B.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using WebApplication3.Model;

namespace WebApplication3.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly AuthDbContext _authDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(ILogger<IndexModel> logger, 
                            AuthDbContext authDbContext,
                            UserManager<ApplicationUser> userManager,
                            SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _authDbContext = authDbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task OnGet()
        {
            var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
            var _dataProtector = dataProtectionProvider.CreateProtector("MySecretKey");
            var userDetails = await _userManager.GetUserAsync(User);
            if (userDetails != null)
            {
                firstName = userDetails.FirstName;
                lastName = userDetails.LastName;
                gender = userDetails.Gender;
                nric = _dataProtector.Unprotect(userDetails.NRIC);
                emailAddress = userDetails.UserName;
                dob = userDetails.DateOfBirth;
                resume = userDetails.Resume;
                whoAmI = userDetails.WhoAmI;
            }

            if (userDetails == null || userDetails.SessionId != HttpContext.Session.GetString("SessionId"))
            {
                RedirectToPage("Login");
            }
        }

        public string firstName { get; set; }
        public string lastName { get; set; }
        public string gender { get; set; }
        public string nric { get; set; }
        public string emailAddress { get; set; }
        public DateTime dob { get; set; }
        public string resume { get; set; }
        public string whoAmI { get; set; }
    }
}