using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace ltbdb.Models
{
	public class UserModel
	{
		public ObjectId Id { get; set; }

		[Required(ErrorMessage="Bitte gib einen Benutzernamen ein.")]
		public string Username { get; set; }

		[Required(ErrorMessage="Bitte gib ein Passwort ein.")]
		public string Password { get; set; }

		[Required(ErrorMessage="Bitte gib ein Passwort ein.")]
		public string PasswordRepeat { get; set; }

		public string Role { get; set; }

		public bool Enabled { get; set; }
	}
}