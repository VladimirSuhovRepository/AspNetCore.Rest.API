using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Verivox.API.Database;
using Verivox.API.Middleware;
using Verivox.API.Repositories;
using Verivox.API.Services;
using Verivox.API.UOW;

namespace Verivox.API
{
    /// <summary>
    /// Initialization class.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">Configuration for the application.</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets Configuration property for custom configs.
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ProductsDbContext>(o => o.UseInMemoryDatabase("Products"));
            services.AddScoped<IBaseTariffRepository, BaseTariffRepository>();
            services.AddScoped<IPackagedTariffRepository, PackagedTariffRepository>();
            services.AddScoped<IUnitOfWork, ProductsDbUnitOfWork>();
            services.AddScoped<IProductsService, ProductsService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMvc();
        }
    }
}
