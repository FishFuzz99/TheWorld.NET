using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using TheWorld.Services;
using Microsoft.Dnx.Runtime;

namespace TheWorld.NET
{
    public class Startup
    {
        public static IConfigurationRoot Configuration;

        public Startup(IApplicationEnvironment appEnv)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

#if DEBUG
            services.AddScoped<IMailService, DebugMailService>();
#else
            services.AddScoped<IMailService, RealMailService>();
#endif


        }

        public void Configure(IApplicationBuilder app)
        { // order matters
            app.UseStaticFiles();
            app.UseMvc(config =>
            {
                config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "App", Action = "index" }
                    );
            });
        }

         //public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
