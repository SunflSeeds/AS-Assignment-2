using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace WebApplication3.ViewModels
{
    public class Register
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "First Name must be alphanumeric")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
		[RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Last Name must be alphanumeric")]
		public string LastName { get; set; } = string.Empty;

        [Required]
		[RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Gender must be alphanumeric")]
		public string Gender { get; set; } = string.Empty;

        [Required]
		[RegularExpression(@"^[STFG]\d{7}[A-JZ]$", ErrorMessage = "Please input a valid NRIC")]
		public string NRIC { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Please input a valid email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^a-zA-Z\\d]).{15,}$", ErrorMessage = "Your password is not strong enough")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public IFormFile Resume { get; set; }

        [Required]
        public string WhoAmI { get; set; } = string.Empty;
    }
}
