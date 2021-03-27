using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Net5Api.MongoDB;
using Net5Api.Gateway.Middleware;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Net5Api.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((host, config) => 
                {
                    config.AddJsonStream(getOcelonJson());
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        static Stream getOcelonJson()
        {
            var repository = new RouteRepository();
            var result = new MiddlewareDTO();
            result.GlobalConfiguration = new GlobalConfigurationDTO()
            {
                BaseUrl = "http://localhost"
            };
            result.Routes = (from c in repository.GetList()
                             select new RouteDTO
                             {
                                 DownstreamHostAndPorts = c.DownstreamHostAndPorts,
                                 DownstreamPathTemplate = c.DownstreamPathTemplate,
                                 DownstreamScheme = c.DownstreamScheme,
                                 UpstreamHttpMethod = c.UpstreamHttpMethod,
                                 UpstreamPathTemplate = c.UpstreamPathTemplate
                             }).ToList();
            return SerializeToStream(result);
        }

        static Stream SerializeToStream(object o)
        {
            Stream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, o);
            stream.Close();
            return stream;
        }
    }
}
