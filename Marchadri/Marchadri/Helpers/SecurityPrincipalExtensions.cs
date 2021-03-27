using Marchadri.Common.Extantions;
using Marchadri.Filters;
using System;
using System.Linq;
using System.Security.Claims;

namespace Marchadri.Helpers
{
	public static class SecurityPrincipalExtensions
	{
		public static Guid GetGuidClaim(this ClaimsPrincipal user, TokenAuthenticationFilter.EClaimType claimType)
		{
			string claimValue = user.Claims.FirstOrDefault(c => c.Type == claimType.ToString())?.Value;
			if (!Guid.TryParse(claimValue, out Guid id))
			{
				ThrowClaimNotDefinedException(claimType);
			}

			return id;
		}

		public static string GetStringClaim(this ClaimsPrincipal user, TokenAuthenticationFilter.EClaimType claimType)
		{
			string claimValue = user.Claims?.FirstOrDefault(c => c.Type == claimType.ToString())?.Value;
			if (string.IsNullOrEmpty(claimValue))
			{
				ThrowClaimNotDefinedException(claimType);
			}

			return claimValue;
		}

		private static void ThrowClaimNotDefinedException(TokenAuthenticationFilter.EClaimType claimType)
		{
			throw new KeyValueException("Error", $"{claimType} is not defined");
		}
	}
}