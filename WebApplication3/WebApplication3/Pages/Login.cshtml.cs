using AS_Assignment_2_222256B.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using WebApplication3.Model;
using WebApplication3.ViewModels;

namespace WebApplication3.Pages
{
    public class LoginModel : PageModel
    {
		[BindProperty]
		public Login LModel { get; set; }

		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly AuthDbContext _context;
		private readonly ILogger<LoginModel> _logger;
		public LoginModel(SignInManager<ApplicationUser> signInManager, AuthDbContext context, ILogger<LoginModel> logger)
		{
			this.signInManager = signInManager;
			_context = context;	
			_logger = logger;
		}
		public void OnGet()
        {
        }

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ValidateCaptcha())
			{
				ModelState.AddModelError("", "Captcha Failed");
				return Page();
			}

			if (ModelState.IsValid)
			{
				var identityResult = await signInManager.PasswordSignInAsync(LModel.Email, LModel.Password, LModel.RememberMe, true);

				var userExistenceCheck = _context.Users.ToList().Where(x => x.Email == LModel.Email).FirstOrDefault();
				if (userExistenceCheck == null)
				{
					return RedirectToPage("Login");
				}

				if (identityResult.Succeeded)
				{
					var sessionId = Guid.NewGuid().ToString();
					HttpContext.Session.SetString("SessionId", sessionId);
					userExistenceCheck.SessionId = sessionId;

					var auditLogItem = new AuditLog()
					{
						Email = LModel.Email,
						Action = "Login",
						Timestamp = DateTime.Now,
					};
					_context.AuditLogs.Add(auditLogItem);

					await _context.SaveChangesAsync();
					return RedirectToPage("Index");
				}
				else
				{
					ModelState.AddModelError("", "Username or Password incorrect");
				}
			}
			return Page();
		}

		public bool ValidateCaptcha()
		{
			string response = Request.Form["g-recaptcha-response"];
			string secretKey = "6LfK_18pAAAAALm6vauZPZimzU6WAFGCpbMD1Zzh\r\n";
			bool Valid = false;

			HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=" + secretKey + "&response=" + response);

			try
			{
				using (WebResponse wResponse = req.GetResponse())
				{
					using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
					{
						string jsonResponse = readStream.ReadToEnd();
						CaptchaResponse data = JsonSerializer.Deserialize<CaptchaResponse>(jsonResponse);
						Valid = data.success;
					}
				}
				return Valid;
			}

			catch (WebException ex)
			{
				throw ex;
			}
		}

		private class CaptchaResponse
		{
			public bool success { get; set; }
		}
	}
}
