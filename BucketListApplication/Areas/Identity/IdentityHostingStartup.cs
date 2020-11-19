using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(BucketListApplication.Areas.Identity.IdentityHostingStartup))]
namespace BucketListApplication.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}