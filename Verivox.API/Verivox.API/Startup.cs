using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Verivox.API.Database;
using Verivox.API.Middleware;
using Verivox.API.Repositories;
using Verivox.API.Services;
using Verivox.API.UOW;

namespace Verivox.API
{
    /// <summary>
    /// Initialization class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="configuration">Configuration for the application</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration property for custom configs
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ProductsDbContext>(o => o.UseInMemoryDatabase("Products"));
            services.AddScoped<IBaseTariffRepository, BaseTariffRepository >();
            services.AddScoped<IPackagedTariffRepository, PackagedTariffRepository>();
            services.AddScoped<IUnitOfWork, ProductsDbUnitOfWork>();
            services.AddScoped<IProductsService, ProductsService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseMiddleware<ExceptionMiddleware>();
            }

            app.UseMvc();
        }
    }
}
