using System;

namespace Marchadri.Data.Entities.SystemRole
{
	public class SystemRole
	{
		public Guid Id { get; set; }
		public string Name { get; set; }

		public enum ESystemRoles
		{
			SuperAdministrator,
			User
		}
	}
}