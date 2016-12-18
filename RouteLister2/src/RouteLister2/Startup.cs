using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RouteLister2.Data;
using RouteLister2.Models;
using RouteLister2.Services;
using AutoMapper;
using Newtonsoft.Json;
using Microsoft.AspNetCore.SignalR;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RouteLister2.Services.SimpleTokenProvider;
using Microsoft.Extensions.Options;

namespace RouteLister2
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();

            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddTransient<SeedDefaultUser>();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

         

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            
            //AutoMapper Service
            AutoMapper.IConfigurationProvider configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfileConfiguration>();

            });
            services.AddTransient(sp => configuration.CreateMapper());
            //Unit of work service
            
            services.AddTransient<SignalRBusinessLayer>();
            services.AddTransient<RouteListerRepository>();
            

            //Things needed for SignalR, like a ContractResolver
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new SignalRContractResolver();

            var serializer = JsonSerializer.Create(settings);

            services.Add(new ServiceDescriptor(typeof(JsonSerializer),
                         provider => serializer,
                         ServiceLifetime.Transient));
            services.AddSingleton<ConnectionMapping<string>>();

            //SignalR
            services.AddSignalR(options =>
            {
                options.Hubs.EnableDetailedErrors = true;
            });
            services.AddMvc();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, SeedDefaultUser seedUser)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            // secretKey contains a secret passphrase only your server knows
            //TODO change
            var secretKey = "mysupersecret_secretkey!123";
            //Creating signinKey
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            //Creating token validation parameters
            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = "ExampleIssuer",

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = "ExampleAudience",

                // Validate the token expiry
                ValidateLifetime = true,

                // If you want to allow a certain amount of clock drift, set that here:
                //TODO investigate
                ClockSkew = TimeSpan.Zero
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            });
            //Seeding user
            seedUser.SeedAdminUser();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                                name: "apiCulture",
                                template: "api/{controller}/{id?}"
);
            });
            //Adding token provider
            var options = new TokenProviderOptions
            {
                Audience = "ExampleAudience",
                Issuer = "ExampleIssuer",
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
            };
            //Registering token provider middleware
            app.UseMiddleware<TokenProviderMiddleware>(Options.Create(options));

            app.UseWebSockets();
            app.UseSignalR();
        }
    }
}
