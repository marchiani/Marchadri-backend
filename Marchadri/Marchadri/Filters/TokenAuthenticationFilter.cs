using Marchadri.Common.Helpers;
using Marchadri.Common.Settings;
using Marchadri.Data.Entities.Users;
using Marchadri.Helpers;
using Marchadri.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http.ModelBinding;

namespace Marchadri.Filters
{
	public class TokenAuthenticationFilter : IAuthorizationFilter
	{
		public enum EClaimType
		{
			AccessToken,
			UserId,
			Username,
			SystemRole
		}

		private readonly IUsersService _usersService;

		private readonly TokenExpirationSettings TokenExpirationSettings;
		private readonly IDistributedCache DistributedCache;

		public TokenAuthenticationFilter
		(
			IUsersService usersService,
			TokenExpirationSettings tokenExpirationSettings,
			IDistributedCache distributedCache
		)
		{
			_usersService = usersService;
			TokenExpirationSettings = tokenExpirationSettings;
			DistributedCache = distributedCache;
		}

		public void OnAuthorization(AuthorizationFilterContext context)
		{
			if (FiltersHelper.HasAllowAnonymous(context))
			{
				return;
			}

			try
			{
				string accessTokenStr = GetAccessTokenString(context);
				if (!Guid.TryParse(accessTokenStr, out Guid token))
				{
					throw new Exception($"AccessToken is incorrect or expired: parse error {accessTokenStr}");
				}

				User user = JsonHelper.GetObject<User>(DistributedCache.GetString(token.ToString())) ?? _usersService.GetUserByAccessToken(token);

				CommonAuthenticationChecks(context, accessTokenStr, user);

				List<Claim> claims = new List<Claim>()
				{
					new Claim(EClaimType.AccessToken.ToString(), accessTokenStr),
					new Claim(EClaimType.UserId.ToString(), user.Id.ToString()),
					new Claim(EClaimType.Username.ToString(), user.Username),
					new Claim(EClaimType.SystemRole.ToString(), user.SystemRole.Id.ToString())
				};

				context.HttpContext.User.AddIdentity(new ClaimsIdentity(claims));

				if (user.AccessTokenFrom + TimeSpan.FromDays(1) < DateTime.UtcNow)
				{
					_usersService.RefreshAccessToken(user, true, true);
				}
			}
			catch (Exception e)
			{
				string message = string.IsNullOrEmpty(e.Message) ? "Authentication error" : e.Message;

				ModelStateDictionary modelState = new ModelStateDictionary();
				modelState.AddModelError("AuthenticationError", message);

				BadRequestObjectResult result = new BadRequestObjectResult(modelState)
				{
					StatusCode = 401
				};

				context.Result = result;

				throw;
			}
		}
		#region Helpers
		private string GetAccessTokenString(AuthorizationFilterContext context)
		{
			string headerToken = context.HttpContext.Request.Headers[AppSettings.AuthorizationHeader].FirstOrDefault();
			if (string.IsNullOrEmpty(headerToken))
			{
				throw new Exception($"'{AppSettings.AuthorizationHeader}' header is not defined");
			}

			return headerToken;
		}

		private void CommonAuthenticationChecks(AuthorizationFilterContext context, string accessTokenStr, User user)
		{
			if (user != null && !user.IsConfirmed)
			{
				FiltersHelper.RedirectUserToGetRegistrationDetails(context.HttpContext.Response.Headers, user.ConfirmEmailToken.Value);
			}
			if (user == null || user.AccessTokenFrom + TokenExpirationSettings.AuthTokenExpirationTime < DateTime.UtcNow)
			{
				throw new Exception($"AccessToken is incorrect or expired: {accessTokenStr}");
			}
		}
		#endregion
	}
}