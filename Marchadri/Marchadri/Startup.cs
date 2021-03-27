using Marchadri.Extantions.Cors;
using Marchadri.Extantions.Startup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Marchadri
{
	public class Startup
	{
		public Startup(IConfiguration configuration, IWebHostEnvironment env)
		{
			Configuration = configuration;
			Environment = env;
		}

		public IConfiguration Configuration { get; }

		public IWebHostEnvironment Environment { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors(options => options.AddDefaultPortalCors(Configuration));
			services.AddDatabaseContexts(Configuration)
				.ConfigureOptions(Configuration, Environment)
				.AddHttpClients()
				.AddServices(Environment)
				.AddRepositories()
				.AddHostedServices(Environment)
				.AddMvc()
				.ConfigureApiBehaviorOptions(options =>
				{
					options.SuppressModelStateInvalidFilter = true;
					options.SuppressInferBindingSourcesForParameters = true;
				})
				.AddNewtonsoftJson(x =>
				{
					if (x.SerializerSettings != null)
					{
						x.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
						x.SerializerSettings.Converters.Add(new StringEnumConverter());
						x.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
					}
				});
			services.AddControllers();
			services.AddMemoryCache();

			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo { Title = "BambooApi", Version = "v1" });
				options.EnableAnnotations();

				options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
				{
					Description = "OAuth2 Authorization header using the Bearer scheme with IdentityToken. Example: \"bearer {token}\"",
					In = ParameterLocation.Header,
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey
				});
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();
			app.UseCors();


			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bamboo API V1");
			});
		}
	}
}
