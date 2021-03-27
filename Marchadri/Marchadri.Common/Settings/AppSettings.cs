using System;

namespace Marchadri.Common.Settings
{
	public class AppSettings
	{
		public const string DomainRegex = "(?!-)^[a-zA-Z0-9-]*(?<!-)";
		public const string DomainValidationError = "Domain field can contain only letters, numbers and '-' symbol and can not start with '-'";

		public static string AuthorizationHeader => "Authorization";
		public static string CompanyDomainHeader => "X-Company-Domain";
		public static string CompanyStatusHeader => "X-Company-Status";
		public static string GetRegistrationDetailsHeader => "X-Get-Registration-Details";
		public static string RefererHeader => "X-Referer";
		public static string UserIdHeader => "X-User-Id";

		public static string DomainPostfix { get; set; }

		public static string GetAdjustedDomain(string domain)
		{
			return $"{domain}{DomainPostfix}";
		}

		public static string GetApplicationUrlScheme(string domain = null)
		{
			string adjustedDomain = GetAdjustedDomain(domain ?? "auth");

			return $"https://{adjustedDomain}.myshiftlab.app/";
		}

		public static string GetChangeEmailUrl(string confirmationChangeEmailToken)
		{
			return $"{GetApplicationUrlScheme()}confirm-change-email/{confirmationChangeEmailToken}";
		}

		public static string GetFinishRegistrationUrl(Guid confirmationEmailToken)
		{
			return $"{GetApplicationUrlScheme()}get-registration-details/{confirmationEmailToken}";
		}

		public static string GetPayForInvoiceUrl(string domain, Guid invoiceId)
		{
			return $"{GetApplicationUrlScheme(domain)}pay-for-invoice/{invoiceId}";
		}
    }
}