using Base.Services.AppSettings;
using Base.Services.Clients;
using Consul;
using System;
using IApplicationLifetime = Microsoft.AspNetCore.Hosting.IApplicationLifetime;

namespace WebShop.WebApi.Configurations
{
    public static class ConsulConfiguration
    {
        public static IServiceCollection AddConsul(this IServiceCollection serviceCollection)
        {
            using var serviceProvider = serviceCollection.BuildServiceProvider();
            var configuration = serviceProvider!.GetService<IConfiguration>();

            serviceCollection.Configure<ConsulAppSettings>(configuration!.GetSection("Consul"));
            serviceCollection.AddHttpClient<IConsulHttpClient, ConsulHttpClient>();

            var consulAppSettings = configuration!.GetSection("Consul").Get<ConsulAppSettings>();

            return serviceCollection.AddSingleton<IConsulClient>(c => new ConsulClient(cfg =>
            {
                if (!string.IsNullOrEmpty(consulAppSettings!.Host))
                {
                    cfg.Address = new Uri(consulAppSettings!.Host);
                }
            }));
        }

        public static string UseConsul(this IApplicationBuilder app/*, IApplicationLifetime lifetime*/)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var configuration = scope.ServiceProvider.GetService<IConfiguration>();

            var consulAppSettings = configuration!.GetSection("Consul").Get<ConsulAppSettings>();

            if (!consulAppSettings!.Enabled)
                return string.Empty;

            Guid serviceId = Guid.NewGuid();
            string consulServiceID = $"{consulAppSettings.Service}";

            var client = scope.ServiceProvider.GetService<IConsulClient>();

            var consulServiceRistration = new AgentServiceRegistration
            {
                Name = consulAppSettings.Service,
                ID = consulServiceID,
                Address = consulAppSettings.Address,
                Port = consulAppSettings.Port,
                Tags = new[] { consulAppSettings.Type },
                //Check = new()
                //{
                //    HTTP = $"http://{consulAppSettings.Address}:{consulAppSettings.Port}/api/Health",
                //    Method = "GET" ,
                //    Notes = "Checks /api/Health",
                //    Timeout = TimeSpan.FromSeconds(3),
                //    Interval = TimeSpan.FromSeconds(10)
                //}
                //Check = new()
                //{
                //    TCP = $"{consulAppSettings.Address}:{consulAppSettings.Port}",
                //    Notes = "TCP Check",
                //    Timeout = TimeSpan.FromSeconds(3),
                //    Interval = TimeSpan.FromSeconds(10)
                //}
            };

            client!.Agent.ServiceDeregister(consulServiceRistration.ID).Wait();
            client!.Agent.ServiceRegister(consulServiceRistration);

            return consulServiceID;
        }
    }
}
