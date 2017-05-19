using System.ComponentModel.DataAnnotations;

namespace ltbdb.Models
{
	public class LoginModel
	{
		/// <summary>
		/// The username.
		/// </summary>
		[Required(ErrorMessage = "Bitte gib einen Benutzernamen ein.")]
		public string Username { get; set; }

		/// <summary>
		/// The password.
		/// </summary>
		[Required(ErrorMessage = "Bitte gib ein Passwort ein.")]
		public string Password { get; set; }
	}
}