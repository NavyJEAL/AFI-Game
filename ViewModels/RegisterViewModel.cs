using System.ComponentModel.DataAnnotations;
using AFI_Game.Models;

namespace AFI_Game.ViewModels;
public class RegisterViewModel
{
	[Required]
	public required string Username { get; set; }
	[Required]
	public required string Password { get; set; }
	[Required]
	[Compare("Password", ErrorMessage = "Passwords do not match")]
	public required string ConfirmPassword { get; set; }

}