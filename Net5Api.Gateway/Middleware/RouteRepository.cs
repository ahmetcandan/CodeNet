using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Net5Api.MongoDB;

namespace Net5Api.Gateway.Middleware
{
    public class RouteRepository : BaseMongoRepository<RouteMongo>
    {
        public RouteRepository() : base("Routes")
        {

        }
    }
}
