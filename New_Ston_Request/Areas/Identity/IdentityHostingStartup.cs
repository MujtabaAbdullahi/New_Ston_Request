using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(New_Stone_Request.Areas.Identity.IdentityHostingStartup))]
namespace New_Stone_Request.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}