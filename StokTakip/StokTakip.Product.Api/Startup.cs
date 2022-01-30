using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetCore.Abstraction;
using NetCore.Abstraction.Model;
using NetCore.Cache;
using NetCore.Core.Aspect;
using NetCore.ExceptionHandling;
using NetCore.Logging;
using NetCore.MongoDB;
using NetCore.RabbitMQ;
using NetCore.Redis;
using StokTakip.Abstraction;
using StokTakip.EntityFramework.Models;
using StokTakip.Repository;
using StokTakip.Service;
using System.Text;

namespace StokTakip.Product.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddDbContext<StokTakipContext>(options => options.UseSqlServer(Configuration.GetConnectionString("StokTakip")));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "StokTakip.Product.Api", Version = "v1" });
            });

            // Adding Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });

            services.Configure<RedisSettings>(Configuration.GetSection("Redis"));
            services.Configure<MongoDbSettings>(Configuration.GetSection("MongoDB"));
            services.Configure<RabbitMQSettings>(Configuration.GetSection("RabbitMQ"));

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<DbContext, StokTakipContext>();
            services.AddScoped<ICacheRepository, RedisCacheRepository>();
            services.AddScoped<ILogRepository, MongoDBLogRepository>();
            services.AddScoped<IQService, RabbitMQService>();

            services.DecorateWithDispatchProxy<IProductService, CacheProxy<IProductService>>();
            services.DecorateWithDispatchProxy<IProductService, LogProxy<IProductService>>();
            services.DecorateWithDispatchProxy<IProductService, ExceptionHanlingProxy<IProductService>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Products API v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
