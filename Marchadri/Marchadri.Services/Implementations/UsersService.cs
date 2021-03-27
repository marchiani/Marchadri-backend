using System;
using Marchadri.Data.Entities.Users;
using Marchadri.Services.Interfaces;

namespace Marchadri.Services.Implementations
{
	public class UsersService : IUsersService
	{
		public User GetUserByAccessToken(Guid accesToken)
		{
			throw new NotImplementedException();
		}

		public void RefreshAccessToken(User user, bool refreshTokenFromOnly, bool commit)
		{
			throw new NotImplementedException();
		}
	}
}