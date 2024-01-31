using AS_Assignment_2_222256B.Model;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;
using WebApplication3.Model;
using WebApplication3.ViewModels;

namespace WebApplication3.Pages
{
    public class RegisterModel : PageModel
    {

        private UserManager<ApplicationUser> userManager { get; }
        private AuthDbContext _authDbContext { get; set; }
        private readonly ILogger<RegisterModel> _logger;

        [BindProperty]
        public Register RModel { get; set; }

        public RegisterModel(UserManager<ApplicationUser> userManager, ILogger<RegisterModel> logger, AuthDbContext authDbContext)
        {
            this.userManager = userManager;
            _logger = logger;
            _authDbContext = authDbContext;
        }

        public void OnGet()
        {
        }


        // Save data into database
        public async Task<IActionResult>OnPostAsync()
        {
            if (!ValidateCaptcha())
            {
                ModelState.AddModelError("", "Captcha Failed");
                return Page();
            }

            ValidateExtension(RModel.Resume);
            ValidateNRIC(RModel.NRIC);
            ValidatePassword(RModel.Password);
            ValidateDOB(RModel.DateOfBirth);

            if (ModelState.IsValid)
            {
                var ResumeFileName = Guid.NewGuid().ToString() + RModel.Resume.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", ResumeFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await RModel.Resume.CopyToAsync(fileStream);
                }
                var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
                var protector = dataProtectionProvider.CreateProtector("MySecretKey");
                var user = new ApplicationUser()
                {
                    FirstName = RModel.FirstName,
                    LastName = RModel.LastName,
                    Gender = RModel.Gender,
                    NRIC = protector.Protect(RModel.NRIC),
                    UserName = RModel.Email,
                    Email = RModel.Email,
                    DateOfBirth = RModel.DateOfBirth,
                    Resume = ResumeFileName,
                    WhoAmI = WebUtility.HtmlEncode(RModel.WhoAmI),
                };
                var auditLogItem = new AuditLog()
                {
                    Email = RModel.Email,
                    Action = "Registration",
                    Timestamp = DateTime.Now,
                };
                
                var result = await userManager.CreateAsync(user, RModel.Password);
                if (result.Succeeded)
                {
					_authDbContext.AuditLogs.Add(auditLogItem);
					await _authDbContext.SaveChangesAsync();
                    return RedirectToPage("Login");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return Page();
        }

        private void ValidateExtension(IFormFile Resume)
        {
            if (Resume != null)
            {
                var FileExtension = Path.GetExtension(Resume.FileName).ToLower();
                if (FileExtension != ".pdf" && FileExtension != ".docx")
                {
                    ModelState.AddModelError("Resume", "Please input a valid file type");
                }
            } 
        }

        private void ValidateNRIC(string NRIC)
        {
            if (NRIC != null)
            {
                if (string.IsNullOrEmpty(NRIC) || NRIC.Length != 9)
                {
                    ModelState.AddModelError("NRIC", "Please input a valid NRIC");
                }

                // Check first letter
                char firstChar = NRIC[0];
                if (firstChar != 'S' && firstChar != 'T' && firstChar != 'F' && firstChar != 'G')
                {
                    ModelState.AddModelError("NRIC", "Please input a valid NRIC");
                }

                // Extract the numeric part
                if (NRIC.Length == 9)
                {
                    if (!int.TryParse(NRIC.Substring(1, 7), out int numericPart))
                    {
                        ModelState.AddModelError("NRIC", "Please input a valid NRIC");
                    }
                }
            }
        }

        private void ValidatePassword(string Password)
        {
            if (Password != null)
            {
                if (Password.Length < 15)
                {
                    ModelState.AddModelError("Password", "Please enter a password longer than 15 characters");
                }

                bool hasLowerCase = false;
                bool hasUpperCase = false;
                bool hasDigit = false;
                bool hasSpecialChar = false;

                foreach (char c in Password)
                {
                    if (char.IsLower(c))
                    {
                        hasLowerCase = true;
                    }
                    else if (char.IsUpper(c))
                    {
                        hasUpperCase = true;
                    }
                    else if (char.IsDigit(c))
                    {
                        hasDigit = true;
                    }
                    else if (!char.IsLetterOrDigit(c))
                    {
                        hasSpecialChar = true;
                    }
                }

                if (hasLowerCase == false)
                {
                    ModelState.AddModelError("Password", "Password should at least have 1 lowercase character");
                }

                if (hasUpperCase == false)
                {
                    ModelState.AddModelError("Password", "Password should have at least 1 uppercase character");
                }

                if (hasDigit == false)
                {
                    ModelState.AddModelError("Password", "Password should have at least 1 numeric character");
                }

                if (hasSpecialChar == false)
                {
                    ModelState.AddModelError("Password", "Password should have at least 1 special character");
                }
            }
        }

        private void ValidateDOB(DateTime DOB)
        {
            if (DOB >= DateTime.Today && DOB != null)
            {
                ModelState.AddModelError("Date Of Birth", "Date of birth should be in the past");
            }
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
