using System.Configuration;
using System.Linq;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Marchadri.Extantions.Cors
{
	public static class ServiceCollectionExtensions
	{
		public static CorsOptions AddDefaultPortalCors(this CorsOptions options, IConfiguration configuration)
		{
			var origins = GetAllowedOrigins(configuration);
			options.AddDefaultPolicy(
				builder =>
				{
					builder
						.WithOrigins(origins)
						.AllowAnyMethod()
						.AllowAnyHeader()
						.AllowCredentials();
				});

			return options;
		}

		private static string[] GetAllowedOrigins(IConfiguration configuration)
		{
			var section = configuration.GetSection(CorsConfig.ConfigSection);
			if (section == null)
			{
				throw new ConfigurationErrorsException($"Invalid configuration. {CorsConfig.ConfigSection} is missing");
			}

			var config = section.Get<CorsConfig>();
			if (config == null)
			{
				throw new ConfigurationErrorsException($"Invalid configuration {CorsConfig.ConfigSection}");
			}

			if (string.IsNullOrEmpty(config.AllowedOrigins))
			{
				throw new ConfigurationErrorsException($"Invalid configuration {CorsConfig.ConfigSection}:{nameof(CorsConfig.AllowedOrigins)}");
			}

			var origins = config.AllowedOrigins.Split(',');
			origins = origins
				.Select(o => o.Trim())
				.Where(o => o != "")
				.ToArray();
			if (origins.Length == 0)
			{
				throw new ConfigurationErrorsException($"Invalid configuration. {CorsConfig.ConfigSection}:{nameof(CorsConfig.AllowedOrigins)} does not containt valid origins");
			}

			return origins;
		}
	}
}