using AS_Assignment_2_222256B.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Model;

namespace WebApplication3.Pages
{
    public class LogoutModel : PageModel
    {
		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly UserManager<ApplicationUser> userManager;
		private readonly AuthDbContext _context;
		private readonly ILogger<LoginModel> _logger;

		public LogoutModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, AuthDbContext context, ILogger<LoginModel> logger)
		{
			this.signInManager = signInManager;
			this.userManager = userManager;
			_context = context;
			_logger = logger;
		}
		public void OnGet()
        {
        }

		public async Task<IActionResult> OnPostLogoutAsync()
		{
			//var user = await userManager.GetUserAsync(User);
			//var auditLogItem = new AuditLog()
			//{
			//	Email = user.UserName,
			//	Action = "Logout",
			//	Timestamp = DateTime.Now,
			//};
			//_context.AuditLogs.Add(auditLogItem);

			//await _context.SaveChangesAsync();

			HttpContext.Session.Clear();
			await signInManager.SignOutAsync();
			return RedirectToPage("Login");
		}
		public async Task<IActionResult> OnPostDontLogoutAsync()
		{
			return RedirectToPage("Index");
		}
	}
}
