using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Celia.io.Core.StaticObjects.OpenAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Info("init main");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            //加入配置文件
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true);
            var configObject = config.Build();
            var fileName = configObject.GetValue<string>("SigningCredentials:FileName");
            var password = configObject.GetValue<string>("SigningCredentials:Password");
            var httpsPort = 40021;
            var httpPort = 40020;

            var tmp0 = 0;
            var tmp1 = configObject.GetValue<string>("Urls:HttpPort");
            if (!string.IsNullOrEmpty(tmp1) && int.TryParse(tmp1, out tmp0))
            {
                httpPort = tmp0;
            }
            var tmp2 = configObject.GetValue<string>("Urls:HttpsPort");
            if (!string.IsNullOrEmpty(tmp2) && int.TryParse(tmp2, out tmp0))
            {
                httpsPort = tmp0;
            }

            //var cert = new X509Certificate2(fileName, password);
            //FileInfo fileInfo = new FileInfo(fileName);
            //System.Diagnostics.Trace.TraceInformation(
            //    $"CERT fileInfo = {fileInfo.FullName} {fileInfo.Exists}");

            var builder = WebHost.CreateDefaultBuilder(args)
                .UseKestrel(cfg =>
                {
                    cfg.Limits.MaxConcurrentConnections = 100;
                    cfg.Limits.MaxConcurrentUpgradedConnections = 100;
                    cfg.Limits.MaxRequestBodySize = 10 * 1024;
                    cfg.Limits.MinRequestBodyDataRate =
                        new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                    cfg.Limits.MinResponseDataRate =
                        new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                    cfg.Listen(IPAddress.Any, httpPort);
                    //cfg.Listen(IPAddress.Any, httpsPort, listenOptions =>
                    //{
                    //    listenOptions.UseHttps(cert);
                    //});
                })
                .UseStartup<Startup>()
                .UseIISIntegration()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUrls($"http://*:{httpPort}", $"https://*:{httpsPort}")
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog(); // NLog: setup NLog for Dependency injection
            //.Build();

            return builder;
        }
    }
}
