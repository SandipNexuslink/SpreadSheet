using CDR.Azure.Storage.Blob.Client;
using CDR.Services.Spreadsheet.Service;
using CDR.Services.Spreadsheet.Service.Interfaces;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Http;

[assembly: FunctionsStartup(typeof(CDR.Services.Spreadsheet.Web.Function.Startup))]
namespace CDR.Services.Spreadsheet.Web.Function
{
    public class Startup : FunctionsStartup
    {
        public IConfigurationRefresher ConfigurationRefresher { get; private set; }

        public bool IsDevelopment { get; set; } = false;
        public IConfiguration Config { get; private set; }
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton(Config);
            builder.Services.AddSingleton<IAzureBlobStorageClient, AzureBlobStorageClient>();
            builder.Services.AddSingleton<ValidationResult>();
            builder.Services.AddHttpClient<IValidationService, ValidationService>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(GetRetryPolicy());
            //TODO: Enable this whne we have azure connection
            //builder.Services.AddAzureAppConfiguration();
            //builder.Services.AddSingleton(ConfigurationRefresher);
            builder.Services.AddSingleton<IValidationService, ValidationService>();

            builder.Services.AddLogging();
        }
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var hostBuilderContext = builder.GetContext();

            try
            {
                builder.ConfigurationBuilder
                    .AddJsonFile(Path.Combine(hostBuilderContext.ApplicationRootPath, $"local.settings.json"), optional: true, reloadOnChange: true);
                //Config = builder.ConfigurationBuilder.Build();
                Config = new ConfigurationBuilder()
                          .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                          .AddJsonFile("local.settings.json", reloadOnChange: true, optional: false)
                          .AddEnvironmentVariables()
                          .Build();
            }
            catch { };

            builder.ConfigurationBuilder.AddEnvironmentVariables();
            Config = builder.ConfigurationBuilder.Build();


            IsDevelopment = (!string.IsNullOrEmpty(Config["IsDevelopment"])) && Config["IsDevelopment"] == "true";
            //TODO: Enable this whne we have azure connection
            //if (IsDevelopment)
            //{
            //    builder.ConfigurationBuilder
            //        .AddUserSecrets<Startup>(optional: true, reloadOnChange: false);
            //}

            Config = builder.ConfigurationBuilder.Build();
            //TODO: Enable this whne we have azure connection
            //var applicationConfigurationEndpoint =  Config["Azure:AppConfiguration:Endpoint"];
            //var applicationConfigurationSection = Config["Azure:AppConfiguration:Section"];
            //var applicationConfigurationBeacon = Config["Azure:AppConfiguration:Beacon"];

            //if (string.IsNullOrEmpty(applicationConfigurationEndpoint)) throw new Exception("No applicationConfigurationEndpoint config value");

            //builder.ConfigurationBuilder.AddAzureAppConfiguration(appConfigOptions =>
            //{
            //    TokenCredential azureCredential = default;

            //if (IsDevelopment)
            //{
            //    azureCredential = new ClientSecretCredential(Config["Development:Credentials:Azure:Tenant"], Config["Development:Credentials:Azure:Client"], Config["Development:Credentials:Azure:Key"]);
            //}
            //else
            //{
            //    azureCredential = new ManagedIdentityCredential(clientId: Config["Azure:AppConfiguration:ManagedIdentity"], options: null);
            //}

            //appConfigOptions
            //    .Connect(new Uri(applicationConfigurationEndpoint), azureCredential)
            //    .Select($"{applicationConfigurationSection}:*")
            //    .ConfigureKeyVault(keyVaultOptions =>
            //    {
            //        keyVaultOptions.SetCredential(azureCredential);
            //    })
            //    .ConfigureRefresh(refreshOptions =>
            //    {
            //        refreshOptions.Register(key: $"{applicationConfigurationSection}:{applicationConfigurationBeacon}", label: LabelFilter.Null, refreshAll: true);
            //        refreshOptions.SetCacheExpiration(TimeSpan.FromMinutes(3));
            //    });

            //    ConfigurationRefresher = appConfigOptions.GetRefresher();
            //});
            Config = builder.ConfigurationBuilder.Build();
        }
        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                                                                            retryAttempt)));
        }
    }
}
