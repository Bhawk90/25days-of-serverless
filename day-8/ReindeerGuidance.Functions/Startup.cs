using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReindeerGuidance.Core;
using ReindeerGuidance.Core.Repositories;
using ReindeerGuidance.Functions;


[assembly: FunctionsStartup(typeof(Startup))]
namespace ReindeerGuidance.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services
                .AddOptions<TableStorageConfiguration>()
                .Configure<IConfiguration>((settings, configuration) => {
                    configuration.Bind(settings); 
                });

            builder.Services.AddScoped<IIncidentRepository, IncidentRepository>();
        }
    }
}
