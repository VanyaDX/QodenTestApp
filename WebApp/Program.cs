using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
                WebHost.CreateDefaultBuilder(args)
                       .UseStartup<Startup>();

        /**
         * TODO 8: Fix issue -- implemented update, but only for cache
         * Users complains that sometimes, when they call AccountController.UpdateAccount followed by
         * AccountController.GetByInternalId they get account with counter equals 0, like if UpdateCounter was never
         * called.
         *
         * It looks like as if there were two accounts, one being updated by UpdateAccount method and another does not.
         * 
         * This issue started happening at the same time when frontend team introduced major change in their frontend
         * architecture. Entire frontend split into independent widgets, each loading it state independently from API.
         * While this introduce lot of flexibility to frontend this also means multiple widgets call same API methods
         * simultaneously after login. As a result frontend JS code make number of simultaneous queries to
         * AccountController.Get right after login.
         */
    }
}