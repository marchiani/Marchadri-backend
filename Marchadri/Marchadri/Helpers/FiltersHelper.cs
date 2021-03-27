using System;
using System.Linq;
using Marchadri.Common.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Marchadri.Helpers
{
	public class FiltersHelper
	{
		public static bool HasAllowAnonymous(FilterContext context)
		{
			return context?.HttpContext?.GetEndpoint()?.Metadata?.GetMetadata<IAllowAnonymous>() != null || (context?.Filters.Any(f => f is AllowAnonymousFilter) ?? false);
		}

		public static void RedirectUserToGetRegistrationDetails(IHeaderDictionary headers, Guid confirmEmailToken)
		{
			headers.Add("Access-Control-Expose-Headers", AppSettings.GetRegistrationDetailsHeader);
			headers.Add(AppSettings.GetRegistrationDetailsHeader, AppSettings.GetFinishRegistrationUrl(confirmEmailToken));

			throw new Exception("Your account has not been activated yet");
		}
	}
}