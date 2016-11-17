using System;
using System.Collections.Generic;
using System.Globalization;
using Lykke.AzureStorage.Tables;
using Lykke.Common.Log;
using LykkeMarketMakers.AzureRepositories;
using LykkeMarketMakers.AzureRepositories.Entities;
using LykkeMarketMakers.AzureRepositories.Log;
using LykkeMarketMakers.Core.DomainModels;
using LykkeMarketMakers.Core.Interfaces.Repositories;
using LykkeMarketMakers.Data;
using LykkeMarketMakers.Models;
using LykkeMarketMakers.Web.Filters;
using LykkeMarketMakers.Web.Helpers;
using LykkeMarketMakers.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LykkeMarketMakers.Web
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        private BaseSettings Settings { get; set; }
        public IHostingEnvironment HostingEnvironment { get; }
        private ILog Log { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            HostingEnvironment = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var settingsConnectionString = Configuration["SettingsConnString"];
            var settingsContainer = Configuration["SettingsContainerName"];
            var settingsFileName = Configuration["SettingsFileName"];
            var connectionStringLogs = Configuration["LogsConnString"];

            Log = new LogToTableRepository(new AzureTableStorage<LogEntity>(connectionStringLogs, "LogMarketMakersUI", null));

            try
            {
                Settings = GeneralSettingsReader.ReadGeneralSettings<BaseSettings>(settingsConnectionString,
                    settingsContainer, settingsFileName);
            }
            catch (Exception ex)
            {
                Log.WriteErrorAsync("Startup", "ReadSettingsFile", "Reading Settings File", ex).Wait();
            }

            CheckSettings(Settings, Log);

            try
            {
                // Add framework services.
                services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

                services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

                services.AddLocalization(options => options.ResourcesPath = "Translates");

                services.AddMvc()
                    .AddViewLocalization()
                    .AddDataAnnotationsLocalization();

                services.AddScoped<FilterFeaturesAccessAttribute>();

                services.Configure<RequestLocalizationOptions>(options =>
                    {
                        var supportedCultures = new List<CultureInfo>
                            {
                                new CultureInfo("en-US"),
                                new CultureInfo("ru-RU")
                            };

                        options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                        options.SupportedCultures = supportedCultures;
                        options.SupportedUICultures = supportedCultures;
                    }
                );

                services.RegisterRepositories(Settings.Db.MarketMakerConnString, Log);
                services.RegisterLyykeServices();
            }
            catch (Exception ex)
            {
                Log.WriteErrorAsync("Startup", "RegisterServices", "Registering Repositories and services", ex).Wait();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            try
            {
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug();

                if (env.IsStaging() || env.IsProduction())
                {
                    app.Use(async (context, next) =>
                    {
                        if (context.Request.IsHttps)
                        {
                            await next();
                        }
                        else
                        {
                            string withHttps = "https://" + context.Request.Host + context.Request.Path;
                            context.Response.Redirect(withHttps);
                        }
                    });
                }

                if (env.IsDevelopment() || env.IsStaging())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                }

                if (env.IsDevelopment())
                {
                    app.UseDatabaseErrorPage();
                    app.UseBrowserLink();
                }

                app.UseCookieAuthentication(new CookieAuthenticationOptions
                {
                    AuthenticationScheme = LykkeAuthHelper.AuthenticationScheme,
                    AutomaticAuthenticate = true,
                    AutomaticChallenge = true,
                    ExpireTimeSpan = TimeSpan.FromMinutes(60),
                    LoginPath = new PathString("/"),
                    AccessDeniedPath = "/"
                });

                app.UseStaticFiles();

                IOptions<RequestLocalizationOptions> locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
                app.UseRequestLocalization(locOptions.Value);

                app.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "areaRoute",
                        template: "{area:exists}/{controller}/{action}");
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                });

                CreateAdminUser(app);
                FillUsersCache(app);
            }
            catch (Exception ex)
            {
                Log.WriteErrorAsync("Startup", "Configure", "Configuring App and Authentication", ex).Wait();
            }
        }

        private void CheckSettings(BaseSettings settings, ILog log)
        {
            if (string.IsNullOrEmpty(settings.Db.MarketMakerConnString))
            {
                WriteSettingsReadError(log, nameof(settings.Db.MarketMakerConnString));
                throw new ArgumentNullException(nameof(settings.Db.MarketMakerConnString));
            }

            if (string.IsNullOrEmpty(settings.Db.LogsConnString))
            {
                WriteSettingsReadError(log, nameof(settings.Db.LogsConnString));
                throw new ArgumentNullException(nameof(settings.Db.LogsConnString));
            }
        }

        private void WriteSettingsReadError(ILog log, string elementName)
        {
            log.WriteErrorAsync("Startup:ReadSettings", "Read " + elementName, elementName + " Missing or Empty",
                new Exception(elementName + " is missing from the settings file!")).Wait();
        }

        private void CreateAdminUser(IApplicationBuilder app)
        {
            IBackOfficeUsersRepository usersRepository = app.ApplicationServices.GetService<IBackOfficeUsersRepository>();
            bool adminUser = usersRepository.UserExists("admin").Result;

            if (!adminUser)
            {
                usersRepository.CreateAsync(BackOfficeUser.CreateDefaultAdminUser("admin"), "123").Wait();
            }
        }

        private void FillUsersCache(IApplicationBuilder app)
        {
            IUserCacheService userCacheService = app.ApplicationServices.GetService<IUserCacheService>();
            userCacheService.UpdateUsersAndRoles().Wait();
        }
    }
}
