using Marchadri.Common.Extantions;
using Marchadri.Common.Helpers;
using Marchadri.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using static Marchadri.Data.Entities.SystemRole.SystemRole;

namespace Marchadri.Filters
{
	public class CheckSystemRoleFilter : ActionFilterAttribute
	{
		public ESystemRoles Role { get; private set; }

		public CheckSystemRoleFilter(ESystemRoles role)
		{
			Role = role;
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (FiltersHelper.HasAllowAnonymous(context))
			{
				return;
			}

			System.Guid roleId = context.HttpContext.User.GetGuidClaim(TokenAuthenticationFilter.EClaimType.SystemRole);
			if (roleId != GuidLibrary.GetId(Role))
			{
				throw new ForbiddenException("You do not have correct role in system to perform this action. Please contact your system administrator.");
			}

			base.OnActionExecuting(context);
		}
	}
}