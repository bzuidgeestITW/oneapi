using System;
using System.IO;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Synigo.DemoApi.Model;
using Synigo.OneApi.Clients;
using Synigo.OneApi.Clients.Notifications;
using Synigo.OneApi.Providers.Tokens;

[assembly: FunctionsStartup(typeof(Synigo.DemoApi.Startup))]
namespace Synigo.DemoApi
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var adSettings = new AzureAdSettings();
            var synigoSettings = new SynigoSettings();

            builder.GetContext().Configuration.Bind("synigo", synigoSettings);
            builder.GetContext().Configuration.Bind("azureAd", adSettings);

            builder.Services.AddSingleton<TokenProvider>((factory) => {
                return new TokenProvider(adSettings, synigoSettings);
            });


            builder.Services.AddSingleton((serviceProvider) =>
            {
                return ConfidentialClientApplicationBuilder
                    .Create(adSettings.ClientId)
                    .WithClientSecret(adSettings.ClientSecret)
                    .WithTenantId(adSettings.TenantId)
                    .WithAuthority(AzureCloudInstance.AzurePublic, adSettings.TenantId)
                    .Build();
            });

            builder.Services.AddScoped<ITokenProvider, DefaultTokenProvider>();
            // todo: NotificationClient is poorly implemented for DI and now wrapped by SynigoNotificationService. 
            // We can easily integrate the code from notificationsClient into SynigoNotificationService and remove this poor dependency. Its just a thin wrapper itself.
            builder.Services.AddScoped<SynigoApiClient, SynigoApiClient>();
            builder.Services.AddTransient<INotificationsClient, NotificationsClient>();

            //builder.Services.AddSingleton<INotificationsClient>((factory) => {
            //    builder.GetContext().Configuration.Bind("synigo", synigoSettings);
            //    builder.GetContext().Configuration.Bind("azureAd",adSettings);

            //    return new NotificationsClient(adSettings.ClientId, adSettings.ClientSecret, adSettings.TenantId, synigoSettings.SynigoApiUrl);
            //});
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var context = builder.GetContext();
            builder.ConfigurationBuilder.AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), optional: true, reloadOnChange: false);            
        }
    }
}