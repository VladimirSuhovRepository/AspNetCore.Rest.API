using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Verivox.API
{
    /// <summary>
    /// Startup class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Startup poing.
        /// </summary>
        /// <param name="args">arguments for execution.</param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Creates web host for the application.
        /// </summary>
        /// <param name="args">arguments for execution.</param>
        /// <returns>Web host with application running.</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
