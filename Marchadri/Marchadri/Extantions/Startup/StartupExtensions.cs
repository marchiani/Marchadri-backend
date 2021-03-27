using System;
using System.Net.Http;
using System.Reflection;
using Marchadri.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;

namespace Marchadri.Extantions.Startup
{
    internal static class StartupExtensions
    {
        public static IServiceCollection AddDatabaseContexts(this IServiceCollection services, IConfiguration configuration)
        {
            string connection = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlServer(connection,
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(Marchadri.Startup).GetTypeInfo().Assembly.GetName().Name);
                        //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
            });

            return services;
        }

        public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            // HttpClient

            return services;
        }

        public static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient("IncommAppClient");
            services.AddHttpClient("IncommApiClient")
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(GetRetryPolicy());
            //services.AddHttpClient<ITangoCardClient, TangoCardClient>();
            //services.AddHttpClient<IDiggecardClient, DiggecardClient>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services, IWebHostEnvironment env)
        {
            //services.AddScoped<IWebsiteService, WebsiteService>();

            //services.AddScoped<IPasswordGenerator, PasswordGenerator>();

            //services.AddScoped<IBrandsService, BrandsService>();

            //if (env.IsDevelopment() || env.IsTest() || env.IsLocal())
            //{
            //    // Development exchange rates service only supports USD as base currency
            //    services.AddScoped<ICurrencyExchangeService, DollarBaseCurrencyExchangeService>();
            //}
            //else
            //{
            //    services.AddScoped<ICurrencyExchangeService, CurrencyExchangeService>();
            //}

            //services.AddScoped<IPriceCalculator, PriceCalculator>();
            //services.AddScoped<ICsvParser, CsvParser>();

            //services.AddScoped<IIncommClient, IncommClient>();
            //services.AddScoped<IEpayMiddleEastClient, EpayMiddleEastClient>();

            //services.AddSingleton<OrdersToProcess>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            //services.AddScoped<IUsersRepository, UsersRepository>();

            return services;
        }

        public static IServiceCollection AddHostedServices(this IServiceCollection services, IWebHostEnvironment environment)
        {
            // Do not initialize countries, currencies, suppliers for development and test
            // They are normally up to date
            if (!environment.IsDevelopment() && !environment.IsTest())
            {
                //services.AddHostedService<CountryInitializingHostedService>();
            }

            //services.AddHostedService<QuartzHostedService>();
            //// services.AddHostedService<XoXoUpdateAccessTokenHostedService>();
            //services.AddHostedService<OrderProcessingBackgroundService>();

            // Should be last configuration in services because uses results from invocations of previous hosted services
            if (environment.IsLocal())
            {
                //services.AddHostedService<DataSeedHostedService>();
            }

            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                                                                            retryAttempt)));
        }

        public static bool IsLocal(this IHostEnvironment hostEnvironment)
        {
            return hostEnvironment.IsEnvironment("Local");
        }

        public static bool IsTest(this IHostEnvironment hostEnvironment)
        {
            return hostEnvironment.IsEnvironment("Test");
        }
    }
}