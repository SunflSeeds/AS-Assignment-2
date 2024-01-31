using AS_Assignment_2_222256B.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication3.Model;
using WebApplication3.Pages;
using WebApplication3.ViewModels;

namespace AS_Assignment_2_222256B.Pages
{
	[Authorize]
    public class ChangePasswordModel : PageModel
    {
		private UserManager<ApplicationUser> userManager { get; }
		private SignInManager<ApplicationUser> signInManager { get; }
		private AuthDbContext _authDbContext { get; set; }
		private readonly ILogger<ChangePasswordModel> _logger;

		[BindProperty]
		public ChangePassword CModel { get; set; }

		public ChangePasswordModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<ChangePasswordModel> logger, AuthDbContext authDbContext)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
			_logger = logger;
			_authDbContext = authDbContext;
		}
		public void OnGet() { }
		public async Task<IActionResult>OnPostAsync()
        {
			var user = await userManager.GetUserAsync(User);
			var check1 = userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, CModel.NewPassword);
			var check2 = userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHistory, CModel.NewPassword);
			//var newPasswordHash = userManager.PasswordHasher.HashPassword(user, CModel.CurrentPassword);
			if (check1 == PasswordVerificationResult.Failed && check2 == PasswordVerificationResult.Failed)
			{
				_logger.LogInformation(check1.ToString());
				_logger.LogInformation(check2.ToString());
				var passwordChange = await userManager.ChangePasswordAsync(user, CModel.CurrentPassword, CModel.NewPassword);
				if (passwordChange.Succeeded)
				{
					user.PasswordHistory = user.PasswordHash;
					await userManager.UpdateAsync(user);
					await signInManager.SignInAsync(user, isPersistent: false);
					return RedirectToPage("Index");
				}
				else
				{
					ModelState.AddModelError("", "Your password is incorrect!");
				}
			}
			else
			{
				ModelState.AddModelError("", "Please do not reuse your last 2 passwords");
			}
			return Page();
		}
		public string PasswordHash { get; set; }
    }
}
