using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Dnx.Runtime;
using TheWorld.NET.Services;
using TheWorld.NET.Models;
using Newtonsoft.Json.Serialization;
using AutoMapper;
using TheWorld.NET.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authentication.Cookies;
using System.Net;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
            
            services.AddMvc(config =>
            {
#if !DEBUG
                config.Filters.Add(new RequireHttpsAttribute());
#endif
            })  
            .AddJsonOptions(opt =>
            {
                opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddIdentity<WorldUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 8;
                config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
                config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = ctx =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == (int)HttpStatusCode.OK)
                        {
                            ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        }
                        
                        ctx.Response.Redirect(ctx.RedirectUri);

                        return Task.FromResult(0);
                    }
                };
            })
            .AddEntityFrameworkStores<WorldContext>();

            services.AddLogging();

            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<WorldContext>();

            //services.AddScoped<WorldContextSeedData>(); this would work // also, AddSingleton AddInstance
            services.AddScoped<CoordService>();
            services.AddTransient<WorldContextSeedData>();
            services.AddScoped<IWorldRepository, WorldRepository>();

#if DEBUG
            services.AddScoped<IMailService, DebugMailService>();
#else
            services.AddScoped<IMailService, RealMailService>();
#endif


        }

        public async void Configure(IApplicationBuilder app, WorldContextSeedData seeder, ILoggerFactory loggerFactory)
        { 
            // order matters
            loggerFactory.AddDebug(LogLevel.Warning);

            app.UseStaticFiles();

            app.UseIdentity();

            Mapper.Initialize(config =>
            {
                config.CreateMap<Trip, TripViewModel>().ReverseMap();
                config.CreateMap<Stop, StopViewModel>().ReverseMap();
            });

            app.UseMvc(config =>
            {
                config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "App", Action = "index" }
                    );
            });

            await seeder.EnsureSeedDataAsync();
        }

         public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
