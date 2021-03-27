using System;
using Marchadri.Data.Entities.Users;

namespace Marchadri.Services.Interfaces
{
	public interface IUsersService
	{
		User GetUserByAccessToken(Guid accesToken);
		void RefreshAccessToken(User user, bool refreshTokenFromOnly, bool commit);
	}
}