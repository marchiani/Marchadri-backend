using Marchadri.Data.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using Marchadri.Data.Entities.SystemRole;

namespace Marchadri.Data.Entities
{
	public class ApplicationContext : DbContext
	{
		private readonly ILogger<ApplicationContext> _logger;

		public DbSet<User> Products { get; set; }
		public DbSet<SystemRole.SystemRole> SystemRoles { get; set; }

		public ApplicationContext(DbContextOptions<ApplicationContext> options, ILogger<ApplicationContext> logger)
			: base(options)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));

			try
			{
				Database.Migrate();
			}
			catch (Exception ex)
			{
				string operation = "Initialization.Startup";
				string message = "Could not migrate database";
				string data = JsonConvert.SerializeObject(ex);
				_logger.LogError($"Error in operation: {operation} - Message: {message} - Exception: {data}");
				throw;
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//modelBuilder.Entity<EmailRequestType>().HasKey(i => i.Id);

			ConfigureQueryFilters(modelBuilder);
		}

		private static void ConfigureQueryFilters(ModelBuilder modelBuilder)
		{
			//modelBuilder.Entity<Supplier>().HasQueryFilter(s => s.Code != "tangocard" && s.Status != Statuses.Deleted);
		}
	}
}