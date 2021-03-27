using System;
using System.Threading.Tasks;
using AutoMapper;
using Marchadri.Data.Entities.SystemRole;
using static Marchadri.Data.Entities.SystemRole.SystemRole;
using Marchadri.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Marchadri.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : BaseController
	{
		public UsersController(IMapper mapper) : base(mapper)
		{
		}

		[HttpGet]
		[CheckSystemRoleFilter(ESystemRoles.SuperAdministrator)]
		public async Task<IActionResult> GetUser([FromQuery] Guid UserId)
		{

			return Ok();
		}

	}
}