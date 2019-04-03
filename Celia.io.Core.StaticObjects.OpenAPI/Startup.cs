using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Celia.io.Core.MicroServices.Utilities;
using Celia.io.Core.StaticObjects.DataAccess;
using Celia.io.Core.StaticObjects.Models;
using Celia.io.Core.StaticObjects.OpenAPI.Models;
using Celia.io.Core.StaticObjects.Services;
using Celia.io.Core.StaticObjects.Services.Impl;
using Celia.io.Core.StaticObjects.StorageProviders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;

namespace Celia.io.Core.StaticObjects.OpenAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            //加入配置文件
            var builder = new ConfigurationBuilder()
               .SetBasePath(hostingEnvironment.ContentRootPath)
               .AddJsonFile("appsettings.json", true, true)
               .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", true);
            builder.AddEnvironmentVariables();

            Configuration = builder.Build();// configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options=>
                options.Filters.Add(typeof(OpenApiAuthFilter)))
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
               .AddJsonOptions(setupAction =>
               {
                   setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                   setupAction.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
               }); 

            string connectionString = Configuration.GetConnectionString(
                "DefaultConnectionString");
            bool isDebugMode = Configuration.GetValue<bool>("IsDebugMode");
             
            //var signKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(
            //        Configuration.GetValue<string>("SigningCredentials:Key")));

            //SigningCredentials signingCredentials = new SigningCredentials(signKey,
            //    SecurityAlgorithms.HmacSha256Signature);
            //services.AddSingleton<SigningCredentials>(signingCredentials);

            //string issuer = Configuration.GetValue<string>("SigningCredentials:Issuer");
            //string audience = Configuration.GetValue<string>("SigningCredentials:Audience");

            services.AddSingleton<IOpenAppAuthService>(impl =>
            {
                return new InternalOpenAuthService("yltbook", "85959r9wz9r7rni9izo");
            });

            //EntityFramework Core UseMySQL
            //services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString));

            //Identity Framework Add JwtOptions
            //services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            //{
            //    options.Password.RequireDigit = false;
            //    options.Password.RequiredLength = 6;
            //    options.Password.RequireLowercase = false;
            //    options.Password.RequireUppercase = false;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.User.AllowedUserNameCharacters = string.Empty;
            //    options.Lockout.MaxFailedAccessAttempts = 5;
            //}).AddDefaultTokenProviders();

            //services.AddAuthentication().AddJwtBearer(
            //    options =>
            //    {
            //        options.TokenValidationParameters =
            //         new TokenValidationParameters()
            //         {
            //             ValidateIssuer = true,
            //             ValidateAudience = true,
            //             ValidateLifetime = true,
            //             ValidateIssuerSigningKey = true,
            //             ValidIssuer = issuer, 
            //             ValidAudience = audience, 
            //             IssuerSigningKey = signKey,
            //         };
            //    });
            services.AddTransient<AzureStorageProvider>();
            services.AddTransient<AliyunStorageProvider>();
            services.AddTransient<LocalStorageProvider>();

            DisconfService disconf = new DisconfService(this.Configuration);
            disconf.CustomConfigs.Add(AzureStorageProvider.STORAGE_PROVIDER_TYPE_KEY, AzureStorageProvider.STORAGE_PROVIDER_TYPE_VALUE);
            disconf.CustomConfigs.Add(AliyunStorageProvider.STORAGE_PROVIDER_TYPE_KEY, AliyunStorageProvider.STORAGE_PROVIDER_TYPE_VALUE);
            disconf.CustomConfigs.Add(LocalStorageProvider.STORAGE_PROVIDER_TYPE_KEY, LocalStorageProvider.STORAGE_PROVIDER_TYPE_VALUE);
            disconf.CustomConfigs.Add("IsDebugMode", isDebugMode);

            services.AddSingleton(disconf);

            services.AddDbContext<StaticObjectsDbContext>(options => options.UseSqlServer(connectionString));

            services.AddTransient<Abstractions.IStaticObjectsRepository>(
                new Func<IServiceProvider, Abstractions.IStaticObjectsRepository>(
                (provider) =>
                {
                    return new EfCoreStaticObjectsRepository(
                        provider.GetService<ILogger<EfCoreStaticObjectsRepository>>(),
                        provider.GetService<StaticObjectsDbContext>());
                }));
            services.AddTransient<IServiceAppService>(new Func<IServiceProvider, IServiceAppService>(
                (provider) =>
                {
                    return new ServiceAppService(provider.GetService<ILogger<ServiceAppService>>(), provider.GetService<Abstractions.IStaticObjectsRepository>(), provider);
                }));
            services.AddTransient<IStorageService>(new Func<IServiceProvider, IStorageService>(
                (provider) =>
                {
                    return new StorageService(provider.GetService<ILogger<StorageService>>(), 
                        provider.GetService<Abstractions.IStaticObjectsRepository>(), provider);
                }));
            services.AddTransient<IImageService>(new Func<IServiceProvider, IImageService>(
                (provider) =>
                {
                    return new ImageService(provider.GetService<ILogger<ImageService>>(),
                        provider.GetService<IServiceAppService>(), provider.GetService<IStorageService>(),
                        provider.GetService<Abstractions.IStaticObjectsRepository>());
                }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseCookiePolicy(); 

            //强行HTTPS
            //app.UseHttpsRedirection();

            //启用ASP.NET Core Identity
            //app.UseAuthentication();

            app.UseMvc();
        }
    }
}
