using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using System;
using System.Threading.Tasks;
namespace CDR.Services.Spreadsheet.CommonTestAssets
{
    public class TestConfiguration
    {
        public static IConfigurationRefresher ConfigurationRefresher;

        public static bool IsDevelopment { get; set; } = false;
        public static bool IsRefreshed { get; set; } = false;

        public static IConfiguration GetConfiguration()
        {
            var configBuilder = new ConfigurationBuilder()
             .AddEnvironmentVariables()
             .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);

            var config = configBuilder.Build();

            IsDevelopment = (!string.IsNullOrEmpty(config["IsDevelopment"])) && config["IsDevelopment"] == "true";

            if (IsDevelopment)
            {
                configBuilder
                    .AddUserSecrets<TestConfiguration>(optional: true, reloadOnChange: false);
                config = configBuilder.Build();

                var applicationConfigurationEndpoint = config["Azure:AppConfiguration:Endpoint"];
                var applicationConfigurationSections = config["Azure:AppConfiguration:Sections"].Split(" ");
                var applicationConfigurationBeacon = config["Azure:AppConfiguration:Beacon"];

                if (string.IsNullOrEmpty(applicationConfigurationEndpoint)) throw new Exception("No applicationConfigurationEndpoint config value");

                configBuilder
                    .AddAzureAppConfiguration(appConfigOptions =>
                    {
                        var azureTenant = config["Development:Credentials:Azure:Tenant"];
                        if (string.IsNullOrEmpty(azureTenant)) throw new Exception("No azureTenant config value");
                        var azureCredential = new ClientSecretCredential(azureTenant, config["Development:Credentials:Azure:Client"], config["Development:Credentials:Azure:Key"]);

                        appConfigOptions
                            .Connect(new Uri(applicationConfigurationEndpoint), azureCredential)
                            .ConfigureKeyVault(keyVaultOptions =>
                            {
                                keyVaultOptions.SetCredential(azureCredential);
                            });

                        foreach (var section in applicationConfigurationSections)
                        {
                            appConfigOptions
                                .Select($"{section}:*")
                                .ConfigureRefresh(refreshOptions =>
                                {
                                    refreshOptions.Register(key: $"{section}:{applicationConfigurationBeacon}", label: LabelFilter.Null, refreshAll: true);
                                    refreshOptions.SetCacheExpiration(TimeSpan.FromMinutes(3));
                                });
                        }

                        ConfigurationRefresher = appConfigOptions.GetRefresher();
                    });

                config = configBuilder.Build();
            }
            return config;
        }

        public static async Task RefreshConfiguration()
        {
            if (ConfigurationRefresher != null)
            {
                if (!IsRefreshed)
                {
                    IsRefreshed = await ConfigurationRefresher.TryRefreshAsync();
                }
            }
        }
    }
}
