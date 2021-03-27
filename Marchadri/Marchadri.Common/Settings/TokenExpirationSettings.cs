using System;

namespace Marchadri.Common.Settings
{
    public class TokenExpirationSettings
    {
	    public TimeSpan AuthTokenExpirationTime { get; set; }
	    public TimeSpan ConfirmationTokenExpirationTime { get; set; }
	    public TimeSpan PasswordResetTokenExpirationTime { get; set; }
    }
}