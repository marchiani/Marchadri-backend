using System;

namespace Marchadri.Data.Entities.Users
{
	public class User
	{
		public Guid Id { get; set; }
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string MobileNumber { get; set; }
		public string Email { get; set; }
		public bool IsAdmin { get; set; }
		public Guid AccessToken { get; set; }
		public SystemRole.SystemRole SystemRole { get; set; }
		public DateTime? AccessTokenFrom { get; set; }
		public System.Guid? ConfirmEmailToken { get; set; }
		public System.DateTime? ConfirmEmailTokenFrom { get; set; }
		public Guid? ResetPasswordToken { get; set; }
		public DateTime? ResetPasswordTokenFrom { get; set; }
		public bool IsConfirmed { get; set; }
		public DateTime? LastLoggedIn { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime ModifiedDate { get; set; }
	}
}