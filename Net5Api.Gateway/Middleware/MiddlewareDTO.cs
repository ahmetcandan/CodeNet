using Net5Api.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Net5Api.Gateway.Middleware
{
    [Serializable]
    public class MiddlewareDTO
    {
        public GlobalConfigurationDTO GlobalConfiguration { get; set; }

        public List<RouteDTO> Routes { get; set; }
    }

    [Serializable]
    public class GlobalConfigurationDTO
    {
        public string BaseUrl { get; set; } 
    }

    [Serializable]
    public class RouteDTO
    {
        public string UpstreamPathTemplate { get; set; }
        public List<string> UpstreamHttpMethod { get; set; }
        public string DownstreamPathTemplate { get; set; }
        public string DownstreamScheme { get; set; }
        public List<DownstreamHostAndPortDTO> DownstreamHostAndPorts { get; set; }
    }
    public class RouteMongo : BaseMongoModel
    {
        public string UpstreamPathTemplate { get; set; }
        public List<string> UpstreamHttpMethod { get; set; }
        public string DownstreamPathTemplate { get; set; }
        public string DownstreamScheme { get; set; }
        public List<DownstreamHostAndPortDTO> DownstreamHostAndPorts { get; set; }
    }

    [Serializable]
    public class DownstreamHostAndPortDTO
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
