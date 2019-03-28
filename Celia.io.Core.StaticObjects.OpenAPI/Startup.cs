using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
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

            var signKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(
                    Configuration.GetValue<string>("SigningCredentials:Key")));

            SigningCredentials signingCredentials = new SigningCredentials(signKey,
                SecurityAlgorithms.HmacSha256Signature);
            services.AddSingleton<SigningCredentials>(signingCredentials);

            string issuer = Configuration.GetValue<string>("SigningCredentials:Issuer");
            string audience = Configuration.GetValue<string>("SigningCredentials:Audience"); 

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
