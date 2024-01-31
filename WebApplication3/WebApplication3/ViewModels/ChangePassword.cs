using System.ComponentModel.DataAnnotations;

namespace AS_Assignment_2_222256B.ViewModels
{
	public class ChangePassword
	{
		[Required]
		public string CurrentPassword { get; set; } = string.Empty;

		[Required]
		[DataType(DataType.Password)]
		[RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^a-zA-Z\\d]).{15,}$", ErrorMessage = "Your password is not strong enough")]
		public string NewPassword { get; set; } = string.Empty;

		[Required]
		[DataType(DataType.Password)]
		[Compare(nameof(NewPassword), ErrorMessage = "Password and confirmation password does not match")]
		public string ConfirmPassword { get; set; } = string.Empty;
	}
}
